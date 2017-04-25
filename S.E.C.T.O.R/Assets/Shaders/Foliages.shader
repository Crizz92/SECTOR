Shader "S.E.C.T.O.R/Foliages" 
{
	Properties 
	{
		[Header(Textures)]
		[NoScaleOffset] _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" {}
		[Normal][NoScaleOffset] _BumpMap ("Normal map", 2D) = "bump" {}
		[Space(10)]

		[Header(Variables)]
		_AmbiantPrincipal ("Ambiant principal", Color) = (1,1,1,1)
		_AmbiantBackface ("Ambiant backface", Color) = (1,1,1,1)
		_AlphaInt ("Alpha intensity", Range (0.1,.95)) = .5
		_WindScale ("Wind scale", Range (0,2)) = 1
		_WindSpeed ("Wind speed", Range (0,5)) = 1
	}

	SubShader 
	{
		Tags {"RenderType"="Opaque"} 
		LOD 200
		Cull front

		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 5.0

		sampler2D _MainTex, _BumpMap, _WindNoise;
		half3 _AmbiantPrincipal, _AmbiantBackface;
		half _AlphaInt, _WindScale, _WindSpeed, _WindNoiseTile;

		struct Input 
		{
			half2 uv_MainTex, uv_BumpMap;
			half3 Color	: COLOR;
		};

		void vert(inout appdata_full v) 
		{
			half WindSpeed = _WindSpeed;

			half3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;

			half3 offset = half3 (sin (worldPosition.x + (_Time.y * WindSpeed) * _WindScale),
									0,
									sin (worldPosition.y + (_Time.y * WindSpeed) * .67) )
									* _WindScale;

			v.vertex.xyz += offset * v.color;

			v.normal *= -1;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half4 diffuse = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = (diffuse.rgb * _AmbiantPrincipal) * _AmbiantBackface;

			o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_BumpMap));
			o.Alpha = diffuse.a;
			clip (diffuse.a - _AlphaInt);
		}
		ENDCG

		Cull back
		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 5.0

		sampler2D _MainTex, _BumpMap, _WindNoise;
		half3 _AmbiantPrincipal;
		half _AlphaInt, _WindScale, _WindSpeed, _WindNoiseTile;

		struct Input 
		{
			half2 uv_MainTex, uv_BumpMap;
			half3 Color	: COLOR;
		};

		void vert(inout appdata_full v)
		{
			half WindSpeed = _WindSpeed;

			half3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;

			half3 offset = half3 (sin (worldPosition.x + (_Time.y * WindSpeed) * _WindScale),
                                    0,
                                    sin (worldPosition.y + (_Time.y * WindSpeed) * .67) )
                                    * _WindScale;

            v.vertex.xyz += offset * v.color;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half4 diffuse = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = (diffuse.rgb * _AmbiantPrincipal);

			o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_BumpMap));
			o.Alpha = diffuse.a;
			clip (diffuse.a - _AlphaInt);
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
		
			sampler2D _MainTex, _WindNoise;
			half _AlphaInt, _WindScale, _WindSpeed, _WindNoiseTile;

			struct appData //v.
			{
				half4 pos 	 : POSITION;
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

				half WindSpeed = _WindSpeed;

				half3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;

				half3 offset = half3 (sin (worldPosition.x + (_Time.y * WindSpeed) * _WindScale),
                                    0,
                                    sin (worldPosition.y + (_Time.y * WindSpeed) * .67) )
                                    * _WindScale;

				v.vertex.xyz += offset * v.color;

				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
		
			half4 frag(v2f i) : COLOR
			{
				half cutout = tex2D(_MainTex, i.uv).a;
				clip (cutout - _AlphaInt);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	FallBack "Diffuse/VertexLit"
}