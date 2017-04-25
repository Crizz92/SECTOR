Shader "S.E.C.T.O.R/Foreground_opaque" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal]_BumpMap ("NormalMap", 2D) = "bump" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 5.0

		sampler2D _MainTex, _BumpMap;
		half4 _Color, _EmissionColor;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 Diffuse = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = Diffuse.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		}
		ENDCG
	}
	FallBack "Diffuse"
}