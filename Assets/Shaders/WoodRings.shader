Shader "Unlit/WoodRings"
{
	Properties
	{
		_Ambient("Ambient", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				//float2 uv : TEXCOORD0; // not needed
			};

			struct v2f
			{
				//float2 uv : TEXCOORD0; // not needed
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				float4 col : TEXCOORD1;
				float4 col1 : TEXCOORD2;
				};

			float _Ambient;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.col = v.vertex + 0.1;
				o.col1 = mul(UNITY_MATRIX_MV, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// TODO:
				//  Depending on the object space position, add a wood pattern

				float deg2rad = 0.0174533;
				float distance = sqrt(pow(i.col.x*3, 2) + pow(i.col.z/1.5, 2));
				float sinDistance = fmod(sin(distance * 50), 1);
				
				float camDistance = abs(i.col1.z);
	
				float4 col = lerp(float4(0.51, 0.38, 0.18, 1), float4(0.30, 0.23, 0.11, 1), sinDistance);

				//  Depending on the camera space position, fade in a fog color
				if (camDistance > 20)
				{
					 col = lerp(col, float4(0.2, 0.2, 0.2, 1), clamp((camDistance - 20)/10, 0, 0.7));
				}

				col *= (_Ambient + (1 - _Ambient) * saturate(i.normal.y)); // VERY basic lighting. TODO LATER: improve

				return col;
			}
			ENDCG
		}
	}
}
