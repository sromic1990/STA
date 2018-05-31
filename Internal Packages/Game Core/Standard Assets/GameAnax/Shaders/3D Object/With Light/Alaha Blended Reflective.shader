/*
GameAnax/Basic/Alaha Blended Reflective
=======================================
Technolgies Used	: Unity ShaderLab 
Version				: 1.3
Description			: Using single Pass it's clauclate diffuse Color with Alpha Blending, this shader may calculate lights and shadow on based of render setting and calculate reflacteion in second pass on based of _Cube texture 

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Crated by			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 2 (Two)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 158 for RGB changel)
_MainTex	- Main Texture to paint of Geometry Texture with Alpha Value, Alpha Value will Blened using Blend option
_Cube		- Cube texture are used at refleaction after converts as SphereMap
_Shininess	- apply to matrial tab for apply shininess to geometry

Change Log
======================================================================================================================== 
v1.1
==== 
*/



Shader "GameAnax/Basic/Alaha Blended Reflective" { 
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Base Texture", 2D) = "white" {}
		_Cube  ("Reflation Map (RGB)",2D) = "_Skybox" {TexGen SphereMap}
		_Shininess ("Shininess", Range (0.01, 1)) = 0.7
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent"  "LightMode" = "Vertex" }
		//
		Lighting On
		SeparateSpecular On
		//
		Material {
			Diffuse [_Color]
			Ambient [_Color]
			Emission [_Color]
			Specular [_Color]
			Shininess [_Shininess]
		}
		//
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			SetTexture [_MainTex] {
				Combine texture, texture  }
		}
		//
		Pass {
			Blend DstAlpha One
			SetTexture [_] {
				Combine One - texture, One - texture }
			SetTexture [_Cube] {
				combine texture, previous }
		}
	}
	FallBack "Reflective/Diffuse"
}