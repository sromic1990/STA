/*
GameAnax/Basic/Alpha Test
==========================
Technolgies Used	: Unity ShaderLab 
Version				: 1.0
Description			: Using single Pass it's clauclate diffuse Color with Alpha Blending, this shader may calculate lights and shadow on based of render setting

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Crated by			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 158 for RGB changel)
_MainTex	- Main Texture to paint of Geometry Texture with Alpha Value, Alpha Value will Blened using Blend option
_CutOff		- Cut Off Value applied to "AlphaTest" keyword with Greter Condition cut mess where alpha value higer the cut off value

Change Log
======================================================================================================================== 
v1.1
==== 
*/



Shader "GameAnax/Basic/Alpha Test" {
	Properties {
		_Color("Base Color",color) = (0,0,0,0)
		_MainTex ("Base Texture", 2D) = "white" {}
		_CutOff("Cutoff Value",Range(0.00,1)) = 0.04
	}

	SubShader {
		Tags {"RenderType" = "Geometry" "Queue" = "AlphaTest"}
		Alphatest Greater [_CutOff]
		//
		Pass{
			SetTexture [_MainTex] {
				ConstantColor [_Color]
				combine texture  * constant }
		}
	}
}
