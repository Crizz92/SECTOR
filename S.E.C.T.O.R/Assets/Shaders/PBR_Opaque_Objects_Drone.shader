Shader "S.E.C.T.O.R/PBR_Opaque_Objects_Drone"
{
	Properties 
	{
		[Header(Diffuse)]
		[NoScaleOffset] _RGB_nX ("RGB_normalX", 2D) = "Grey" {}
		[Space(5)]
		[HDR]_Color ("Ambiant", Color) = (1,1,1,1)
		[Space(10)]

		[Header(Metalness Gloss Emissive)]
		[NoScaleOffset] _MRE_nY ("MRE_normalY", 2D) = "Grey" {}
		[Space(5)]

		[HDR]_EmissionColor ("Emissive", Color) = (0,0,0,1)
		_Glossiness ("Roughness intensity", Range (0,0.95)) = .95
		_Metallic ("Metal intensity", Range (0,1)) = 1
		_Pulse ("Pulse emissive", Range (0,2.5)) = 0
		_NormalMapScale ("Normal map scale", Range (0,1)) = 1
        [Space(10)]

        [Header(Outline)]
		_OutlineColor ("Outline color", Color) = (0,0,0,0)
        _OutlineThickness ("Outline thickness", Range (0,1)) = 1
		[Space(5)]
		_InnerOutline ("Inner outline thickness", Range (0,1)) = .75
		_InnerIntensity ("Inner outline intensity", Range(0,1)) = .75
        [Space(10)]

		[Header(Mask)]
		[NoScaleOffset]_MaskColor ("Mask color drone", 2D) = "black" {}
		_ColorDrone ("Color drone", Color) = (0,0,0,0)
	}

	SubShader 
	{	
	  	Tags {"RenderType"="Opaque"}

		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Standard fullforwardshadows vertex:vert
	
		sampler2D _RGB_nX, _MRE_nY, _MaskColor;
		half3 _EmissionColor, _Color, _ColorDrone, _OutlineColor;
		half _Glossiness, _Metallic, _NormalMapScale, _Pulse, _InnerOutline, _InnerIntensity;

		struct Input 
		{
			float2 uv_RGB_nX;
			half3 viewDir;
			half3 normal;
		};
	
		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.normal = v.normal;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			//search texture-----------------------------------------------------------------	
			half4 RGB_nX = tex2D(_RGB_nX, IN.uv_RGB_nX);
			half4 MRE_nY = tex2D (_MRE_nY, IN.uv_RGB_nX);
			half3 Mask = tex2D (_MaskColor, IN.uv_RGB_nX) * _ColorDrone;

			//outpout------------------------------------------------------------------------
			//o.Normal = UnpackScaleNormal(half4(0,MRE_nY.a,0,RGB_nX.a), _NormalMapScale);
			o.Normal = half3(RGB_nX.a, MRE_nY.a, sqrt(RGB_nX.a + MRE_nY.a)) * 2 - 1;

			half3 Albedo = ((RGB_nX.rgb + Mask) * _Color);
			o.Albedo = Albedo;
			o.Smoothness = ((1-MRE_nY.g) * (1-MRE_nY.b)) * _Glossiness;
			o.Metallic = MRE_nY.r * _Metallic;

			half Pulse = (1 + sin(_Time.y * _Pulse)) * .5;
			o.Emission = (((MRE_nY.b * RGB_nX.rgb) * _EmissionColor) * Pulse) + (Albedo * .2);
		}
		ENDCG
	}
	Fallback "VertexLit"
 }