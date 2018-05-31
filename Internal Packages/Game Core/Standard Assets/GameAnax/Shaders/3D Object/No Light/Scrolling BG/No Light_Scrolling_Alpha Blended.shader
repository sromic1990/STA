// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/No Light/Scrolling/Alpha Blended
==========================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: Reuslt texure without any light or shadow informaion Calculation done in Frag program so it may be havy on device and much more havy on High resolution devicses

Created Date		: Nov 17th 2014
Last Modified		: Nov 17th 2014

Created By			: Ankur Ranpariya
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: 

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_Color 		- Color usied for Maltiply with texutre output (if you need to paint texture as it set it 255 for RGB changel)
_MainTex	- Paint of Geometry after multiplying with color
_TSInfo		- 


Change Log
======================================================================================================================== 
v1.0
==== 
*/




Shader "GameAnax/No Light/Scrolling/Alpha Blended" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) A (Alpha)", 2D) = "white" {}
		_TSInfo("(X,Y)=Speed (ZW)=Tiling)",vector)=(0,0,1,1)
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True"}
		Blend SrcAlpha OneMinusSrcAlpha
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
			#pragma fragmentoption ARB_fog_exp2
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform fixed4 _Color; 
			uniform fixed4 _TSInfo;
			fixed4 _MainTex_ST;


			struct vInput {
    			fixed4 vertex : POSITION;
    			fixed2 texcoord : TEXCOORD0;
    			fixed4 color : COLOR;
			};
			struct v2f {
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert( vInput v )
			{
				v2f o;
				fixed2 tiling = fixed2(_TSInfo.z,_TSInfo.w);
				fixed2 speed = fixed2(_TSInfo.x, _TSInfo.y) * _Time;

				o.pos = UnityObjectToClipPos (v.vertex);

				o.uv = v.texcoord.xy;
				o.uv *= tiling;
				o.uv.xy += frac(speed); 
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : COLOR {
				fixed4 texFinal;
				fixed4 texMain = tex2D(_MainTex,i.uv);
				texFinal = texMain * _Color * i.color;
				return texFinal;
			}
			ENDCG
		}
	}
}
