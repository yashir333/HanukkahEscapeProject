Shader "Custom/Mobile/Diffuse VC Invert"
{

	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull mode", Float) = 2
		[Toggle(Invert)] _Invert("Invert", Float) = 0
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{

		Tags
		{
			"RenderType" = "Opaque"
		}

		Cull[_Cull]
		LOD 150

		CGPROGRAM

		#pragma surface surf Lambert noforwardadd

		#pragma shader_feature Invert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			
#ifdef Invert
			o.Albedo = 1 - c.rgb;

#else
			o.Albedo = c.rgb;
#endif

			o.Alpha = c.a;
		}

		ENDCG

	}

	Fallback "Mobile/VertexLit"

}
