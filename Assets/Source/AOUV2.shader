Shader "Custom/AOUV2"
{
	Properties{
			_MainTex("Base (RGB)", 2D) = "white" {}
			_AoTex("AO (RGB)", 2D) = "white" {}

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			sampler2D _AoTex;


			struct Input {
				float2 uv_MainTex : TEXCOORD0;
				float2 uv2_AoTex : TEXCOORD1;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				half4 c = tex2D(_MainTex, IN.uv_MainTex.xy);
				half4 ao = tex2D(_AoTex, IN.uv2_AoTex.xy);
				o.Albedo = c.rgb * ao.rgb;
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}
