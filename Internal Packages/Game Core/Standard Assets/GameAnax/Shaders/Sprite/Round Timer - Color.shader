/*
GameAnax/No Light/User Round Timer
==================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: 

Created Date		: Dec 5th 2017
Last Modified		: Dec 5th 2017

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 255 for RGB changel)

_MainTex	- Paint of Geometry Texture with for Player image
_UserOffTex	- User Mask to create Round / shape base image

_ValueTex	- Progress texure when progress value is greater
_NonValTex	- Progress texure when progress value is lesser
_AnimTex	- progress runing style like round, oblique or any other
_CutOff		- Prgoress value scaller, progress value scaller got data from script at run time.


Change Log
======================================================================================================================== 
v1.0
==== 
1. 
*/



Shader "GameAnax/Sprites/Round Timer - Color" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_Color ("Tint", Color) = (1,1,1,1)
		
		_AnimTex ("A (Alpha)", 2D) = "white" {}
		
		_VColor ("Value Color", Color) = (1,1,1,1)
		_NVColor ("Non Value Color", Color) = (1,1,1,1)


		_CutOff("Animation Pos",Range(0,1.01)) = 0
	}
	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True"

			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};

			uniform fixed4 _Color;
			
			uniform sampler2D _MainTex;
			sampler2D _AlphaTex;
			
			uniform sampler2D _AnimTex;
			
			uniform fixed4 _VColor; 
			uniform fixed4 _NVColor;
			
			uniform fixed _CutOff;

			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}
			fixed4 SampleSpriteTexture (float2 uv) {
				fixed4 color = tex2D (_MainTex, uv); 
				#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
				#endif //ETC1_EXTERNAL_ALPHA
				return color;
			}

			fixed4 frag(v2f IN) : SV_Target {
				fixed4 a;

				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				
				fixed4 texAnim = tex2D(_AnimTex,IN.texcoord); 
				if(texAnim.a < _CutOff) {
					a = _VColor;
				}else{
					a = _NVColor;
				}				
				c.rgb *= a.rgb;
				return c;
			}
			ENDCG
		}
	}
}
