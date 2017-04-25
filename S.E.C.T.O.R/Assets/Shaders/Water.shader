Shader "S.E.C.T.O.R/Water" 
{
	Properties 
	{
        [Header(TEMP)]
        _CameraBug ("_CameraBug", Range(0,1)) = .9
		[Space(10)]

		[Header(Global settings)]
		[KeywordEnum(Off, On)] DISTORTION("Distortion", Float) = 0
        [Space(10)]
		
		[Header(Color and normal)]
		[Header(Textures)]
		_MainTex ("Diffuse", 2D) = "white" {}
		[Normal] _NormalTex ("Normal map", 2D) = "bump" {}
		[Space(5)]
		
		[Header(Variables)]
		_Color ("Ambiant", Color) = (1,1,1,1)
		_Smoothness ("Gloss", Range(0.8,.95)) = 0.9
		_Metalness ("Fake metal", Range (0,1)) = 0
		_RefractionInt ("Refraction intensity", Float) = 50
		_FadeRefraction ("Fade refraction intensity", Range (0,1)) = 1
		[Space(15)]
		
		[Header(Displacement)]
		[Header(Textures)]
		_DisplacementMap ("Displacement", 2D) = "white" {}
		[Space(5)]
		
		[Header(Variables)]
		_DisplacementInt ("Displacement", Range (0,.25)) = .1
		_Speed ("Speed (XY) (ZW)", Vector) = (.8,-.3,.2,.3)
		_SpeedInt ("Speed intensity", Range (0,3)) = 1
		[Space(15)]
		
		[Header(Border)]
        _HighlightThresholdMax ("Pattern threshold", Range(0,5)) = 1
	}

	SubShader 
	{
		GrabPass
		{
			"_BackgroundTex"
		}

		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

		CGPROGRAM
		#pragma surface surf Standard vertex:vert alpha:fade
		#pragma target 5.0
		#pragma multi_compile DISTORTION_OFF	DISTORTION_ON
		#include "UnityCG.cginc"

		half _Smoothness, _DisplacementInt, _SpeedInt, _HighlightThresholdMax, _CameraBug, _Metalness, _RefractionInt, _FadeRefraction;
		half4 _Speed, _Color, _DisplacementMap_ST, _BackgroundTex_TexelSize;
		sampler2D _NormalTex, _DisplacementMap, _MainTex, _CameraDepthTexture, _BackgroundTex;

		struct Input 
		{
			float2 uv_MainTex, uv_NormalTex, uv_DisplacementMap;
            float4 pos : POSITION;
            float4 projPos : TEXCOORD0;
#if DISTORTION_ON
			half4 screenPos;
			half3 viewDir;
			float eyeDepth;
#endif
		};

		void vert (inout appdata_full v, out Input o)
		{
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
            o.projPos = ComputeScreenPos(o.pos);

			half4 Speed = _Time * (_Speed * _SpeedInt) * (_DisplacementInt * .5);

			half displacementmap = ((tex2Dlod (_DisplacementMap, (float4 ((v.texcoord.xy * _DisplacementMap_ST) + (Speed.xy * 3) ,0,0)))) * 2-1) * _DisplacementInt;
			half displacementmap2 = ((tex2Dlod (_DisplacementMap, (float4 ((v.texcoord.xy * _DisplacementMap_ST) + (Speed.zw * 3) ,0,0)))) * 2-1) * _DisplacementInt;
			v.vertex.y += (displacementmap + displacementmap2);
#if DISTORTION_ON
			COMPUTE_EYEDEPTH(o.eyeDepth);
#endif
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half4 Speed = _Time * (_Speed * _SpeedInt) * (_DisplacementInt * .5);

            //Textures---------------------------------------------------------------------------
			half3 Normal1 = UnpackNormal (tex2D(_NormalTex, IN.uv_NormalTex + Speed.xy));
			half3 Normal2 = UnpackNormal (tex2D(_NormalTex, (IN.uv_NormalTex *.8) + Speed.zw));
            
			half4 Color = tex2D (_MainTex, IN.uv_MainTex);
            Color.rgb += (Color.a * 3);

			//Transparent intersection----------------------------------------------------------
            half4 finalColor = 0;
            half4 HighlightColor = 1;
			
            half sceneZ = LinearEyeDepth (tex2Dproj (_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos)).r);
            half partZ = IN.projPos.a;
            half diff = (abs(sceneZ - partZ)) / _HighlightThresholdMax;
            
            if(diff <= _CameraBug)
            {
                finalColor = lerp(HighlightColor, 0, half4(diff, diff, diff, diff));
            }
			
			//GrabPass---------------------------------------------------------------------------
#if DISTORTION_ON
			float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
			float sceneZGP = LinearEyeDepth(rawZ);
			float fade = saturate(_Color.a * (sceneZGP - IN.eyeDepth));

			float2 offset = (Normal1 + Normal2) * (_RefractionInt * 10) * _BackgroundTex_TexelSize.xy;
			float2 uvScreen = IN.screenPos.xy + offset * IN.screenPos.z;
			uvScreen /= IN.screenPos.w;

			float4 refractedColor = tex2D(_BackgroundTex, uvScreen);

			o.Emission = refractedColor * _FadeRefraction;
			o.Albedo = (Color.rgb  * _Color.rgb);
			o.Normal = (Normal1 + Normal2);
			o.Smoothness = _Smoothness;
			o.Metallic = _Metalness;
			o.Alpha = (1 - finalColor) * _Color.a;
#else
			//Output---------------------------------------------------------------------------
			o.Albedo = (Color.rgb  * _Color.rgb);
			o.Normal = Normal1 + Normal2;
			o.Smoothness = _Smoothness;
			o.Metallic = _Metalness;
			o.Alpha = (1 - finalColor) * _Color.a;
#endif
		}
		ENDCG
	}
} 