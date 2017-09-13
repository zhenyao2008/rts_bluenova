// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/Diffuse-RimWithBump" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_BumpMap("BumpMap",2D) = "bump" {}
	_RimColor ("Rim Color", Color) = (1,1,1,1)
	_RimPower("Rim Power",Float) = 2
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd
#include "UnityCG.cginc"
sampler2D _MainTex;
sampler2D _BumpMap;
float4 _RimColor;
float _RimPower;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	float nl = 1.0 - saturate(dot(normalize(IN.viewDir),o.Normal));
	o.Emission = _RimColor.rgb * pow(nl,_RimPower);
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
