// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

/*
GameAnax/Basic/Vertex Fog Lienar
=================================
Technolgies Used	: CGPrograming with Vertex and fragment 
Version				: 1.0
Description			: calculate dummy fog using dummy fog color

Created Date		: Aug 2nd 2016
Last Modified		: Aug 2nd 2016

Created By			: https://jakobknudsen.wordpress.com/2013/08/06/altitude-fog/
Last Modified by	: Ankur Ranpariya
Also Contributed By	: 
Curtosey By			: https://jakobknudsen.wordpress.com/2013/08/06/altitude-fog/

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
v1.0
==== 
*/



Shader "GameAnax/Basic/Vertex Fog Lienar" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)

        _MainTex ("Base (RGB)", 2D) = "white" {}

        _FogColor ("Fog Color", Color) = (1,1,1,0)

        _FogDensity("Fog Density",Range(0,2)) = 1
        _FogStart ("Fog Start", Float) = 0.0
        _FogEnd ("Fog End", Float) = 10.0
    }

    SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        //

		LOD 200
        Fog { Mode Off }
        //
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert alpha:fade finalcolor:finalcolor
		#pragma debug
		//

		sampler2D _MainTex;
		fixed4 _FogColor;
		fixed4 _Color;

		float _FogEnd;
		float _FogStart;

		struct Input {
			float2 uv_MainTex;
			float4 pos;
			//float4 color : COLOR;
		};
		 
		void vert (inout appdata_full v, out Input o) {
			float4 hpos = UnityObjectToClipPos (v.vertex);
			o.pos = mul(unity_ObjectToWorld, v.vertex);
			o.uv_MainTex = v.texcoord.xy;
			//o.color = v.color;
			//float lerpValue = clamp((_FogStart - o.pos.z) / (_FogEnd - _FogStart), 0, 1);
			//o.color.a = lerp (_FogColor.a, o.color.a, lerpValue);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float4 c = float4(1,1,1,1);
			c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Gloss = c.a;
			o.Alpha = c.a * _Color.a; 
			//o.Albedo = c.rgb * _Color.rgb * IN.color.rgb;
			//o.Alpha = c.a * _Color.a * IN.color.a; 
		}

		void finalcolor (Input IN, SurfaceOutput o, inout fixed4 color) {
			float lerpValue = clamp((_FogStart - IN.pos.z) / (_FogEnd - _FogStart), 0, 1);
			color.rgba = lerp (_FogColor.rgba, color.rgba, lerpValue);
			//color.a = lerp (_FogColor.a, color.a, lerpValue);
		} 
		ENDCG
    }
}