Shader "S.E.C.T.O.R/PBR_Objects"
{
	Properties 
	{
		[Header (Global settings)]
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull mode", Float) = 2
		[Space(30)]
		
		[Header(Diffuse)]
		_RGB_nX ("RGB_normalX", 2D) = "Grey" {}
		[Space(5)]
		[HDR]_Color ("Ambiant", Color) = (1,1,1,1)
		[Space(30)]
		
		[Header(Metalness Gloss Emissive)]
		[NoScaleOffset]_MRE_nY ("MRE_normalY", 2D) = "Grey" {}
		[Space(10)]

		[HDR]_EmissionColor ("Emissive", Color) = (0,0,0,1)
		_Pulse ("Pulse time", Range(0,10)) = 0
		_PulsePower ("Pulse power", Range(0, 1)) = 0
		_Glossiness ("Roughness intensity", Range (0,0.97)) = .7
		_Metallic ("Metal intensity", Range (0,1)) = .8
		_NormalMapScale ("Normal map scale", Range (0,1)) = 1
		[Space(30)]
		
		[Header (Alpha)]
		[KeywordEnum(Off, On)] ALPHA("Alpha enable ?", Float) = 0
		_AlphaTex ("Alpha map", 2D) = "white" {}
		_AlphaInt ("Alpha intensity", Range(0,1)) = 1
		[Space(30)]

		[Header (Desintegrate effect)]
		[KeywordEnum(Off, On)] DESINTEGRATE("Desintegrate effect ?", Float) = 0
		_LUT ("Lut", 2D) = "white" {}
		[HDR]_EmberColor ("Ember color", Color) = (1,1,1,1)
		[Space(30)]

		[Header (Outline)]
		[KeywordEnum(Off, On)] OUTLINE("Outline ?", Float) = 0
		[HDR]_OutlineColor ("Outline color", Color) = (0,0,0,0)
		_OutlineThickness ("Outline thickness", Range(0,.1)) = .1
		[Space(30)]

		[Header(Panning)]
		_Speed ("Panning - Direction and Speed", Vector) = (0,0,0,0)
	}

	SubShader 
	{
		Name "PBR Base"
		Tags {"RenderType"="Opaque"}
		Cull [_CullMode]
		
		CGPROGRAM
		#pragma target 5.0
		#pragma only_renderers d3d11 ps4 xboxone
		#pragma surface surf Standard vertex:vert 
		#pragma multi_compile ALPHA_OFF	ALPHA_ON
		#pragma multi_compile DESINTEGRATE_OFF	DESINTEGRATE_ON
		#pragma shader_feature DOUBLEFACE

		sampler2D _RGB_nX, _MRE_nY;
		half3 _EmissionColor, _Color;
		half _Glossiness, _Metallic, _NormalMapScale, _Pulse, _PulsePower;
		half2 _Speed;

#if ALPHA_ON
		sampler2D _AlphaTex;
		half _AlphaInt;
#endif
#if DESINTEGRATE_ON
		sampler2D _LUT;
		half3 _EmberColor;
		half4 _LUT_ST;
#endif

		struct Input 
		{
			float2 uv_RGB_nX;

		#if ALPHA_ON
			float2 uv_AlphaTex;
		#endif

		#if DESINTEGRATE_ON
			float2 uv_LUT;
		#endif
		};
		
	#if ALPHA_ON
		void vert(inout appdata_full v)
		{
			float3 normalDir = mul(unity_ObjectToWorld, float4(v.normal.xyz, 0.0f)).xyz;
			float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0f)).xyz);

			if (dot(viewDir, normalDir)<0.0f)
				v.normal *= -1.0f;
		}
	#else
		void vert(inout appdata_full v)
		{
			
		}
	#endif

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			//search texture-----------------------------------------------------------------
			half2 Speed = _Time * _Speed;

			half4 RGB_nX = tex2D (_RGB_nX, (IN.uv_RGB_nX + Speed));
			half4 MRE_nY = tex2D (_MRE_nY, (IN.uv_RGB_nX + Speed));
	#if ALPHA_ON
			half AlphaTex = tex2D (_AlphaTex, IN.uv_AlphaTex + Speed);
	#endif	

	#if ALPHA_ON
		#if DESINTEGRATE_ON

			//Calcul-------------------------------------------------------------------------
			float CutOff = (((1 - _AlphaInt) * 2.7 + -2) + AlphaTex.r);

			float2 CutOffLut = float2 ((1.0 - (saturate((CutOff * 6.0 + -3.0))) * .9), 0);
			float4 EmberLut = tex2D(_LUT, TRANSFORM_TEX(CutOffLut, _LUT));
			float3 DesintegrateEffect = EmberLut.rgb * _EmberColor;
		#endif
	#endif
			//outpout------------------------------------------------------------------------
			//o.Normal = UnpackScaleNormal(half4(0,MRE_nY.a,0,RGB_nX.a), _NormalMapScale);
			half3 normal = half3(RGB_nX.a, MRE_nY.a, sqrt(RGB_nX.a + MRE_nY.a)) * 2 - 1;
			half3 albedo = RGB_nX.rgb * _Color;

			half smoothness = ((1 - MRE_nY.g) * (1 - MRE_nY.b)) * _Glossiness;
			half metallic = MRE_nY.r * _Metallic;

			half3 emission = (MRE_nY.b * RGB_nX.rgb) * _EmissionColor;
			half Pulse = MRE_nY.b * (1 + sin(_Time.y * _Pulse));
			emission += (Pulse * _PulsePower) * (RGB_nX.rgb *.5);

	#if ALPHA_ON
		#if DESINTEGRATE_ON
			emission += DesintegrateEffect;
		#endif
	#endif

			o.Normal = normal;
			o.Albedo = albedo;
			o.Smoothness = smoothness;
			o.Metallic = metallic;
			o.Emission = emission;

	#if ALPHA_ON
			//o.Alpha = AlphaTex * _AlphaInt;
			clip (AlphaTex - _AlphaInt);
	#else
			o.Alpha = 1;
	#endif
		}
		ENDCG

		Name "Outline"
		Tags{ "RenderType" = "Opaque" }
		Cull front

		CGPROGRAM
		#pragma target 5.0
		#pragma only_renderers d3d11 ps4 xboxone
		#pragma surface surf Lambert noshadow vertex:vert
		#pragma multi_compile OUTLINE_OFF	OUTLINE_ON
		#pragma multi_compile ALPHA_OFF	ALPHA_ON

#if OUTLINE_ON
		half4 _OutlineColor;
		half _OutlineThickness;

	#if ALPHA_ON
		sampler2D _AlphaTex;
		half _AlphaInt;
	#endif

		struct Input
		{
			half3 uv_AlphaTex;
		};

		void vert(inout appdata_full v)
		{
			v.vertex.xyz += v.normal * _OutlineThickness;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
	#if ALPHA_ON
			half AlphaTex = tex2D(_AlphaTex, IN.uv_AlphaTex).r;
			//clip(AlphaTex - _AlphaInt);
	#endif
			o.Emission = _OutlineColor;
		}
#else

		struct Input
		{
			half2 Time;
		};

		void vert(inout appdata_full v)
		{

		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			clip(0 - 1);
		}
#endif
		ENDCG

		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			Fog{ Mode Off }
			ZWrite On 
			ZTest LEqual 
			Cull back
		
			CGPROGRAM
			#pragma target 5.0
			#pragma only_renderers d3d11 ps4 xboxone
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile ALPHA_OFF	ALPHA_ON
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
		
#if ALPHA_ON
			sampler2D _AlphaTex;
			half4 _AlphaTex_ST;
			half _AlphaInt;
		
			struct pixelInput
			{
				float2 uv0 : TEXCOORD0;
				V2F_SHADOW_CASTER;
			};
		
			pixelInput vert(appdata_base v)
			{
				pixelInput o;
				o.uv0 = v.texcoord;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
		
			half4 frag(pixelInput i) : COLOR
			{
				half cutout = tex2D(_AlphaTex, TRANSFORM_TEX(i.uv0.rg, _AlphaTex));
				clip(cutout - _AlphaInt);
				SHADOW_CASTER_FRAGMENT(i)
			}
#else
			struct pixelInput
			{
				V2F_SHADOW_CASTER;
			};
		
			pixelInput vert(appdata_base v)
			{
				pixelInput o;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
		
			half4 frag(pixelInput i) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
#endif
		ENDCG
		}
	}
	Fallback "VertexLit"
	//CustomEditor "CustomEditor_PBR_Objects"
 }