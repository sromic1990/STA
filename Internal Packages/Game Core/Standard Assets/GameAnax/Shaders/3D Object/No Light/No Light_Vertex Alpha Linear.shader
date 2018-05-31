// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/Vertex Alpha Linear wo Specular Shader
===============================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: creadted shader from LightMapded Vertex alpha without calulate Lightmap effect

Created Date		: May 31th 2014
Last Modified		: May 31th 2014

Created By			: Ankur Ranpariya, Tushar Prajapati
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: MADFINGER, melong(http://forum.unity3d.com/members/61543-melong), Software 7 Blog

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_MainTex		- Texture to paint on geomatry
_FogStart		- Fog start 
_FogFadeLength	- Fade till lenght after that it's total transparant like simple air

Change Log
======================================================================================================================== 
v1.0
==== 
*/


Shader "GameAnax/Vertex Alpha Linear wo Specular" {
    Properties {
    	_Color ("Color (RGB)",Color) = (0.58,0.58,0.58,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogStart ("Fog Start", Float) = 0.0
        _FogFadeLength ("Fog Fade Length", Float) = 500.0
    }
    SubShader {
        Tags {  "RenderType" = "Transparent" "Queue" = "Transparent-100" "Type"="Vertex"}
        //
        Fog { Mode Off }
    	Blend SrcAlpha OneMinusSrcAlpha
        //
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_fog_exp2
            #include "UnityCG.cginc"
            //
            fixed4 _Color;
            sampler2D _MainTex;

            //Get Fog Color Value form Unity Settings
            //
            fixed _FogStart;
            fixed _FogFadeLength;
            //
			fixed4 _MainTex_ST;

            struct appdata {
               fixed4 vertex : POSITION;
               fixed2 texcoord : TEXCOORD0;
            };
            struct v2f {
                fixed4 pos : SV_POSITION;
                fixed2 uv  : TEXCOORD0;
				fixed4 color : COLOR;
            }; 
            v2f vert (appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				
				//dummy Fog
				fixed fog;
				fixed4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
				fog =  -viewPos.z - _ProjectionParams.y;
				fog = (fog - _FogStart) / _FogFadeLength;
				fog = clamp(fog,0,1);
				o.color.a = 1 - fog;
				return o;
			}
            fixed4 frag(v2f i) : COLOR {
				fixed4 c = tex2D (_MainTex, i.uv);
				c *= _Color;
				c.a *= i.color.a;
				return c;
            }
            ENDCG
        }
    } 
}