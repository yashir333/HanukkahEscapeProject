Shader "Custom/Mobile/Diffuse VC"
{

	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull mode", Float) = 2
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

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG

	}

	Fallback "Mobile/VertexLit"

}
