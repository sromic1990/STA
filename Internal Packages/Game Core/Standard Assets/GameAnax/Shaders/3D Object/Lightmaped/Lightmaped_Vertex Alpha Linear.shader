/*
GameAnax/Lightmaped/Vertex Alpha Linear wo Specular Shader
==========================================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.2
Description			: creadted shader using rafrance of Vertex Fog Liner

Created Date		: Mar 29th 2014
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
v1.1
==== 
1. In Struct v2f now Fog field not represnt to TEXCOORD2 (field "fog" with TEXCOORD2 is supported in iOS)
2. All float(n) and half(n) Field convreted to fixed(n) expect field "fog"

v1.2
==== 
1. Removes unity_FogColor it's no more  requires for assign alpha to vertex color
2. Adds color simentics on v2f to gave fade value to vertex value.
3. Removes _FogDensity properity
4. convrets it to AlphBlended Shader to fade geomatry on far distance.
*/


Shader "GameAnax/Lightmaped/Vertex Alpha Linear wo Specular" {
    Properties {
    	_Color ("Color (RGB)",Color) = (0.58,0.58,0.58,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogStart ("Fog Start", Float) = 0.0
        _FogFadeLength("Fog Fade Length", Float) = 500.0
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
				fixed alpha;
				#ifndef LIGHTMAP
				fixed2 lmap : TEXCOORD1;
				#endif

            }; 
            v2f vert (appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				
				//Lightmap Calculation
				#ifndef LIGHTMAP
				o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				 
				//dummy Fog
				fixed fog;
				fixed4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
				fog =  -viewPos.z - _ProjectionParams.y;
				fog = (fog - _FogStart) / _FogFadeLength;
				fog = clamp(fog,0,1);
				o.alpha = 1 - fog;
				return o;
			}
            fixed4 frag(v2f i) : COLOR {
				fixed4 c = tex2D (_MainTex, i.uv);
				c *= _Color;
				c.a = i.alpha;
				#ifndef LIGHTMAP
				fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
				c.rgb *= lm;
				#endif
				return c;
            }
            ENDCG
        }
    } 
}