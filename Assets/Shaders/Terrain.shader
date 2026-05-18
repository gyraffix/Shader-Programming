Shader "Unlit/Terrain"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HeightMap("HeightMap", 2D) = "black" {}
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

			sampler2D _MainTex;
			sampler2D _HeightMap;
			float4 _MainTex_ST;
			float readHeight;
			float4 _HeightMap_TexelSize;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
				float4 vertex : SV_POSITION;
			};
			
			
			v2f vert(appdata v)
			{
				v2f o;
				
				
				readHeight = tex2Dlod(_HeightMap, float4(v.uv.x, v.uv.y,0,0)).r;

				v.vertex.y += readHeight;

				float4 modVertex = v.vertex;

				o.vertex = UnityObjectToClipPos(modVertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float2 texel = _HeightMap_TexelSize.xy;

				// First Get positions of three vertices: The one where I calculate the normal, one further towards the X, and one further towards the Y.
				float3 pC = float3 (v.uv.x, readHeight, v.uv.y);

				float readHeightX = tex2Dlod(_HeightMap, float4(v.uv.x - texel.x, v.uv.y,0,0)).r;
				float3 pX = float3(v.uv.x - texel.x, readHeightX, v.uv.y);

				float readHeightY = tex2Dlod(_HeightMap, float4(v.uv.x, v.uv.y - texel.y,0,0)).r;
				float3 pY = float3(v.uv.x, readHeightY, v.uv.y  - texel.y);

				// Then Get 2 vectors, one from C to X, and one from C to Y
				float3 vectorCX = pX - pC;
				float3 vectorCY = pY - pC;

				// Then do Cross Product to calculate the normal
				float3 normal = normalize(cross(vectorCX, vectorCY));
				
			    o.normal = normalize(float4(normal, 0));

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 baseCol = tex2D(_MainTex, i.uv);

				// After calculating the normal it's time to calculate the dot product between the normal and the light direction
				float3 lightDir = normalize(float3(_WorldSpaceLightPos0.x,-1 * _WorldSpaceLightPos0.y, _WorldSpaceLightPos0.z));

				float lightingScalar = saturate(dot(lightDir, i.normal)) + 0.5;

				float4 col = baseCol *  lightingScalar;

				return col;
			}
			ENDCG
		}
	}
}
