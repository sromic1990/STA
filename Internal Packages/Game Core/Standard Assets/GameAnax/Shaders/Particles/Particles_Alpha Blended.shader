/*
GameAnax/Particles/Alpha Blended Shader
=======================================
Technolgies Used	: Unity ShaderLab 
Version				: 1.0
Description			: Using single Pass it's clauclate diffuse Color with Alpha Blending, this shader may calculate lights and shadow on based of render setting.

Created Date		: Jun 9th 2014
Last Modified		: Jun 9th 2014

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

Change Log
======================================================================================================================== 
v1.0
====
*/



Shader "GameAnax/Particles/Alpha Blended" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Base Texture", 2D) = "white" {}
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Blend SrcAlpha OneMinusSrcAlpha
		//
		ZWrite Off
		Lighting Off
		Cull Off
		//
		Pass{
			SetTexture [_MainTex] {
				constantcolor [_Color]
				combine texture * constant
			}
			SetTexture[_] {
				combine previous * primary
			}	
		}
	}
}

