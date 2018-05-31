/*
GameAnax/No Light/CMT Power Animation
=========================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: Reuslt on geomatry withou light and shadow informaiton Show Active or Deactive texture after comparing CutOff Value with Animation Texture alpha, Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 255 for RGB changel)
_MainTex	- Paint of Geometry Texture with and used as Deactive Texture when Cut Off Value is less-than Animation Texures Aplha Value
_ActiveTex	- Paint of Geometry Texture with and used as Active Texture when Cut Off Value is Greater-than Animation Texures Aplha Value
_AnimTex	- Animation Texture will not paint on any geometry directy it's used for comparing with user given Cut Off value
_CutOff		- Cut Off Value used to comparision with Animaion texure and accroding to result of comparision it's shows Main or Active texture


Change Log
======================================================================================================================== 
v1.1
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
*/



Shader "GameAnax/No Light/Round Timer" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) A (Alpha)", 2D) = "white" {}
		_ActiveTex ("OverLap Ring Texture (RGB) A (Alpha)", 2D) = "white" {}
		_AnimTex ("A (Alpha)", 2D) = "white" {}
		_CutOff("Animation Pos", Range(0,1.01)) = 0
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
			uniform sampler2D _ActiveTex;
			uniform sampler2D _AnimTex;
			uniform fixed _CutOff;

			fixed4 frag (v2f_img i) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex,i.uv);
				fixed4 texActive = tex2D(_ActiveTex,i.uv);
				fixed4 texAnim = tex2D(_AnimTex,i.uv);

				if(texAnim.a < _CutOff) {
					texFinal = texMain;
				}else{
					texFinal = texActive;
				}

				texFinal *= _Color;
				return texFinal;
			}
			ENDCG
		}
	}
}
