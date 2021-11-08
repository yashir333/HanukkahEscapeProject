Shader "Custom/Mobile/Diffuse Specular Simulate"
{

	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull mode", Float) = 2
		_MainTex("Base (RGB)", 2D) = "white" {}
		_SpecTex("Specular (RGB)", 2D) = "white" {}
		_Speed("Speed", Float) = 1
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
		sampler2D _SpecTex;
		float _Speed;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SpecTex;
			float2 uv_SpecTexR;
			fixed4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c1 = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			fixed4 c2 = tex2D(_SpecTex, IN.uv_SpecTex + (_Time.y * _Speed));
			o.Albedo = c1.rgb + c2.rgb;
			o.Alpha = c1.a;
		}

		ENDCG

	}

	Fallback "Mobile/VertexLit"

}
