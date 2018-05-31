/*
GameAnax/No Light/WTW Letters
===============================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: Santhana Bharthy, Santharao Simhadri, Prvin Mane
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with Main texutre output (if you need to paint texture as it set it 255 for RGB changel)
_MainTex	- Alphabet texture which will applied run time as per Require of Geometry after multiplying with color
_BGColor	- Color usied for Maltiply with Background texutre output (if you need to paint texture as it set it 255 for RGB changel)
_BGTex		- Alphabet Backgound texture which show where Alphabet texture have alpha values


Change Log
======================================================================================================================== 
v1.1
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
*/



Shader "GameAnax/No Light/WTW Letters" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Letters[MainTex] (RGBA)", 2D) = "white" {}
		_BGColor ("Back Ground Color", Color) = (1,1,1,1)
		_BGTex ("BG Texture (RGBA)", 2D) = "white" {}
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		//
		Lighting Off 
		ZWrite On
		Fog { Mode Off }
		//
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma fragmentoption ARB_fog_exp2
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _BGTex;
			uniform fixed4 _Color; 
			uniform fixed4 _BGColor; 

			fixed4 frag (v2f_img i) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex,i.uv);
				texMain *=  _Color;

				fixed4 texSecond = tex2D(_BGTex,i.uv);
				texSecond *= _BGColor;

				texFinal = lerp(texSecond, texMain, texMain.a);
				return texFinal;
			}
			ENDCG
		}
	}
}