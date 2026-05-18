Shader "Unlit/Waves"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HeightMap("HeightMap", 2D) = "black" {}

		_WaveSpeed("Wave Speed", float) = 0
		_WaveHeight("Wave Height", float) = 0
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			sampler2D _MainTex;
			float _WaveSpeed;
			float _WaveHeight;
			sampler2D _HeightMap;
			float readHeight;
			v2f vert(appdata v)
			{
				v2f o;
				// TODO:
				//  -move the vertex up and down in a wave pattern
				//float4 modVertex = v.vertex;
				
				float readHeight = tex2Dlod(_HeightMap, float4(v.uv.x + _Time.w*0.1*_WaveSpeed, v.uv.y,0,0)).r;

				v.vertex.y += readHeight * _WaveHeight;

				float4 modVertex = v.vertex;

				//modVertex.w = 1;
				o.vertex = UnityObjectToClipPos(modVertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}
