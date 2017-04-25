Shader "S.E.C.T.O.R/Distortion" 
{
	Properties 
	{
		_BumpAmt  ("Distortion", Float) = 10
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Speed ("Speed & direction panning", Vector) = (0,0,0,0)
	}

	Category 
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
	
		SubShader 
		{
			GrabPass 
			{
				Name "BASE"
				Tags { "LightMode" = "Always" }
			}

			Pass 
			{
				Name "BASE"
				Tags { "LightMode" = "Always" "RenderType"="Transparent" "Queue" = "Transparent"}
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 5.0
				#include "UnityCG.cginc"

				sampler2D _GrabTexture, _BumpMap;
				half4 _GrabTexture_TexelSize, _BumpMap_ST;
				half2 _Speed;
				half _BumpAmt;

				struct appdata_t //v;
				{
					half4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};
				
				struct v2f //o;
				{
					half4 vertex : SV_POSITION;
					half4 uvgrab : TEXCOORD0;
					float2 uv : TEXCOORD1;
				};

				v2f vert (appdata_t v)
				{
					v2f o;
                    o.uv = v.texcoord;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					#if UNITY_UV_STARTS_AT_TOP
					half scale = -1.0;
					#else
					half scale = 1.0;
					#endif
					o.uvgrab.xy = (half2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					return o;
				}
				
				half4 frag (v2f i) : SV_Target
				{
					half2 Speed = _Speed * _Time;
                    
					half2 bump = UnpackNormal(tex2D( _BumpMap, i.uv + Speed)).rg;
					float2 offset = bump * (_BumpAmt *10) * _GrabTexture_TexelSize.xy;
					i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
					
					half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
					return col;
				}
				ENDCG
			}
		}
		
			// ------------------------------------------------------------------
			// Fallback for older cards and Unity non-Pro
		
		SubShader 
		{
			Blend DstColor Zero
			Pass 
			{
				Name "BASE"
				SetTexture [_MainTex] {	combine texture }
			}
		}
	}
}