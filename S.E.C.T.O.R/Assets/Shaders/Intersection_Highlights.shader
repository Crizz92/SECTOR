Shader "S.E.C.T.O.R/Intersection_Highlights" 
{
	Properties
	{
		[Header (Global settings)]
		[KeywordEnum(Off, On)] BORDERBLUR ("Border blur enable ?", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull mode", Float) = 2
		[Space (10)]
		
        [Header(Debug)]
        _CameraBug ("_CameraBug", Range(0,1)) = .9
        [Space(10)]

		[Header(Texture)]
		_MainTex ("Albedo -> RGB, Alpha -> A", 2D) = "black" {}
		_PatternTex ("Pattern in hightlight", 2D) = "white" {}
		[Space(10)]

		[Header(Variables)]
		[HDR]_RegularColor ("Regular color", Color) = (1, 1, 1, .5)
		_HiglightInt ("Highlight intensity", Float) = 1
		_HighlightThresholdMax ("Highlight threshold", Float) = 1
		_PatternInt ("Pattern intensity", Float) = .5
		_AlphaCenter ("Alpha center intensity", Range (0,1)) = .5
		_FadeY("FadeY", Range(0,1)) = 1
		_SpeedInt ("Speed direction & time", Vector) = (0,0,0,0)
	}
	
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType"="Transparent"}
		ZWrite Off
		Cull [_CullMode]
        Blend One One
	
		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Lambert vertex:vert
		#pragma multi_compile BORDERBLUR_OFF	BORDERBLUR_ON
		#pragma shader_feature DOUBLEFACE
		#include "UnityCG.cginc"
		
		sampler2D _PatternTex, _MainTex, _CameraDepthTexture;
		half4 _RegularColor;
		half2 _SpeedInt;
		half _HighlightThresholdMax, _HiglightInt, _AlphaCenter, _PatternInt, _CameraBug, _FadeY;

		struct Input
		{
			float2 uv_PatternTex, uv_MainTex;
			float4 pos : POSITION;
			half4 projPos : TEXCOORD1;
			half3 viewDir;
		};

		void vert(inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.projPos = ComputeScreenPos(o.pos);
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half2 Speed = _Time * _SpeedInt;
			half UVFade = IN.uv_MainTex.y + (( 1- IN.uv_MainTex.y) * _FadeY);

			//Texture---------------------------------------------------------------------------
			half3 Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _RegularColor.rgb;
			half4 PatternHighlight = tex2D (_PatternTex, IN.uv_PatternTex + Speed).r * (_RegularColor + .3);

			//Calcul----------------------------------------------------------------------------
			half4 finalColor = _RegularColor;
			finalColor += ((PatternHighlight * _RegularColor) * (_PatternInt * .1));

			half4 HighlightColor = _RegularColor * _HiglightInt;
			HighlightColor += (PatternHighlight * _PatternInt);
#if BORDERBLUR_ON
			half3 N = o.Normal;
			half3 V = IN.viewDir;
			half NdotV = saturate (dot (N,V));
			half3 NdotVExt = smoothstep (0, _AlphaCenter * 1.5	, NdotV) * _RegularColor.a; 
#endif
			//Get the distance to the camera from the depth buffer for this point
			half sceneZ = LinearEyeDepth (tex2Dproj (_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos)).r);
			
			//Actual distance to the camera
			half partZ = IN.projPos.a;
			
			//If the two are similar, then there is an object intersecting with our object
			half diff = (abs(sceneZ - partZ)) / _HighlightThresholdMax;
			
			if(diff <= _CameraBug)
			{
				finalColor = lerp(HighlightColor, _RegularColor, half4(diff, diff, diff, diff));
			}
#if BORDERBLUR_ON
			o.Emission = (finalColor.rgb * NdotVExt + Albedo) * UVFade;
#else
			o.Emission = (finalColor.rgb + Albedo) * UVFade;
#endif
		}
		ENDCG
	}
}