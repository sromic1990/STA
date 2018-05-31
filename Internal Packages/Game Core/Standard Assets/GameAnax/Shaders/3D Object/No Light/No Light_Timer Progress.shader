/*
GameAnax/No Light/Timer Progress
====================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: Gives Scractch out efffect whenn Alpha Textures Alpha Changel will change runtime using scripts. Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

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
_MainTex		- Base Texure will show when Progess Value is higher then Progess texture's alpha chanel value rest of part will become transparnt
_ProgresssTex	- Progress texure is pre-defined in matrial which need only alpha value to compare with Cut Off
_CutOff			- Cut Off will compared with Progress texure's Alpha chanel, if value is lower in that case it make Main Texture Alpha 0 so it will become transparnt for that area.

Change Log
======================================================================================================================== 
v1.1
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
*/





Shader "GameAnax/No Light/Timer Progress" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ProgresssTex("Timer Effect Tex (A)",2D) = "white"{}
		_CutOff("Progress", Range(0,1.01)) = 0
	}
	SubShader{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
		Blend SrcAlpha OneMinusSrcAlpha
		//
		Lighting Off
		ZWrite On
		Fog { Mode Off }

	 	BindChannels {
            Bind "Color", color
            Bind "Vertex", vertex
            Bind "TexCoord", texcoord
        }
		//
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _ProgresssTex;
			uniform fixed _CutOff; 
			
			fixed4 frag (v2f_img i) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex,i.uv);
				fixed texProgress = tex2D(_ProgresssTex,i.uv).a;
				texFinal = texMain;
				if(texProgress < _CutOff){
					texFinal.a = 0;
				}
				return texFinal;
			}
			ENDCG
		}
	}
}
