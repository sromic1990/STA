/* 
Unify Official/Modified/GUI/Text Unity3.5x
========================================
Technolgies Used	: Unity ShaderLab 
Version				: 1.0
Description			: 

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Crated by			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total Pass		: 1 (One)
Used Total SubShader: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for change Color of Text
_MainTex	- Main Texture, White color texture with Alpha Value

Change Log
======================================================================================================================== 
*/



Shader "Unify Official/Modified/GUI/Text Unity 3.5x" {
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_Color ("Text Color", Color) = (1,1,1,1)
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
			Color [_Color] 
			SetTexture [_MainTex] {
				combine primary, texture * primary
			}
		}
	}
 }