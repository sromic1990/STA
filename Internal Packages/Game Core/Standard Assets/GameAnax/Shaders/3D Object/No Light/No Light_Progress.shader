// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/No Light/Progressbar
=================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: Gives progressbar effect on screen using mesh UI when Progress Value chaged by scripts. Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Jan 10th 2013
Last Modified		: May 26th 2014

Created By			: Dhruvil Bavisi
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_MainTex	- Base Texure will show as filled value in progressbar
_ActiveTex	- Base Texure will show as un-filled value in progressbar
_CutOff	- Used for checking Mesh UV value in Horizontal mode to display Base or MainLay texture


Change Log
======================================================================================================================== 
v1.1
==== 
1. All float(n) and half(n) Field convreted to fixed(n)
2. Changed Main txture as filled value txture
3. Changed Overlay texture as Base texture and also renamed as "_ActiveTex" which show at non filled value
*/



Shader "GameAnax/No Light/Progressbar" { 
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("MainLay Tex (RGBA)", 2D) = "white" {}
		_ActiveTex ("Base Tex (RGBA)", 2D) = "black" {}
		_CutOff ("Progress",Range(0,1)) = 0.0
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
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform fixed4 _Color;

			uniform sampler2D _MainTex;
			uniform sampler2D _ActiveTex;
			uniform fixed _CutOff;
			
			struct v2f {
				fixed4 pos : POSITION;
				fixed2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = TRANSFORM_UV(0);
				return o;
			}
			fixed4 frag( v2f i ) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex, i.uv);
				fixed4 texBase = tex2D(_ActiveTex, i.uv);
				texBase = lerp(texMain, texBase, texBase.a);
				fixed b = i.uv.x > _CutOff;
				texFinal = lerp(texBase, texMain, b);
				texFinal *= _Color;
				return texFinal;
			}
			ENDCG
		}
	}
}