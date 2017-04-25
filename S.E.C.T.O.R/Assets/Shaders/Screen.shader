Shader "S.E.C.T.O.R/Screen" 
{
	Properties 
	{
		[Header(Textures)]
		[NoScaleOffset] _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Lut1 ("Look Up Texture 1", 2D) = "white" {}
		//_Lut2 ("Look Up Texture 2", 2D) = "white" {}
		//_SquareGlitch ("Square Glitch", 2D) = "white" {}
		_Ambiant ("Ambiant", Color) = (1,1,1,1)
		[Space(10)]

		[Header(Variable)]
		_Glossiness ("Smoothness", Range(0,.98)) = 0.5
		[Space(10)]

		[Header(Maps intensity)]
		_BlackLinesInt ("Black lines intensity", Range (0,.1)) = .05
		_WhiteLinesInt ("White lines intensity", Range (0,1)) = .5
		[Space(10)]

		[Header(Glitch intensity)]
		_BarrelRollGlitchInt ("Barrel roll intensity", Range (0,3)) = .5
		_BlackLinesGlitchInt ("Black lines intensity", Range (0,3)) = .5
		_WhiteLinesGlitchInt ("White lines intensity", Range (0,1)) = .5
		_DistoGlitchInt ("Disto intensity", Range (0,1)) = .5
		[Space(10)]

		[Header(Time)]
		_BarrelRollTime("Barrel roll time", Range(0,3)) = 1
		_BlackLinesTime("Black lines time", Range(-2,2)) = 1
		_WhiteLinesTime("White lines time", Range(-2,2)) = 1
		_DistoTime("Disto time", Range(0,2)) = 1
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 5.0

			sampler2D _MainTex, _Lut1;//, _Lut2, _SquareGlitch;
		half _Glossiness, _WhiteLinesGlitchInt, _BarrelRollGlitchInt, _BlackLinesGlitchInt, _DistoGlitchInt, _BarrelRollTime, _BlackLinesTime, _WhiteLinesTime, _DistoTime, _BlackLinesInt, _WhiteLinesInt;
		half3 _Ambiant;
		half4 _Lut1_ST, _Lut2_ST;

		struct Input 
		{
			float2 uv_MainTex;
			half3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float Time = _Time.x;

			//Import textures------------------------------------------------------
			//Lut1--------------
			//LutR -> Barrel roll
			//LutG -> Black lines -> X
			//LutB -> White lines -> Y
			//LutA -> Disto
			half Lut1R = tex2D (_Lut1, half2 (fmod (Time * _BarrelRollTime , 4.0) ,0.5) ).r;
			half Lut1G = tex2D (_Lut1, (IN.worldPos.yx * (_Lut1_ST.x * .01)) + (Time * _BlackLinesTime)).g;
			half Lut1B = tex2D (_Lut1, (IN.worldPos.yx * (_Lut1_ST.y * .01)) + (Time * _WhiteLinesTime)).b;
			half Lut1A = tex2D (_Lut1, half2 (fmod (Time * _DistoTime , 4.0) ,0.5) ).a;

			//Lut2--------------
			//half3 Lut2 = tex2D (_Lut1, IN.worldPos.yx * _Lut2_ST.x).rgb;

			//Calcul---------------------------------------------------------------
			//Lut1--------------
			half LutBarrelRoll = (Lut1R * 2-1) * _BarrelRollGlitchInt;
			half LutBlackLines = Lut1G * _BlackLinesInt;
			half LutWhiteLines = (Lut1B * 2-1) *_WhiteLinesInt;
			half LutDisto = (Lut1A * 2-1) * _DistoGlitchInt;

			//Result---------------------------------------------------------------
			half colorR = tex2D (_MainTex, half2 (IN.uv_MainTex.x + ((LutDisto * 2) *
			((LutBlackLines * _BlackLinesGlitchInt) + (LutWhiteLines * _WhiteLinesGlitchInt))),
			IN.uv_MainTex.y + (LutBarrelRoll * LutDisto))).r;
																														 
			half colorG = tex2D (_MainTex, half2 (IN.uv_MainTex.x + (LutDisto *
			(((LutBlackLines *.5) * _BlackLinesGlitchInt) + ((LutWhiteLines * 2) * _WhiteLinesGlitchInt))),
			IN.uv_MainTex.y + (LutBarrelRoll * LutDisto))).g;

			half colorB = tex2D (_MainTex, half2 (IN.uv_MainTex.x + ((LutDisto * 3) *
			(((LutBlackLines * 2.5) * _BlackLinesGlitchInt) + ((LutWhiteLines * .5) * _WhiteLinesGlitchInt))),
			IN.uv_MainTex.y + (LutBarrelRoll * LutDisto))).b;

			half3 ColorF = saturate (half3 (colorR, colorG, colorB)) + (LutWhiteLines + LutBlackLines);
			o.Albedo = ColorF;

			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}