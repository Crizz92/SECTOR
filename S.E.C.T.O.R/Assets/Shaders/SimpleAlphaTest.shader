Shader "S.E.C.T.O.R/SimpleAlphaTest" 
{
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_AlphaInt ("Alpha intensity", Range (0,0.9999)) = .99
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader 
    {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard
		#pragma target 5.0

        sampler2D _MainTex;
        half _Glossiness, _Metallic, _AlphaInt;
        half3 _Color;
        
        struct Input 
        {
        	float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
        	fixed4 color = tex2D (_MainTex, IN.uv_MainTex);

        	o.Albedo = color.rgb * _Color.rgb;
        	o.Metallic = _Metallic;
        	o.Smoothness = _Glossiness;

            o.Alpha = color.a;
            clip (color.a - (1-_AlphaInt));
        }
		ENDCG
		
		Pass
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
		
			sampler2D _MainTex;
			half _AlphaInt;

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
				half color = tex2D(_MainTex, i.uv).a;
				clip (color - (1-_AlphaInt));
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}