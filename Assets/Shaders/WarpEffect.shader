// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WarpEffect"
{
	Properties
	{
		_GlowColor("GlowColor", Color) = (0.08561765,0.4423579,0.490566,0)
		_NormalFresnelPower("NormalFresnelPower", Float) = 5
		_NormalDistortion("NormalDistortion", Float) = 0.5
		_GlowFresnelPower("GlowFresnelPower", Float) = 2
		_Border("Border", Float) = 0.5
		_RemapPower("RemapPower", Float) = 0
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent+1000" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		GrabPass{ }

		Pass
		{
			Name "Unlit"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float3 ase_normal : NORMAL;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float3 ase_normal : NORMAL;
			};

			uniform float _Border;
			uniform float _RemapPower;
			uniform float _GlowFresnelPower;
			uniform float4 _GlowColor;
			uniform sampler2D _GrabTexture;
			uniform float _NormalDistortion;
			uniform float _NormalFresnelPower;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				o.ase_texcoord2 = v.vertex;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float3 ase_worldPos = i.ase_texcoord.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float fresnelNdotV9 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode9 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV9, 1.0 ) );
				float temp_output_47_0 = ( _Border + 1.0 );
				float temp_output_38_0 = (0.0 + (fresnelNode9 - 0.0) * (temp_output_47_0 - 0.0) / (1.0 - 0.0));
				float temp_output_48_0 = pow( temp_output_38_0 , _RemapPower );
				float ifLocalVar44 = 0;
				if( temp_output_38_0 <= 1.0 )
				ifLocalVar44 = temp_output_48_0;
				else
				ifLocalVar44 = (1.0 + (temp_output_38_0 - 1.0) * (0.0 - 1.0) / (temp_output_47_0 - 1.0));
				float4 unityObjectToClipPos26 = UnityObjectToClipPos( ( i.ase_texcoord2.xyz + ( -i.ase_normal * _NormalDistortion * pow( ifLocalVar44 , _NormalFresnelPower ) ) ) );
				float4 computeScreenPos25 = ComputeScreenPos( unityObjectToClipPos26 );
				float4 screenColor4 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( computeScreenPos25 ) );
				
				
				finalColor = ( ( pow( ifLocalVar44 , _GlowFresnelPower ) * _GlowColor ) + screenColor4 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
424;73;1130;656;2508.534;649.8093;1.3;True;True
Node;AmplifyShaderEditor.RangedFloatNode;46;-2226.45,-299.2688;Float;False;Property;_Border;Border;4;0;Create;True;0;0;False;0;0.5;0.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-2075.633,-269.1055;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;9;-2146.546,-524.0185;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1737.634,-227.9595;Float;False;Property;_RemapPower;RemapPower;5;0;Create;True;0;0;False;0;0;3.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-1861.988,-436.899;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;48;-1567.334,-442.4595;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-1814.765,-124.7817;Float;False;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1554.905,536.3648;Float;False;Property;_NormalFresnelPower;NormalFresnelPower;1;0;Create;True;0;0;False;0;5;2.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;44;-1496.387,-184.6001;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;28;-1377.455,187.774;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-1244.777,335.7091;Float;False;Property;_NormalDistortion;NormalDistortion;2;0;Create;True;0;0;False;0;0.5;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;37;-1206.933,441.0291;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;32;-1179.972,210.5574;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;27;-1082.418,36.7063;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1013.027,231.607;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-837.4181,107.7063;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-904.9574,-322.9704;Float;False;Property;_GlowFresnelPower;GlowFresnelPower;3;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;26;-690.2051,110.4416;Float;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;36;-568.274,-409.202;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-432.8,-243;Float;False;Property;_GlowColor;GlowColor;0;0;Create;True;0;0;False;0;0.08561765,0.4423579,0.490566,0;0.08561751,0.4423577,0.490566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComputeScreenPosHlpNode;25;-452.4635,78.20685;Float;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-146.1732,-266.3385;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;4;-193.305,52.33607;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-5,-79.5;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;262.1789,-189.92;Float;False;True;2;Float;ASEMaterialInspector;0;1;WarpEffect;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;1;False;-1;0;1;False;-1;1;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=1000;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;47;0;46;0
WireConnection;38;0;9;0
WireConnection;38;4;47;0
WireConnection;48;0;38;0
WireConnection;48;1;49;0
WireConnection;45;0;38;0
WireConnection;45;2;47;0
WireConnection;44;0;38;0
WireConnection;44;2;45;0
WireConnection;44;3;48;0
WireConnection;44;4;48;0
WireConnection;37;0;44;0
WireConnection;37;1;34;0
WireConnection;32;0;28;0
WireConnection;30;0;32;0
WireConnection;30;1;31;0
WireConnection;30;2;37;0
WireConnection;29;0;27;0
WireConnection;29;1;30;0
WireConnection;26;0;29;0
WireConnection;36;0;44;0
WireConnection;36;1;10;0
WireConnection;25;0;26;0
WireConnection;7;0;36;0
WireConnection;7;1;3;0
WireConnection;4;0;25;0
WireConnection;5;0;7;0
WireConnection;5;1;4;0
WireConnection;0;0;5;0
ASEEND*/
//CHKSM=F42D6FF8F923DCDBE499A391858CEFD0FDC3F6C7