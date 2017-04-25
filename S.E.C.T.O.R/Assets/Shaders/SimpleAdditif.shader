Shader "S.E.C.T.O.R/SimpleAdditif"
{
Properties 
	{
		[Header (Global settings)]
		[KeywordEnum(Off, On)] BORDERBLUR ("Border blur enable ?", Float) = 0
		[KeywordEnum(Off, On)] ONLYBLOOM ("Only bloom enable ?", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull mode", Float) = 2
		[Space (10)]
		
		[Header (Object settings)]
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SideInt ("Side alpha intensity", Range (0,1)) = 1
        _Speed ("Speed and direction", Vector) = (0,0,0,0)
	}

	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		LOD 200
		Cull [_CullMode]
		Blend One One
		Zwrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert 
		#pragma target 5.0
		#pragma multi_compile BORDERBLUR_OFF	BORDERBLUR_ON
		#pragma multi_compile ONLYBLOOM_OFF	ONLYBLOOM_ON
		#pragma shader_feature DOUBLEFACE

		sampler2D _MainTex;
		half4 _Color;
        half2 _Speed;
#if BORDERBLUR_ON
		half _SideInt;
#endif

		struct Input 
		{
			half2 uv_MainTex;
			half3 VColor	:	COLOR;
#if BORDERBLUR_ON
			half3 viewDir;
#endif
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
#if ONLYBLOOM_OFF
            half2 Time = _Speed * _Time;
			half3 Diffuse = tex2D(_MainTex, IN.uv_MainTex + Time) * _Color.rgb * _Color.a;

	#if BORDERBLUR_ON
			half3 N = o.Normal;
			half3 V = IN.viewDir;
			half NdotV = saturate(dot(N,V));
			NdotV = smoothstep(0, _SideInt, NdotV);
			
			half3 Emissive = Diffuse * NdotV;
	#else
			half3 Emissive = Diffuse;
	#endif
#else
			half3 Emissive = _Color.rgb;
#endif
			o.Emission = Emissive * IN.VColor;


		}
		ENDCG
	}
}