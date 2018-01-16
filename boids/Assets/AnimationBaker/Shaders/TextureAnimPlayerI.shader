// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Unlit/TextureAnimPlayerI"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PosTex("position texture", 2D) = "black"{}
		_NmlTex("normal texture", 2D) = "white"{}
		_DT ("delta time", float) = 0
		_Col ("col", Color) = (1.0,0,0,1.0)		
		_Length ("animation length", Float) = 1
		[Toggle(ANIM_LOOP)] _Loop("loop", Float) = 0
	}
	SubShader
	{
		
		LOD 100 Cull Off

		Pass
		{
		    Tags { "RenderType"="Opaque" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target es3.0
            #pragma multi_compile_instancing			
			#pragma multi_compile ___ ANIM_LOOP
			#include "UnityCG.cginc"

			#define ts _PosTex_TexelSize

			struct appdata
			{
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex, _PosTex, _NmlTex;
			float4 _PosTex_TexelSize;
			float _Length;
			
            UNITY_INSTANCING_CBUFFER_START (MyProperties)
                UNITY_DEFINE_INSTANCED_PROP (float4, _Col)
                UNITY_DEFINE_INSTANCED_PROP (float, _DT)
            UNITY_INSTANCING_CBUFFER_END
			
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
			 
			    v2f o;
			    UNITY_SETUP_INSTANCE_ID (v);
                UNITY_TRANSFER_INSTANCE_ID (v, o);
			
				float t = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(_DT)) / _Length;
#if ANIM_LOOP
				t = fmod(t, 1.0);
#else
				t = saturate(t);
#endif
				float x = (vid + 0.5) * ts.x;
				float y = t;
				float4 pos = tex2Dlod(_PosTex, float4(x, y, 0, 0));
				float3 normal = tex2Dlod(_NmlTex, float4(x, y, 0, 0));

				
				o.vertex = UnityObjectToClipPos(pos);
				o.normal = UnityObjectToWorldNormal(normal);
				o.uv = v.uv;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
			    UNITY_SETUP_INSTANCE_ID (i);
				half diff = dot(i.normal, float3(0,1,0))*0.5 + 0.5;
				half4 col = tex2D(_MainTex, i.uv);
				
				return diff * col * UNITY_ACCESS_INSTANCED_PROP (_Col);
			}
			ENDCG
		}
		
		/////////////////
		
		
		Pass
		{
		    Tags { "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma multi_compile_instancing			
			#pragma multi_compile ___ ANIM_LOOP
			#include "UnityCG.cginc"

			#define ts _PosTex_TexelSize

			struct appdata
			{
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex, _PosTex, _NmlTex;
			float4 _PosTex_TexelSize;
			float _Length;
			
            UNITY_INSTANCING_CBUFFER_START (MyProperties)
                UNITY_DEFINE_INSTANCED_PROP (float4, _Col)
                UNITY_DEFINE_INSTANCED_PROP (float, _DT)
            UNITY_INSTANCING_CBUFFER_END
			
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
			 
			    v2f o;
			    UNITY_SETUP_INSTANCE_ID (v);
                UNITY_TRANSFER_INSTANCE_ID (v, o);
			
				float t = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(_DT)) / _Length;
#if ANIM_LOOP
				t = fmod(t, 1.0);
#else
				t = saturate(t);
#endif
				float x = (vid + 0.5) * ts.x;
				float y = t;
				float4 pos = tex2Dlod(_PosTex, float4(x, y, 0, 0));
				float3 normal = tex2Dlod(_NmlTex, float4(x, y, 0, 0));

				
				o.vertex = UnityObjectToClipPos(pos);
				o.normal = UnityObjectToWorldNormal(normal);
				o.uv = v.uv;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				//half diff = dot(i.normal, float3(0,1,0))*0.5 + 0.5;
				//half4 col = tex2D(_MainTex, i.uv);
				
				return _Col;
			}
			ENDCG
		}
		
		
		
	}
}
