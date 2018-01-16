Shader "Hidden/BoidsSimulationOnGPU/BoidsRender"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
		_PosTex("position texture", 2D) = "black"{}
		_NmlTex("normal texture", 2D) = "white"{}
		_DT ("delta time", float) = 0
		_Length ("animation length", Float) = 1
		[Toggle(ANIM_LOOP)] _Loop("loop", Float) = 0
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma instancing_options procedural:setup
		#pragma multi_compile ___ ANIM_LOOP//
		#pragma multi_compile_instancing//
		#define ts _PosTex_TexelSize
		
		struct Input
		{
			float2 uv_MainTex;
			float4 color : COLOR;            		
		};
		
		// Boidの構造体
		struct BoidData
		{
			float3 velocity; // 速度
			float3 position; // 位置
			float3 color;
			float dt;
			
		};

		#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            // Boidデータの構造体バッファ
            StructuredBuffer<BoidData> _BoidDataBuffer;
            //float _DT;
            float _Length;
		#endif
		
		

        UNITY_INSTANCING_CBUFFER_START (MyProperties)
            //            UNITY_DEFINE_INSTANCED_PROP (float4, _Col)
            UNITY_DEFINE_INSTANCED_PROP (float, _DT)
        UNITY_INSTANCING_CBUFFER_END

		sampler2D _MainTex; // テクスチャ
        sampler2D _PosTex, _NmlTex;

		half   _Glossiness; // 光沢
		half   _Metallic;   // 金属特性
		fixed4 _Color;      // カラー
        float4 _PosTex_TexelSize;
		float3 _ObjectScale; // Boidオブジェクトのスケール

		// オイラー角（ラジアン）を回転行列に変換
		float4x4 eulerAnglesToRotationMatrix(float3 angles)
		{
			float ch = cos(angles.y); float sh = sin(angles.y); // heading
			float ca = cos(angles.z); float sa = sin(angles.z); // attitude
			float cb = cos(angles.x); float sb = sin(angles.x); // bank

			// Ry-Rx-Rz (Yaw Pitch Roll)
			return float4x4(
				ch * ca + sh * sb * sa, -ch * sa + sh * sb * ca, sh * cb, 0,
				cb * sa, cb * ca, -sb, 0,
				-sh * ca + ch * sb * sa, sh * sa + ch * sb * ca, ch * cb, 0,
				0, 0, 0, 1
			);
		}

		// 頂点シェーダ
		void vert(inout appdata_full v)
		{
			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

            //koko
                //UNITY_SETUP_INSTANCE_ID (v);
                //UNITY_TRANSFER_INSTANCE_ID (v, o);

			// インスタンスIDからBoidのデータを取得
			BoidData boidData = _BoidDataBuffer[unity_InstanceID]; 

			float3 pos = boidData.position.xyz; // Boidの位置を取得、グローバルな座標
			float3 scl = _ObjectScale;          // Boidのスケールを取得
            pos.y=0;
            
			float t = (_Time.y - boidData.dt) / _Length;
#if ANIM_LOOP
			t = fmod(t, 1.0);
#else
			t = saturate(t);
#endif
			
			float x = (v.texcoord.x + 0.5) * ts.x;
			float y = t;
			float4 pos2 = tex2Dlod(_PosTex, float4(x, y, 0, 0));
			float3 normal = tex2Dlod(_NmlTex, float4(x, y, 0, 0));            
            //pos = pos2.xyz;

			// オブジェクト座標からワールド座標に変換する行列を定義
			float4x4 object2world = (float4x4)0; 
			// スケール値を代入
			object2world._11_22_33_44 = float4(scl.xyz, 1.0);
			// 速度からY軸についての回転を算出
			float rotY = atan2(boidData.velocity.x, boidData.velocity.z);
			// 速度からX軸についての回転を算出
			float rotX = 0;//-asin(boidData.velocity.y / (length(boidData.velocity.xyz) + 1e-8));
			// オイラー角（ラジアン）から回転行列を求める
			float4x4 rotMatrix = eulerAnglesToRotationMatrix(float3(rotX, rotY, 0));
			// 行列に回転を適用
			object2world = mul(rotMatrix, object2world);
			// 行列に位置（平行移動）を適用
			object2world._14_24_34 += pos.xyz;

            



			// 頂点を座標変換
			v.vertex = mul(object2world, pos2);
			// 法線を座標変換
			v.normal = normalize(mul(object2world, v.normal));
			
			v.color = float4(boidData.color,1.0);//float4( 1.0,1.0,0,1.0 );
			//v.color = 
			#endif
		}

		
		
		void setup()
		{
		}

		// サーフェスシェーダ
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = IN.color.rgb;//c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}