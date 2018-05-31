/*
GameAnax/No Light/User Timer Bar
=========================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: Reuslt on geomatry withou light and shadow informaiton Show Active or Deactive texture after comparing CutOff Value with Animation Texture alpha, Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Oct 3rd 2015
Last Modified		: Oct 3rd 2015

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



Shader "GameAnax/No Light/User Timer Bar" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

		_MainTex ("Base (RGB) A (Alpha)", 2D) = "white" {}

		_VColor ("Value Color", Color) = (1,1,1,1)
		_NVColor ("Non Value Color", Color) = (1,1,1,1)

		_AnimTex ("A (Alpha)", 2D) = "white" {}

		_CutOff("Animation Pos",Range(0,1.01)) = 0
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

			uniform fixed4 _Color;
			uniform sampler2D _MainTex;

			uniform fixed4 _VColor;
			uniform fixed4 _NVColor;
			
			uniform sampler2D _AnimTex;
			uniform fixed _CutOff;

			fixed4 frag (v2f_img i) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex,i.uv);
				fixed4 texAnim = tex2D(_AnimTex,i.uv); 

				if(texAnim.a < _CutOff) {
					texFinal = texMain * _NVColor;
				}else{
					texFinal = texMain * _VColor;
				}

				texFinal *= _Color;
				return texFinal;
			}
			ENDCG
		}
	}
}
