Shader "S.E.C.T.O.R/Gauge"
{
	Properties
	{
		[NoScaleOffset] _ColorTex("ColorTex", 2D) = "" {}
		_CutOffRealGauge ("CutOff real gauge", Range (0,0.9999999)) = 0.5
		_CutOffCurrentGauge ("CutOff current gauge", Range (0,0.9999999)) = 0.5
		_ColorGauge ("Color gauge", Color) = (1,1,1,1)
		_ColorHit ("Color hit or restore", Color) = (1,1,1,1)
	}
	
	SubShader
	{
		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0

			float _CutOffCurrentGauge;
			sampler2D _ColorTex;
			float3 _ColorHit;

			struct appData
			{
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos  : SV_POSITION;
				float2 UV   : ATTR1;
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
				float4 colTex = tex2D(_ColorTex,i.UV).rgba;

				float3 CurrentGauge = _ColorHit;
				clip (colTex.a - (1-_CutOffCurrentGauge));

				o.rgb = CurrentGauge;
				o.a = 1;
				return o;
			}
			ENDCG
		}

		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0

			float _CutOffRealGauge;
			sampler2D _ColorTex;
			float3 _ColorHit, _ColorGauge;

			struct appData
			{
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos  : SV_POSITION;
				float2 UV   : ATTR1;
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
				float4 colTex = tex2D(_ColorTex,i.UV).rgba;

				float3 RealGauge = _ColorGauge;
				clip (colTex.a - (1-_CutOffRealGauge));

				o.rgb = RealGauge;
				o.a = 1;
				return o;
			}
			ENDCG
		}
	} 
}
