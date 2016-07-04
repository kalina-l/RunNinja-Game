
Shader "Ninja/ColorMapSpriteUI" 
{
	Properties { 
		_MainTex ("Texture", any) = "" {} 
		_Color ("Tint", Color) = (1,1,1,1)
		_PlayerColor ("Player Color", Color) = (1,1,1,1)
		_PlayerColor2 ("Player Color 2", Color) = (1,1,1,1)
		_PlayerColor3 ("Player Color 3", Color) = (1,1,1,1)
	} 

	SubShader {

		Tags { "ForceSupported" = "True" "RenderType"="Overlay" } 
		
		Lighting Off 
		Blend SrcAlpha OneMinusSrcAlpha 
		Cull Off 
		ZWrite Off 
		ZTest Always 
		
		Pass {	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _PlayerColor;
			fixed4 _PlayerColor2;
			fixed4 _PlayerColor3;

			uniform float4 _MainTex_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, i.texcoord) * i.color;
				c.rgb *= c.a;

				float rValue = step(0.1f, c.r - ((c.g + c.b)*0.5f));
				float gValue = step(0.1f, c.g - ((c.r + c.b)*0.5f));
				float bValue = step(0.1f, c.b - ((c.g + c.r)*0.5f));


				fixed4 rCol = fixed4(c.r, c.r, c.r, c.a) * _PlayerColor;
				fixed4 gCol = fixed4(c.g, c.g, c.g, c.a) * _PlayerColor2;
				fixed4 bCol = fixed4(c.b, c.b, c.b, c.a) * _PlayerColor3;


				return lerp(lerp(lerp(c * _Color, rCol, rValue), gCol, gValue), bCol, bValue);
			}
			ENDCG 
		}
	} 
	
	Fallback off 
}
