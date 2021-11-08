Shader "Custom/Unlit/Texture Alpha VC"
{

	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull mode", Float) = 2
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendMode1("Blend mode 1", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendMode2("Blend mode 2", Float) = 10
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{

		Tags
		{
			"Queue"="Transparent"
			"RenderType" = "Transparent"
		}

		Cull[_Cull]
		LOD 100
		Blend [_BlendMode1] [_BlendMode2]
		ZWrite Off
		
		Pass
		{

			CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                //UNITY_OPAQUE_ALPHA(col.a);
                return col;
            }

        ENDCG

		}
	}

}
