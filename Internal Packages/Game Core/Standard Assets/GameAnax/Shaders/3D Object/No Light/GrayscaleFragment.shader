    Shader "Unity/GreyScaleFragment" {
	     Properties {
		    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		    _EffectAmount ("Effect Amount", Range (0, 1)) = 0.0
	   	 }
	    Category
	    {
		    ZWrite On
		    Alphatest Greater 0.5
		    Cull Off
		    SubShader {
		    Blend SrcAlpha OneMinusSrcAlpha
			    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
			    LOD 200
			    Pass { 
				    CGPROGRAM
				    #pragma vertex vert_img
				    #pragma fragment frag
				    #include "UnityCG.cginc"
				    
				    uniform sampler2D _MainTex;
				    uniform float _EffectAmount;
				     
				    half4 frag (v2f_img i) : Color {
				    half4 c = tex2D(_MainTex, i.uv);
				    c.rgb = lerp(c.rgb, dot(c.rgb, float3(0.3, 0.59, 0.11)), _EffectAmount);
				    return c;
				    }
			     
			    ENDCG
			    }
		    }
	    }
	    Fallback "Unlit/Transparent"
    }