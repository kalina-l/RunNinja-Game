Shader "Ninja/ColorMapSprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_PlayerColor ("Player Color", Color) = (1,1,1,1)
		_PlayerColor2 ("Player Color 2", Color) = (1,1,1,1)
		_PlayerColor3 ("Player Color 3", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed4 _PlayerColor;
			fixed4 _PlayerColor2;
			fixed4 _PlayerColor3;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;

			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
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
}
