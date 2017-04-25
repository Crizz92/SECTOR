Shader "S.E.C.T.O.R/VertexPaint_2Textures"
{
	Properties 
	{	
        [Header(1st tex)]
		[Header(Diffuse)]
		_RGB_nX ("RGB_normalX", 2D) = "grey" {}
		[Space(5)]
		_Color ("Ambiant", Color) = (1,1,1,1)
		[Space(10)]
		
		[Header(Metalness Gloss Emissive)]
		[NoScaleOffset]_MRH_nY ("MRH_normalY", 2D) = "grey" {}
		[Space(5)]
        
		_Glossiness ("Roughness intensity", Range (0,1)) = .95
        [Space(15)]

        [Header(2nd tex)]
        [Header(Diffuse)]
        _RGB_nX2 ("RGB_normalX", 2D) = "grey" {}
        [Space(5)]
        _Color2 ("Ambiant", Color) = (1,1,1,1)
        [Space(10)]
        
        [Header(Metalness Gloss Emissive)]
        [NoScaleOffset]_MRH_nY2 ("MRH_normalY", 2D) = "grey" {}
        [Space(5)]
        
        _Glossiness2 ("Roughness intensity", Range (0,1)) = .95
	}

	SubShader 
	{
	  Tags {"RenderType"="Opaque"}

		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Standard fullforwardshadows
	
		sampler2D _RGB_nX, _MRH_nY, _RGB_nX2, _MRH_nY2;
		half4 _Color, _Color2;
		half _Glossiness, _Glossiness2;
	
		struct Input 
		{
			float2 uv_RGB_nX, uv_RGB_nX2;
			half3 color : COLOR;
		};
	
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
            //Mask blend---------------------------------------------------------------------
            half HeightR = IN.color.r - tex2D (_MRH_nY, IN.uv_RGB_nX).b;
			half BlendMaskR = step (0, HeightR);

            //search texture-----------------------------------------------------------------
    		half4 RGB_nX = tex2D(_RGB_nX, IN.uv_RGB_nX) * _Color;
    		half4 MRH_nY = tex2D (_MRH_nY, IN.uv_RGB_nX);

            half4 RGB_nX2 = tex2D(_RGB_nX2, IN.uv_RGB_nX2) * _Color2;
            half4 MRH_nY2 = tex2D (_MRH_nY2, IN.uv_RGB_nX2);

            //Calcul-------------------------------------------------------------------------
            half3 Normalmap = half3(RGB_nX.a, MRH_nY.a, sqrt(RGB_nX.a + MRH_nY.a)) * 2 - 1;
            half3 Normalmap2 = half3(RGB_nX2.a, MRH_nY2.a, sqrt(RGB_nX2.a + MRH_nY2.a)) * 2 - 1;

            half Roughness = (1-MRH_nY.g) * _Glossiness;
            half Roughness2 = (1-MRH_nY2.g) * _Glossiness2;
			
			//Output-------------------------------------------------------------------------
			o.Normal = lerp(Normalmap, Normalmap2, BlendMaskR); 
			o.Albedo = lerp(RGB_nX, RGB_nX2, BlendMaskR);
			o.Smoothness = lerp(Roughness, Roughness2, BlendMaskR);
		}
		ENDCG
	}
	Fallback "VertexLit"
}