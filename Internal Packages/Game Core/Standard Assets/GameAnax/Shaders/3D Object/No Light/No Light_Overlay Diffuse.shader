// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/No Light/Overlay Diffuse
=====================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: Calculate Overlay effect like photoshop overlay. Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Jul 20th 2013
Last Modified		: May 26th 2014

Created By			: Ankur Ranpariya, Ankit Jain, Tushar Prajapati
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_MainTex	- Main Texture is Base or bottam Texture and used for caluclate overy effect
_OverlayTex	- Overlay Texture is overlayed texured and it will be overlayed with the Main Texture

Change Log
======================================================================================================================== 
v1.1
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
*/



Shader "GameAnax/No Light/Overlay Diffuse" {
	Properties {
		_MainTex ("Bottam Layer (RGB) A (Alpha)", 2D) = "white" {}
		_OverlayTex ("Overlay Layer (RGB) A (Alpha)", 2D) = "white" {}
	}
	SubShader {
		Tags {"RenderType" = "Geometry" "Queue" = "Geometry"}
		//
		Lighting Off
		ZWrite On
		Fog { Mode Off }
		//
		Pass{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#include "UnityCG.cginc"
				
				struct appdata_t {
					fixed4 vertex : POSITION;
					fixed2 texcoord : TEXCOORD0;
				};

				struct v2f {
					fixed4 vertex : POSITION;
					fixed2 texcoord : TEXCOORD0;
				};

				uniform sampler2D _MainTex;
				uniform sampler2D _OverlayTex;
				
				uniform fixed4 _MainTex_ST;

				v2f vert (appdata_t v) {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR {
					// Get the raw texture value
					fixed4 texBottamLayer = tex2D(_MainTex, i.texcoord);
					fixed4 texOverlay = tex2D(_OverlayTex,i.texcoord);
					
					// Declare the output structure
					fixed4 output;
					if(texBottamLayer.r < 0.5){
						output.r = texOverlay.r * texBottamLayer.r * 2;
					} else {
						output.r = 1 - 2 * (1 - texBottamLayer.r) * (1 - texOverlay.r);
					}
					if(texBottamLayer.g < 0.5){
						output.g = texOverlay.g * texBottamLayer.g * 2;
					} else {
						output.g = 1 - 2 * (1 - texBottamLayer.g) * (1 - texOverlay.g);
					}
					if(texBottamLayer.b < 0.5){
						output.b = texOverlay.b * texBottamLayer.b * 2;
					} else {
						output.b = 1 - 2 * (1 - texBottamLayer.b) * (1 - texOverlay.b);
					}
					if(texBottamLayer.a < 0.5){
						output.a = texOverlay.a * texBottamLayer.a * 2;
					} else {
						output.a = 1 - 2 * (1 - texBottamLayer.a) * (1 - texOverlay.a);
					}
					
					output = lerp(texOverlay,output,texBottamLayer.a);
					output = lerp(texBottamLayer,output,texOverlay.a);
				
					return output;
				}
			ENDCG
		}
	}
}