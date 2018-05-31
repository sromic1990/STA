/*
GameAnax/Basic/Alpha Blended
============================
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
_Shininess	- apply to matrial tab for apply shininess to geometry

Change Log
======================================================================================================================== 
v1.1
==== 
*/



Shader "GameAnax/Basic/Alpha Blended" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Base Texture", 2D) = "white" {}
		_Shininess ("Shininess", Range (0.01, 1)) = 0.7
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		//
		Lighting On
		SeparateSpecular On
		//
		Material {
			Diffuse [_Color]
			Ambient [_Color]
			Emission [_Color]
			Shininess [_Shininess]
			Specular [_Color]
		}
		//
		Pass{
			SetTexture [_MainTex] {
				constantcolor [_Color]
				combine texture * constant }
		}
	}
	FallBack "Diffuse"
}

