// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/No Light/Mask/Alpha Blended Obj Masker
================================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: 

Created Date		: May 30th 2016
Last Modified		: May 30th 2016

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 255 for RGB changel)

Change Log
======================================================================================================================== 
v1.0
==== 
*/

Shader "GameAnax/No Light/Mask/Alpha Blended Obj Masker" {
	Properties {
				_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) A (MASK)", 2D) = "white" {}

	}
	SubShader {
			Tags { 
			"Queue"="Background+50" 
			"RenderType"="Transparent"  
			"IgnoreProjector"="True"
		}

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
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma fragmentoption ARB_fog_exp2
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform fixed4 _Color; 
			float4 _MainTex_ST;

			struct vInput {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
			};

			struct v2f {
				fixed4 pos : SV_POSITION;
				fixed2 uv  : TEXCOORD0;
			};

			v2f vert(vInput v){
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}


			fixed4 frag (v2f_img i) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex,i.uv);
				texFinal = texMain * _Color;
				return texFinal;
			}
			ENDCG
		}
	}
}
