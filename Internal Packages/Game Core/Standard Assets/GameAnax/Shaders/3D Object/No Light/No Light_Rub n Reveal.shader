/*
GameAnax/No Light/Rub n Reveal
==================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.2
Description			: Gives Scractch out efffect whenn Alpha Textures Alpha Changel will change runtime using scripts. Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Created By			: Ankur Ranpariya, Ankit Jain
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_MainTex	- Base Texure is actul texture which will need to show but it shoud be coverd with fog texture
_FogTex		- Fog Texure is covers Main texture
_Alpha		- Alpha texures Alpha Chanel will decide whcih texure will show in geomatry using lerp fucntion


Change Log
======================================================================================================================== 
v1.2
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
*/



Shader "GameAnax/No Light/Rub n Reveal" { 
	Properties {
		_MainTex ("Main Question Image (RGB)", 2D) = "white" {}
		_FogTex ("Fog Effect Texture (RGBA)", 2D) = "white" {}
		_Alpha ("Cover Alpha (A)", 2D) = "white" {}
	}
	Category{
		SubShader { 
			Tags {"RenderType" = "Geometry" "Queue" = "Geometry"}
			//
			Lighting Off
			ZWrite On
			Fog { Mode Off }
			//
			Pass {
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragmentoption ARB_fog_exp2
				
				#include "UnityCG.cginc"
				uniform sampler2D _FogTex;
				uniform sampler2D _MainTex;
				uniform sampler2D _Alpha;

				fixed4 frag (v2f_img i) : COLOR {
					fixed4 texFinal;
					fixed4 texMain = tex2D(_MainTex,i.uv);
					fixed4 texFog = tex2D(_FogTex, i.uv);
					fixed4 texAlpha = tex2D(_Alpha,i.uv);

					texFinal = lerp(texMain,texFog, texAlpha.a);
					return texFinal;
				}
				ENDCG
			}
		}
	}
}