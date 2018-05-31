// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/No Light/WTW Wave Form
=============================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: Desinged for What's the Word - Guess Sound, Main Texure will change run time as per sound wave texture generated in scripts Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Mar 6th 2013
Last Modified		: May 26th 2014

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_MainTex	- Main Texture will change as per script genrated Wave Sound Graph from Audio Clip
_BackTex	- Static Background texture (normay black Colored.)


Change Log
======================================================================================================================== 
v1.1
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
*/



shader "GameAnax/No Light/WTW Wave Form" {
	Properties {
		_MainTex ("WaveForm Tex (RGBA)", 2D) = "white" {}
		_BackTex ("Background Tex (RGBA)", 2D) = "black" {}
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _BackTex;
			
			uniform fixed4 _Color;
			
			struct v2f {
				fixed4 pos : POSITION;
				fixed2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = TRANSFORM_UV(0);
				return o;
			}
			
			fixed4 frag( v2f i ) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex, i.uv);
				fixed4 texOver = tex2D(_BackTex, i.uv);
				texFinal = lerp(texOver,texMain, texMain.a);
				return texFinal;
			}
			ENDCG
		}
	}
}