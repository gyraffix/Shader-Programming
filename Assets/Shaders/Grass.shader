Shader "Unlit/Grass"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// Make transparency work (1):
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
		LOD 100

		Pass
		{
			// Make transparency work (2):
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				// TODO: Make the grass move in the wind (wave pattern), depending on
				//   object and world space position

				float4 worldSpace = mul(UNITY_MATRIX_M, v.vertex);

				v.vertex += float4(0.2,0,0.2,0) * (sin(_Time.y*5 + worldSpace.x + worldSpace.z)+1)/2 * (v.vertex.y + 0.5);
				
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
		ENDCG
		}
	}
}
