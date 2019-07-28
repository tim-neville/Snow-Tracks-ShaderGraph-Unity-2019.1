Shader "myShader_noTransparency" {
	Properties {
		[NoScaleOffset] _Cube("Reflection Map", Cube) = "" {}
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Ref ("display name", Range (0.0, 5.0)) = 0.5
		_AlphaRange ("Alpha Value", Range (0.0, 0.9)) = 0.5
	}
	SubShader {
		
		Pass {
			
			Cull back

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;

			struct vertData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(vertData input) {
				v2f output;
				output.uv = input.uv;
				output.position = UnityObjectToClipPos(input.position);
				return output;
			}

			float4 frag(v2f output) : COLOR {
				float4 finalCoror = tex2D(_MainTex, output.uv.xy);
				if (finalCoror.a < 0.9) {
					discard;
				}
				return finalCoror;
			}
			ENDCG
		}

		Tags { "Queue" = "Transparent" }

		Pass {

			Cull back

			CGPROGRAM
				#pragma vertex vert  
				#pragma fragment frag
				#include "UnityCG.cginc"

				// User-specified uniforms
				uniform samplerCUBE _Cube;
				uniform sampler2D _MainTex;

		        struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
				};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normalDir : TEXCOORD0;
					float3 viewDir : TEXCOORD1;
					float2 uv : TEXCOORD2;
				};

				vertexOutput vert(vertexInput input) {
					vertexOutput output;
					
					float4x4 modelMatrix = unity_ObjectToWorld;
					float4x4 modelMatrixInverse = unity_WorldToObject;
 
					output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
					output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
					output.pos = UnityObjectToClipPos(input.vertex);
					output.uv = input.uv;
					return output;
				}

				uniform float _Ref;
				uniform float _AlphaRange;

				float4 frag(vertexOutput input) : COLOR {
					float4 textureColor = tex2D(_MainTex, input.uv.xy);
					float3 reflectedDir = reflect(input.viewDir, normalize(input.normalDir));
					if (textureColor.a > _AlphaRange) {
						discard;
					}
				return texCUBE(_Cube, reflectedDir) * textureColor * _Ref;
				}
			ENDCG
		}
	}
}