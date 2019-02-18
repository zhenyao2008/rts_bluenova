Shader "Mobile/Transparent/Rim Only" {
	Properties {
		_RimColor("RimPower",Color) = (0,0,0,1)
		_RimPower("RimPower",Float) = 2
	}

 
	SubShader {
	
		Tags { "Queue" = "Transparent" }
//		Blend One OneMinusDstColor
//Cull Off
		 Blend One OneMinusDstColor
	 	CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		#include "UnityCG.cginc"
		float4 _RimColor;
		float _RimPower;

		struct Input {
			float3 worldNormal;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float nl = 1.0 - saturate(dot(normalize(IN.viewDir),IN.worldNormal));
			o.Albedo = _RimColor.rgb * pow(nl,_RimPower);
			o.Alpha = 1;
		}
		ENDCG
 
	}
 
	Fallback "Diffuse"
}
