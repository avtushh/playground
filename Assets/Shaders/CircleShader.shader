Shader "Custom/circle"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_InnerColor("InnerColor", Color) = (1,1,1,1)
		_BorderColor("BorderColor", Color) = (1,1,1,1)
		_BorderWidth("BorderWidth", Float) = 0.1

	}

		CGINCLUDE
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
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	sampler2D _MainTex;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color;
		return OUT;
	}

	half _BorderWidth;
	
	half4 _InnerColor;
	half4 _BorderColor;

	fixed4 frag(v2f IN) : COLOR
	{
		
		half2 local_pos = IN.texcoord - fixed2(0.5,0.5);
		half dist = length(local_pos);

		float4 col;
		if (dist < _BorderWidth) {
			return _InnerColor;
		}
		else {

			fixed4 c = 0;
			c.w = 1;
			

			if (IN.texcoord.x > 0.5) {
				if (IN.texcoord.y > 0.5) {
					c.b = 0;
					c.g = 0.8;
					c.r = 0;
				}
				else {
					c.b = 0.8;
					c.g = 0;
					c.r = 0;
				}
				
			}
			else {
				if (IN.texcoord.y > 0.5) {
					c.b = 0;
					c.g = 0;
					c.r = 0.8;
				}
				else {
					c.b = 0.8;
					c.g = 0;
					c.r = 0.8;
				}
			}

			//c.rgb *= IN.texcoord.x;
			return c;
		}
	
		return _BorderColor;
	}
		ENDCG

		SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

			Cull Off
			Lighting Off
			ZWrite Off
			Fog{ Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}