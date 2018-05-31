// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/No Light/Unlit/GreyScale Alpha Blended
===============================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: Reuslt texure in Greyscale mode without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Oct 28th 2014
Last Modified		: Oct 28th 2014

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: Tim Michels (http://answers.unity3d.com/questions/343243/unlit-greyscale-shader.html)

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 255 for RGB changel)
_MainTex	- Paint of Geometry after multiplying with color
_Level		- desides at what level it should be greyscaled


Change Log
======================================================================================================================== 
v1.0
==== 
*/




Shader "GameAnax/No Light/Unlit/GreyScale Alpha Blended" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Level ("GreyScale Level",Range(0,1)) = 1
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True"}
		Blend SrcAlpha OneMinusSrcAlpha
		//
		Lighting Off
		ZWrite On
		Fog { Mode Off }
		//

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma fragmentoption ARB_fog_exp2

			#include "UnityCG.cginc"

			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform fixed _Level;
			fixed4 _MainTex_ST;

			struct v2f {
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR {
				fixed4 texcol = tex2D (_MainTex, i.uv);
				fixed4 texFinal;
				texcol *= _Color;
				texFinal.a = texcol.a;
				//texFinal.rgb = dot(texcol.rgb,fixed3(0.3, 0.59, 0.11));
				texFinal.rgb = dot(texcol.rgb,fixed3(.222, .707, .071));

				return lerp(texcol,texFinal,_Level);
				//return texFinal;
			}
			ENDCG
		}
	}
} 
