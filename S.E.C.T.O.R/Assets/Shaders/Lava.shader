Shader "S.E.C.T.O.R/Lava" 
{
	Properties 
	{
		[Header(Textures)]
		[NoScaleOffset] _MainTex ("Diffuse map", 2D) = "white" {}
		[Space(5)]

		[Header(Variables)]
		[HDR]_Ambiant ("Ambiant", Color) = (1,1,1,1)
		[HDR]_EmissionColor ("Emissive intensity", Color) = (0,0,0,0)
		_DisplacementInt ("Displacement intensity", Range(0,1)) = 1
		_FlowSpeed ("Flow speed", Range(0,2)) = 1
		_FlowInt ("Flow intensity", Range(0,2)) = 1
		_PulseInt ("Pulse emissive intensity", Range (0,1.5)) = .5
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 5.0
		#include "UnityCG.cginc"

		half _FlowSpeed, _FlowInt, _PulseInt;
		half4 _Ambiant, _Speed;
		half3 _EmissionColor;
		sampler2D _MainTex;

		struct Input 
		{
			half2 uv_MainTex, uv_Rocks, uv_FlowMap;
			float4 color : COLOR;
		};

		void Flow(float2 uv, float2 flow, float speed, float intensity, float noise, out float2 uv1, out float2 uv2, out float interp)
      	{
        	float2 flowVector = (flow * 2.0 - 1.0) * intensity;
        	
        	float timeScale = _Time.y * speed + noise;
        	float2 phase;
        	
        	phase.x = frac(timeScale);
        	phase.y = frac(timeScale + 0.5);
  	  	
        	uv1 = (uv - flowVector * half2(phase.x, phase.x));
        	uv2 = (uv - flowVector * half2(phase.y, phase.y));
        	
        	interp = abs(0.5 - phase.x) / 0.5;
     	}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float2 uv1;
			float2 uv2;
			float interp;

			float noise = 1;
			Flow(IN.uv_MainTex, IN.color.rg, _FlowSpeed, _FlowInt, noise, uv1, uv2, interp);

			half4 Diffuse1 = tex2D (_MainTex, uv1);
			half4 Diffuse2 = tex2D (_MainTex, uv2);

			float4 DiffuseBlend = lerp (Diffuse1, Diffuse2, interp);

			o.Albedo = DiffuseBlend.rgb * _Ambiant;

			float Pulse = (2 + sin(_Time.y * _PulseInt)) * .5;
			o.Emission = ((DiffuseBlend.rgb * DiffuseBlend.a) * _EmissionColor) * Pulse;

		}
		ENDCG
	}
	FallBack "VertexLit"
}