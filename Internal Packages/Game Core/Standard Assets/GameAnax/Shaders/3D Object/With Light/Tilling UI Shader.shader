/*
GameAnax/Basic/Addtive Shader
=============================
Technolgies Used	: Unity Surface Shader
Version				: 1.0
Description			: 

Created Date		: N/A
Last Modified		: Jun 16th 2016

Crated by			: Tushar Prajapati
Last Modified by	: Tushar Prajapati
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Density	- 
_MainTex	- Main Texture to paint of Geometry Texture with Alpha Value, Alpha Value will Blened using Blend option

Change Log
======================================================================================================================== 
v1.0
====
1. 
*/
Shader "GameAnax/Basic/Tilling UI Shader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Density("Density",Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Lighting Off

		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float4 screenPos;
			float4 _ScreenParams;
		};

		sampler2D _MainTex;
		float _Density;
		half3 _BlendDetail;

		void surf (Input IN, inout SurfaceOutput o) {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(_ScreenParams.x,_ScreenParams.y) * _Density / 100;
			o.Albedo = tex2D(_MainTex, screenUV).rgb;
		}
		ENDCG
	} 
}  