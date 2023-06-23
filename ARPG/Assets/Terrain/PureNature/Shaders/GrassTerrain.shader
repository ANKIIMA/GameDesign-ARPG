// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/TerrainEngine/Details/WavingDoublePass"
{
	Properties
	{
		_WavingTint("WavingTint", Color) = (1,0.3215686,0,1)
		_MainTex("MainTex", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_WaveAndDistance("WaveAndDistance", Vector) = (12,3.6,1,1)
		_ColorVariationPower("Color Variation Power", Range( 0 , 1)) = 1
		_NoiseScale("Noise Scale", Float) = 2
		[Toggle(_WINDDEBUGVIEW_ON)] _WindDebugView("WindDebugView", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.0
		#pragma multi_compile_instancing
		#pragma shader_feature _WINDDEBUGVIEW_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 screenPosition;
			float eyeDepth;
		};

		uniform float MicroFrequency;
		uniform float MicroSpeed;
		uniform float MicroPower;
		uniform float4 _WaveAndDistance;
		uniform float4 _WavingTint;
		uniform float _ColorVariationPower;
		uniform float _NoiseScale;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float GrassRenderDist;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline float Dither8x8Bayer( int x, int y )
		{
			const float dither[ 64 ] = {
				 1, 49, 13, 61,  4, 52, 16, 64,
				33, 17, 45, 29, 36, 20, 48, 32,
				 9, 57,  5, 53, 12, 60,  8, 56,
				41, 25, 37, 21, 44, 28, 40, 24,
				 3, 51, 15, 63,  2, 50, 14, 62,
				35, 19, 47, 31, 34, 18, 46, 30,
				11, 59,  7, 55, 10, 58,  6, 54,
				43, 27, 39, 23, 42, 26, 38, 22};
			int r = y * 8 + x;
			return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 appendResult105 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
			float2 temp_cast_0 = (( MicroSpeed * 0.5 )).xx;
			float2 panner102 = ( 1.0 * _Time.y * temp_cast_0 + appendResult105.xy);
			float simplePerlin2D101 = snoise( panner102 );
			simplePerlin2D101 = simplePerlin2D101*0.5 + 0.5;
			float4 MicroWind37 = ( ( float4( ( sin( ( MicroFrequency * ( appendResult105 + simplePerlin2D101 ) ) ) * MicroPower ) , 0.0 ) * _WaveAndDistance ) * v.color.a );
			v.vertex.xyz += MicroWind37.xyz;
			v.vertex.w = 1;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 appendResult40 = (float2(ase_worldPos.x , ase_worldPos.z));
			float simplePerlin2D41 = snoise( appendResult40*( _NoiseScale / 100.0 ) );
			simplePerlin2D41 = simplePerlin2D41*0.5 + 0.5;
			float4 lerpResult48 = lerp( i.vertexColor , _WavingTint , ( _ColorVariationPower * pow( simplePerlin2D41 , 2.0 ) ));
			float4 Color208 = ( lerpResult48 * i.vertexColor );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 Texture204 = tex2D( _MainTex, uv_MainTex );
			float3 appendResult105 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
			float2 temp_cast_0 = (( MicroSpeed * 0.5 )).xx;
			float2 panner102 = ( 1.0 * _Time.y * temp_cast_0 + appendResult105.xy);
			float simplePerlin2D101 = snoise( panner102 );
			simplePerlin2D101 = simplePerlin2D101*0.5 + 0.5;
			float4 MicroWind37 = ( ( float4( ( sin( ( MicroFrequency * ( appendResult105 + simplePerlin2D101 ) ) ) * MicroPower ) , 0.0 ) * _WaveAndDistance ) * i.vertexColor.a );
			#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch107 = MicroWind37;
			#else
				float4 staticSwitch107 = ( Color208 * (Texture204).r );
			#endif
			o.Albedo = staticSwitch107.rgb;
			o.Smoothness = ( (Texture204).g * 0.2 );
			o.Alpha = 1;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen154 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither154 = Dither8x8Bayer( fmod(clipScreen154.x, 8), fmod(clipScreen154.y, 8) );
			float cameraDepthFade155 = (( i.eyeDepth -_ProjectionParams.y - GrassRenderDist ) / GrassRenderDist);
			dither154 = step( dither154, ( 1.0 - cameraDepthFade155 ) );
			float DistanceFade202 = dither154;
			float2 clipScreen210 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither210 = Dither8x8Bayer( fmod(clipScreen210.x, 8), fmod(clipScreen210.y, 8) );
			float3 temp_cast_5 = ((1.5 + (i.uv_texcoord.y - 0.11) * (8.0 - 1.5) / (0.52 - 0.11))).xxx;
			float3 temp_cast_6 = ((1.5 + (i.uv_texcoord.y - 0.11) * (8.0 - 1.5) / (0.52 - 0.11))).xxx;
			float3 gammaToLinear226 = GammaToLinearSpace( temp_cast_6 );
			float3 clampResult217 = clamp( gammaToLinear226 , float3( 0,0,0 ) , float3( 1,1,1 ) );
			dither210 = step( dither210, clampResult217.x );
			float BaseOpacity218 = dither210;
			clip( ( ( (Texture204).a * DistanceFade202 ) * BaseOpacity218 ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18900
752;73;775;650;6063.098;3441.082;6.249369;True;False
Node;AmplifyShaderEditor.CommentaryNode;160;-4224,-737;Inherit;False;2946.128;639.272;;17;37;54;55;35;33;32;29;28;27;22;101;102;105;24;110;109;231;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;24;-4192,-481;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;109;-4192,-673;Float;False;Global;MicroSpeed;MicroSpeed;18;1;[HideInInspector];Create;False;0;0;0;False;0;False;2;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-3968,-673;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;-3936,-449;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;201;-4224,-1472;Inherit;False;2083.608;708.0986;;14;1;208;51;50;48;47;45;46;41;44;40;42;39;43;Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;102;-3712,-593;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-4080,-944;Inherit;False;Property;_NoiseScale;Noise Scale;5;0;Create;True;0;0;0;False;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-4176,-1120;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;-3888,-960;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;-3888,-1088;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;101;-3504,-593;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-3536,-864;Inherit;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;41;-3632,-1088;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-3200,-673;Float;False;Global;MicroFrequency;MicroFrequency;19;1;[HideInInspector];Create;False;0;0;0;False;0;False;5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;220;-4224,-1792;Inherit;False;1153.323;281;;6;218;210;217;226;224;211;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;200;-4224,-2048;Inherit;False;995.633;221.7;;5;202;154;159;155;156;Distance Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-3200,-449;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-4192,-1984;Inherit;False;Global;GrassRenderDist;GrassRenderDist;9;0;Create;True;0;0;0;False;0;False;50;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;45;-3344,-976;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2944,-545;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-3504,-1216;Inherit;False;Property;_ColorVariationPower;Color Variation Power;4;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;211;-4192,-1728;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-3072,-1408;Inherit;False;Property;_WavingTint;WavingTint;0;0;Create;True;0;0;0;False;0;False;1,0.3215686,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-2560,-321;Float;False;Global;MicroPower;MicroPower;20;0;Create;False;0;0;0;False;0;False;0.05;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-3152,-1184;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;224;-3968,-1728;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0.11;False;2;FLOAT;0.52;False;3;FLOAT;1.5;False;4;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;155;-4000,-1984;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;50;-3072,-1024;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;29;-2528,-546.3871;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;55;-2048,-449;Inherit;False;Property;_WaveAndDistance;WaveAndDistance;3;0;Create;True;0;0;0;False;0;False;12,3.6,1,1;12,3.6,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;48;-2784,-1232;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2304,-545;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GammaToLinearNode;226;-3696,-1728;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;159;-3760,-1984;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-4224,-2256;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;0;False;0;False;-1;f260bf73a8296534786c8f7e5bfb0acf;f260bf73a8296534786c8f7e5bfb0acf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;204;-3904,-2192;Inherit;False;Texture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1792,-545;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DitheringNode;154;-3616,-1984;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;231;-1839.507,-306.364;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;217;-3648,-1648;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-2496,-1040;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;202;-3424,-1984;Inherit;False;DistanceFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1651.977,-378.4027;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;208;-2352,-1024;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;210;-3488,-1664;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;205;-2048,-1920;Inherit;False;204;Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;207;-1792,-2016;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;209;-1792,-2112;Inherit;False;208;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;206;-1792,-1856;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1632,-545;Inherit;False;MicroWind;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;218;-3280,-1664;Inherit;False;BaseOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;-1792,-1728;Inherit;False;202;DistanceFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;228;-1792,-1936;Inherit;False;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;219;-1552,-1648;Inherit;False;218;BaseOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;106;-1536,-1920;Inherit;False;37;MicroWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;227;-1281.461,-1789.956;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1520,-2048;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-1536,-1792;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;107;-1280,-2000;Inherit;False;Property;_WindDebugView;WindDebugView;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-1280,-1584;Inherit;False;37;MicroWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-1280,-1696;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;-1105.402,-1865.315;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-976,-1920;Float;False;True;-1;4;;0;0;Standard;Hidden/TerrainEngine/Details/WavingDoublePass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;110;0;109;0
WireConnection;105;0;24;1
WireConnection;105;1;24;3
WireConnection;105;2;24;2
WireConnection;102;0;105;0
WireConnection;102;2;110;0
WireConnection;42;0;43;0
WireConnection;40;0;39;1
WireConnection;40;1;39;3
WireConnection;101;0;102;0
WireConnection;41;0;40;0
WireConnection;41;1;42;0
WireConnection;27;0;105;0
WireConnection;27;1;101;0
WireConnection;45;0;41;0
WireConnection;45;1;44;0
WireConnection;28;0;22;0
WireConnection;28;1;27;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;224;0;211;2
WireConnection;155;0;156;0
WireConnection;155;1;156;0
WireConnection;29;0;28;0
WireConnection;48;0;50;0
WireConnection;48;1;1;0
WireConnection;48;2;47;0
WireConnection;35;0;29;0
WireConnection;35;1;33;0
WireConnection;226;0;224;0
WireConnection;159;0;155;0
WireConnection;204;0;2;0
WireConnection;54;0;35;0
WireConnection;54;1;55;0
WireConnection;154;0;159;0
WireConnection;217;0;226;0
WireConnection;51;0;48;0
WireConnection;51;1;50;0
WireConnection;202;0;154;0
WireConnection;32;0;54;0
WireConnection;32;1;231;4
WireConnection;208;0;51;0
WireConnection;210;0;217;0
WireConnection;207;0;205;0
WireConnection;206;0;205;0
WireConnection;37;0;32;0
WireConnection;218;0;210;0
WireConnection;228;0;205;0
WireConnection;3;0;209;0
WireConnection;3;1;207;0
WireConnection;158;0;206;0
WireConnection;158;1;203;0
WireConnection;107;1;3;0
WireConnection;107;0;106;0
WireConnection;212;0;158;0
WireConnection;212;1;219;0
WireConnection;229;0;228;0
WireConnection;229;1;227;0
WireConnection;0;0;107;0
WireConnection;0;4;229;0
WireConnection;0;10;212;0
WireConnection;0;11;38;0
ASEEND*/
//CHKSM=FAB636819D0093356908D369501B856452740D0F