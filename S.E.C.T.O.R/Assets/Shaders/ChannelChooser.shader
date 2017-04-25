Shader "S.E.C.T.O.R/ChannelChooser" 
{
	Properties 
	{
		[Header(Textures)]
		_MainTexR ("Channel R", 2D) = "white" {}
		[NoScaleOffset]_MainTexG ("Channel G", 2D) = "white" {}
		[NoScaleOffset]_MainTexB ("Channel B", 2D) = "white" {}
		[NoScaleOffset]_MainTexA ("Channel A", 2D) = "white" {}
		[Space(10)]

		[Header(Variables)]
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque"}
		LOD 100
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 5.0

		sampler2D _MainTexR, _MainTexG, _MainTexB, _MainTexA;
		half _Glossiness, _Metallic;
		half4 _Color;

		struct Input 
		{
			float2 uv_MainTexR;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half ChannelR = tex2D (_MainTexR, IN.uv_MainTexR).r;
			half ChannelG = tex2D (_MainTexG, IN.uv_MainTexR).g;
			half ChannelB = tex2D (_MainTexB, IN.uv_MainTexR).b;
			half ChannelA = tex2D (_MainTexA, IN.uv_MainTexR).a;

			half CombChannel = ChannelR * ChannelG * ChannelB * ChannelA;

			o.Albedo = CombChannel * _Color;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = CombChannel * _Color.a;
			clip (CombChannel - _Color.a);
		}
		ENDCG

		/*Pass
		{
			Name "ShadowCaster"
			tags{ "LightMode" = "ShadowCaster" }
			Fog{ Mode Off }
			ZWrite On
			Cull Off
		
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
		
			sampler2D _MainTexR, _MainTexG, _MainTexB, _MainTexA;
			half4 _Color;

			struct appData //v.
			{
				float4 pos 	 : POSITION;
			};

			struct v2f //o.
			{
				float2 uv 	: 	ATTR0;
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.uv = v.texcoord;

				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
		
			half4 frag(v2f i) : COLOR
			{
				half4 o;

				half ChannelR = tex2D (_MainTexR, i.uv).r;
				half ChannelG = tex2D (_MainTexG, i.uv).g;
				half ChannelB = tex2D (_MainTexB, i.uv).b;
				half ChannelA = tex2D (_MainTexA, i.uv).a;

				half CombChannel = (ChannelR * ChannelG * ChannelB * ChannelA);
				o.rgb = CombChannel * _Color;
				o.a = CombChannel * _Color.a;
				clip(CombChannel - _Color.a);
				return o;

				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}*/
	}
}