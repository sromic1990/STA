// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
GameAnax/Vertex Fog Lienar wo Specular Shader
=============================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: calculate dummy fog using unity_FogColor 

Created Date		: Mar 29th 2014
Last Modified		: May 26th 2014

Created By			: Ankur Ranpariya, Tushar Prajapati
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: MADFINGER, melong(http://forum.unity3d.com/members/61543-melong), Software 7 Blog

Used Total SubShader: 1 (One)
Used Total Pass		: 1 (One)

Propperties
======================================================================================================================== 
_MainTex	- Texture to paint on geomatry
_FogDensity	- Fog Desnsity applied as multiplier wiht distnce between cam and object's vertex posiotn
_FogStart	- Fog start 
_FogEnd		- Fog End

Change Log
======================================================================================================================== 
v1.1
==== 
1. In Struct v2f now Fog field not represnt to TEXCOORD2 (field fog with TEXCOORD2 is supported in iOS)
2. All float(n) and half(n) Field convreted to fixed(n)
*/



Shader "GameAnax/Vertex Fog Lienar wo Specular" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogDensity("Fog Density",Range(0,2)) = 1
        _FogStart ("Fog Start", Float) = 0.0
        _FogEnd ("Fog End", Float) = 10.0
    }
    SubShader {
        Tags {  "RenderType" = "Geometry" "Queue" = "Geometry" "Type"="Vertex"}
        //
        Fog { Mode Off }
        //
        Pass {
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members fog)
			#pragma exclude_renderers d3d11 xbox360
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            //
            sampler2D _MainTex;
            fixed _FogDensity;
            fixed _FogStart;
            fixed _FogEnd;
            
            //Get Fog Color Value form Unity Settings
    		uniform half4 unity_FogColor;
			fixed4 _MainTex_ST;

            struct appdata {
               fixed4 vertex : POSITION;
               fixed2 texcoord : TEXCOORD0;
            };
            struct v2f {
                fixed4 pos : SV_POSITION;
                fixed2 uv  : TEXCOORD0;
				#ifndef LIGHTMAP
				fixed2 lmap : TEXCOORD1;
				#endif
                fixed fog;
            };
            v2f vert (appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				
				//dummy Fog
				fixed fogz = mul(UNITY_MATRIX_MV, v.vertex).z;
				fogz *= (_FogDensity * 0.5);
				o.fog = saturate((fogz + _FogStart) / (_FogStart - _FogEnd));
				return o;
			}
            half4 frag(v2f i) : COLOR {
				fixed4 c = tex2D (_MainTex, i.uv);
				fixed4 rVal =lerp(c, unity_FogColor, i.fog);
				return rVal;
            }
            ENDCG
        }
    } 
   }