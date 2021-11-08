Shader "Custom/Mobile/Diffuse VC Flash"
{

	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull mode", Float) = 2
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Flash("Flash", Range(0,1)) = 0
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
		fixed _Flash;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;

			o.Albedo = c.rgb + _Flash;

			o.Alpha = c.a;
		}

		ENDCG

	}

	Fallback "Mobile/VertexLit"

}
