Shader "S.E.C.T.O.R/HologramEffect" 
{
	Properties 
	{
		[Header (Global settings)]
		[KeywordEnum(Off, On)] BORDERBLUR ("Border blur enable ?", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull mode", Float) = 2
		[Space (10)]

		[Header(Textures)]
		[Header(Color)]
		[NoScaleOffset] _ColorTex ("RGB -> ColorTex, A -> Emissive", 2D) = "white" {}
		[Space(15)]

		[HDR]_Color ("Ambiant", Color) = (1,1,1,1)
		[Space(15)]
		
		[Header(LUT)]
		_LUT ("Look up for hologram effect", 2D) = "white" {}
		_OffsetLUT ("Offset LUT", Range (0,2)) = 2
		[Space(15)]

		[Header(Variables)]
		[Header(RimLight)]
		[HDR]_RimLightColor ("RimLight Color", Color) = (1,1,1,1)
		[Space(15)]
		
		[Header(Alpha)]
		_AlphaInt ("Alpha intensity", Range (0,1.5)) = 2
		_BlinkInt ("Blink intensity", Range (0,.3)) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="Transparent-10" "Queue" = "Transparent-10"}
		Cull [_CullMode]
		Blend One One
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 5.0
		#pragma multi_compile BORDERBLUR_OFF	BORDERBLUR_ON
		#pragma shader_feature DOUBLEFACE

		sampler2D _ColorTex, _LUT;
		half3 _RimLightColor;
		half _AlphaInt, _BlinkInt, _OffsetLUT;
		half4 _LUT_ST, _Color;

		struct Input 
		{
			half2 uv_ColorTex, uv_LUT;
			half3 viewDir;
			half3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			//Search maps------------------------------------------------------------------
			half3 Albedo = (tex2D (_ColorTex, IN.uv_ColorTex).rgb);

			//X -> LUT.r
			//Y -> LUT.g
			half2 LUTUV1 = ((IN.worldPos.y + _OffsetLUT) * _LUT_ST.x) + (_Time * 2);
			half2 LUTUV2 = ((IN.worldPos.y + _OffsetLUT) * (_LUT_ST.y * .1)) + _Time;
			half2 LUTUV3 = ((IN.worldPos.y + _OffsetLUT) * (_LUT_ST.z * .01)) + _Time;
			half4 LUT = tex2D (_LUT, LUTUV1);
			half4 LUT2 = tex2D (_LUT, LUTUV2);
			half4 LUT3 = tex2D (_LUT, LUTUV3) * _BlinkInt;

			//Calcul-----------------------------------------------------------------------
#if BORDERBLUR_ON
			half3 N = o.Normal;
			half3 V = IN.viewDir;
			half NdotV = saturate(dot(N,V));

			half3 RimLight = (1-NdotV) * _RimLightColor;
			half AlphaCenter = smoothstep (0, _AlphaInt, 1-NdotV);

            half3 Diffuse = (Albedo * _Color.rgb) + RimLight * (LUT.r * LUT2.g);
            Diffuse *= _Color.a * AlphaCenter;
#else
			half3 Diffuse = (Albedo * _Color.rgb) * (LUT.r * LUT2.g);
            Diffuse *= _Color.a;
#endif
			//Output-----------------------------------------------------------------------
			o.Emission = Diffuse * (((LUT.r * 2) * LUT2.g) + LUT3.g);
		}
		ENDCG
	}
}