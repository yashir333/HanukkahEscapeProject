Shader "Custom/Sprite/Click Effect"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		_MaskTex("Mask Texture", 2D) = "white" {}
		_Scale("Scale", Float) = 1
		_Alpha("Alpha", Range(0,1)) = 1

    }

    SubShader
    {

        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

			CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

			float _Scale;
			fixed _Alpha;

			struct myv2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			myv2f vert(appdata_t IN)
			{
				myv2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
#endif



				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord.xy = IN.texcoord;
				OUT.texcoord.zw = ((IN.texcoord - 0.5) * _Scale) + 0.5;
				OUT.color = IN.color * _Color * _RendererColor;

#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

				return OUT;
			}

			sampler2D _MaskTex;

			fixed4 frag(myv2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord.xy) * IN.color;
				fixed mask = tex2D(_MaskTex, IN.texcoord.zw).r;
				c.a *= mask * _Alpha;
				return c;
			}

			ENDCG

        }

    }

}
