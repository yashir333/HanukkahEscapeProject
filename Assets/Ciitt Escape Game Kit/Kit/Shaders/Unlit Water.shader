Shader "Custom/Unlit/Water"
{

	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull mode", Float) = 2
		[Toggle(MoveX)] _MoveX("Move X", Float) = 0
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Speed("Speed", Range(0, 5)) = 1
		_Sync("Sync", Range(0.1, 0.9)) = 0.5
	}

	SubShader
	{

		Tags
		{
			"RenderType" = "Opaque"
		}

		Cull[_Cull]
		LOD 100

		Pass
		{
		
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Speed;
			fixed _Sync;

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord.zw = o.texcoord.xy;

				float speed = _Speed * _Time.y;

				o.texcoord.x += speed;
				o.texcoord.z += speed * _Sync;

				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col1 = tex2D(_MainTex, i.texcoord.xy) * i.color;
				fixed4 col2 = tex2D(_MainTex, i.texcoord.zw) * i.color;
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//UNITY_OPAQUE_ALPHA(col.a);
				
				return (col1 + col2) * 0.5;
			}

			ENDCG

		}

	}

	Fallback "Mobile/VertexLit"

}
