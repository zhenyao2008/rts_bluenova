﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.2)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			half2 texcoord : TEXCOORD0;
		};
		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
			half2 texcoord : TEXCOORD0;
		};
		uniform float _Outline;
		uniform float4 _OutlineColor;
		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);
			o.pos.xy += offset * o.pos.z * _Outline;
			o.color = _OutlineColor;
			o.texcoord = v.texcoord;
			return o;
		}
	ENDCG
	SubShader {
		Tags { "Queue" = "Transparent" }
		Pass {
			Tags { "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			//ZTest Always//始终通过深度测试，即可以渲染
			//ColorMask RGB // alpha not used
			Blend SrcAlpha OneMinusSrcAlpha // Normal

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR {
				return i.color;
			}
			ENDCG
		}
		
        Pass { 
            CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t {
                half4 vertex : POSITION;
                half4 color : COLOR;
               	half2 texcoord : TEXCOORD0;
            };
            
            struct v2f1 {
                half4 vertex : POSITION;
                half4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            uniform half4 _Color;  
            v2f1 vert (appdata_t v)
            {
                v2f1 o;
                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.color = v.color;
               // o.texcoord = TRANSFORM_TEX(v.texcoord,_HighLightTex);
                o.texcoord = v.texcoord;
                return o;
            }
            
            half4 frag (v2f1 i) : COLOR
            {
				half4 baseColor = tex2D(_MainTex, i.texcoord);
            	if(baseColor.a>0)
            	{
            		baseColor = lerp(baseColor,baseColor * _Color,baseColor.a);
            	}
            	return baseColor;
            }
            
            
			ENDCG
		}
		
//		Pass {
//			Name "BASE"
//			ZWrite On
//			ZTest LEqual
//			Blend SrcAlpha OneMinusSrcAlpha
//			Material {
//				Diffuse [_Color]
//				Ambient [_Color]
//			}
//			Lighting On
//			SetTexture [_MainTex] {
//				ConstantColor [_Color]
//				Combine texture * constant
//			}
//			SetTexture [_MainTex] {
//				Combine previous * primary DOUBLE
//			}
//		}
	}
	Fallback "Diffuse"
}