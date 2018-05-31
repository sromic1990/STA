// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

/*
GameAnax/Lightmaped/Vertex Fog Linear wo Specular Shader
=========================================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.1
Description			: calculate lightmap value for geomatry and dummy fog using unity_FogColor 

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
1. In Struct v2f now Fog field not represnt to TEXCOORD2 (field "fog" with TEXCOORD2 is supported in iOS)
2. All float(n) and half(n) Field convreted to fixed(n) expect field "fog"
*/



Shader "GameAnax/Lightmaped/Vertex Fog Linear wo Specular" {
    Properties {
    	_Color ("Color (RGB)",Color) = (0.58,0.58,0.58,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogDensity("Fog Density", Range(0,2)) = 1.0
        _FogStart ("Fog Start", Float) = 0.0
        _FogEnd ("Fog End", Float) = 500.0
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
            fixed4 _Color;
            sampler2D _MainTex;
            //
            fixed _FogDensity;
            fixed _FogStart;
            fixed _FogEnd;
            //
            //Get Fog Color Value form Unity Settings
	        uniform fixed4 unity_FogColor;
            
            //Light Map Texture from Unity LightMaping
           	#ifndef LIGHTMAP
			fixed4 unity_LightmapST;
			// sampler2D unity_Lightmap;
			#endif

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
                float fog;
            };
            v2f vert (appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv =  MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				
				//Lightmap Calculation
				#ifndef LIGHTMAP
				o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				 
				//dummy Fog
				fixed4 fogZ = (mul(UNITY_MATRIX_MV, v.vertex)).z;
				fogZ *= (_FogDensity * 0.5);
				o.fog = (fogZ + _FogStart) / (_FogStart - _FogEnd); 
				o.fog= clamp(o.fog,0,1);
				return o;
			}
            fixed4 frag(v2f i) : COLOR {
				fixed4 c = tex2D (_MainTex, i.uv);

				#ifndef LIGHTMAP
				fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
				c.rgb *= lm;
				#endif
				c *= _Color;
				fixed4 rVal =lerp(c, unity_FogColor, i.fog);
				return rVal;
            }
            ENDCG
        }
    } 
}