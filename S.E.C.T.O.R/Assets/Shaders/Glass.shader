Shader "S.E.C.T.O.R/Glass" 
{
	Properties
	{
		[Header (Global settings)]
		[KeywordEnum(Off, On)] DISTORTION("Distortion", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull mode", Float) = 2
		
		_RefractionInt("Refraction intensity", Float) = 100.0
		_DistortTex ("Distortion Tex (NormalMap)", 2D) = "bump" {}
		_Smoothness ("Smoothness intensity", Range (0, .95)) = .95
		_Color ("Color RGB, Alpha A", Color) = (1,1,1,.2)
    }
 
	SubShader
	{
		GrabPass
		{
			"_BackgroundTex"
		}

		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		ZWrite On
		Cull [_CullMode]

		CGPROGRAM
		#pragma surface surf Standard alpha:fade vertex:vert
		#pragma target 5.0
		#pragma shader_feature DOUBLEFACE
		#pragma multi_compile DISTORTION_OFF	DISTORTION_ON

		sampler2D _BackgroundTex, _DistortTex, _CameraDepthTexture;
		half _RefractionInt, _FresnelPower, _Smoothness;
		half4 _BackgroundTex_TexelSize, _Color;
		
		struct Input 
		{
#if DISTORTION_ON
			half2 uv_DistortionTex;
			half4 screenPos;
			half3 viewDir;
			float eyeDepth;
#endif
            half VertexPos;
		};

        void vert (inout appdata_full v, out Input o)
        {
#if DISTORTION_ON
            UNITY_INITIALIZE_OUTPUT(Input, o);
			COMPUTE_EYEDEPTH(o.eyeDepth);
#else
			UNITY_INITIALIZE_OUTPUT(Input, o);
#endif
        }

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
#if DISTORTION_ON
			float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
			float sceneZ = LinearEyeDepth(rawZ);
			float fade = saturate(_Color.a * (sceneZ - IN.eyeDepth));

			float3 distort = UnpackNormal(tex2D(_DistortTex, IN.uv_DistortionTex));
			float2 offset = distort.xy * (_RefractionInt *500) * _BackgroundTex_TexelSize.xy;
			float2 uvScreen = IN.screenPos.xy + offset * IN.screenPos.z;
			uvScreen /= IN.screenPos.w;

			float4 refractedColor = tex2D(_BackgroundTex, uvScreen);

			o.Normal = distort;
			o.Emission = refractedColor;
#else
			o.Albedo = _Color.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = _Color.a;
#endif
		}
		ENDCG
	}
	Fallback "Diffuse"
 }