Shader "S.E.C.T.O.R/Equaliser"
{
    Properties
    {
        [Header(Equaliser)]
        [NoScaleOffset] _ColorTex("ColorTex", 2D) = "white" {}
        [HDR]_Color1 ("Color 1", Color) = (1,1,1,1)
		[HDR]_Color2 ("Color 2", Color) = (1,1,1,1)
		[Space(15)]
		
        [Header(LUT)]
        _LUT ("Look up table", 2D) = "white" {}
        _DistoInt ("Distortion intensity", Range (0,2)) = .75
        _LittleLinesInt ("Little distortion intensity", Range (0,2)) = .75
        [Space(20)]

        _SpeedLittleLines ("Speed little disto", Range (0,5)) = 1
        _SpeedDisto ("Speed disto", Range (0,5)) = 1
    }
    
    SubShader
    {
        Pass
        {
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 5.0

            sampler2D _ColorTex, _LUT;
            float4 _LUT_ST;
            float3 _Color1, _Color2;
            float _DistoInt, _LittleLinesInt, _SpeedDisto, _SpeedLittleLines;

            struct appData	//v.
            {
                float4 pos 		: POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f	//o.
            {
                float4 pos  : 	SV_POSITION;
                float2 UV   : 	ATTR1;
            };

            v2f vert(appData v)
            {
                v2f o;
                o.UV = v.texcoord;
                o.pos = mul (UNITY_MATRIX_MVP, v.pos);
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                float4 o;

                float Time = _Time *2;
                float4 LutST = _LUT_ST * .01;

                //Search texture(s)---------------------------------------------------------------------------
                float LutR = tex2D (_LUT, float2 (fmod (Time, 4), .5)).r;
                float LutG = tex2D (_LUT, float2 (i.UV.x * LutST.x, fmod (i.UV.y + (Time * _SpeedLittleLines), 4))).g;
                float LutB = tex2D (_LUT, float2 (i.UV.x * LutST.y, fmod (i.UV.y + Time, 4))).b;
                float LutA = tex2D (_LUT, float2 (fmod (Time * _SpeedDisto, 4), .5)).a;

                //Calcul--------------------------------------------------------------------------------------
                float BarrelRoll = (LutR * 2-1);
                float BLines = LutG * _LittleLinesInt;
                float WLines = LutB *2-1;
                float Disto = (LutA *2-1) * _DistoInt;

				float3 Gradiant = _Color1 * i.UV.y;
				Gradiant += _Color2 * (1-i.UV.y);
				
                float3 colTex = tex2D(_ColorTex, float2 (i.UV.x, i.UV.y + Disto * (BLines + WLines)));

                //Output--------------------------------------------------------------------------------------
                o.rgb = colTex * Gradiant;
                o.a = 1;
                return o;
            }
            ENDCG
        }
    }
}