Shader "Custom/Shield" {
	Properties{
		_Color1("Color 1", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Frequency("Frequency", Float) = 10
		_Extrusion("Extrusion", Float) = 0.2
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _Color1, _Color2;
		float _Extrusion;
		float _Frequency;

		struct Input {
			float2 uv_MainTex;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v) {
			v.vertex.xyz += v.normal * _Extrusion;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float f = sin(_Time.y * _Frequency) * 0.5f + 1;
			fixed4 c = lerp(_Color1, _Color2, f);
			o.Emission = c.rgb;
			o.Albedo = fixed3(0, 0, 0);
			o.Smoothness = 0;
			o.Alpha = _Color1.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
