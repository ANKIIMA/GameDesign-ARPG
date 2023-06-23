// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/WaterFall"
{
	Properties
	{
		[Space(10)][Header(Main Parameters)][Space(10)]_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalPower("Normal Power", Range( 0 , 1)) = 1
		_RefractionPower("Refraction Power", Range( 0 , 5)) = 1
		_NormalScale("Normal Scale", Float) = 1
		_NormalSpeed("Normal Speed", Float) = 1
		[Space(10)][Header(Foam)][Space(10)]_FoamMask("Foam Mask", 2D) = "white" {}
		_FoamNormal("Foam Normal Map", 2D) = "bump" {}
		_DepthColor("Depth Color (RGBA)", Color) = (0,0.6810271,0.6886792,1)
		_FoamPower("Foam Power", Range( 0 , 1)) = 0
		_DepthColor1("Foam Color", Color) = (0,0.6810271,0.6886792,1)
		_FoamScale("Foam Scale", Float) = 1
		_FoamSpeed("Foam Speed", Float) = 1
		[Space(10)][Header(Metallic Smoothness)][Space(10)]_MetallicPower("Metallic Power", Range( 0 , 1)) = 1
		_SmoothnessPower("Smoothness Power", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform sampler2D _NormalMap;
		uniform float _NormalSpeed;
		uniform float _NormalScale;
		uniform float _NormalPower;
		uniform sampler2D _FoamNormal;
		uniform float _FoamSpeed;
		uniform float _FoamScale;
		uniform sampler2D _FoamMask;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FoamPower;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _RefractionPower;
		uniform float4 _DepthColor;
		uniform float4 _DepthColor1;
		uniform float _MetallicPower;
		uniform float _SmoothnessPower;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_350_0 = ( _NormalSpeed / 10.0 );
			float2 appendResult337 = (float2(0.02 , temp_output_350_0));
			float2 UV32 = i.uv_texcoord;
			float2 temp_output_386_0 = ( UV32 * _NormalScale );
			float2 panner72 = ( 1.0 * _Time.y * appendResult337 + temp_output_386_0);
			float2 appendResult338 = (float2(-0.02 , ( temp_output_350_0 * 0.7 )));
			float2 panner73 = ( 1.0 * _Time.y * appendResult338 + temp_output_386_0);
			float temp_output_361_0 = ( _FoamSpeed / 10.0 );
			float2 appendResult363 = (float2(0.02 , temp_output_361_0));
			float2 temp_output_345_0 = ( UV32 * _FoamScale );
			float2 panner213 = ( 1.0 * _Time.y * appendResult363 + temp_output_345_0);
			float2 appendResult364 = (float2(-0.02 , ( temp_output_361_0 * 0.7 )));
			float2 panner254 = ( 1.0 * _Time.y * appendResult364 + temp_output_345_0);
			float temp_output_370_0 = ( i.vertexColor.r - ( tex2D( _FoamMask, panner213 ).r * tex2D( _FoamMask, panner254 ).r ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth393 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth393 = abs( ( screenDepth393 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( ( temp_output_370_0 * 5.0 ) ) );
			float clampResult408 = clamp( ( 1.0 - distanceDepth393 ) , 0.0 , 1.0 );
			float Foam62 = ( 1.0 - step( ( temp_output_370_0 + clampResult408 ) , ( 1.0 - _FoamPower ) ) );
			float3 lerpResult237 = lerp( BlendNormals( UnpackScaleNormal( tex2D( _NormalMap, panner72 ), _NormalPower ) , UnpackScaleNormal( tex2D( _NormalMap, panner73 ), _NormalPower ) ) , UnpackScaleNormal( tex2D( _FoamNormal, panner213 ), _NormalPower ) , Foam62);
			float3 Normals81 = lerpResult237;
			o.Normal = Normals81;
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor20 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm + float4( ( ( _RefractionPower / 10.0 ) * Normals81 ) , 0.0 ) ).xy);
			float4 clampResult21 = clamp( screenColor20 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Refraction60 = clampResult21;
			float4 blendOpSrc173 = Refraction60;
			float4 blendOpDest173 = _DepthColor;
			float4 lerpBlendMode173 = lerp(blendOpDest173,(( blendOpDest173 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest173 ) * ( 1.0 - blendOpSrc173 ) ) : ( 2.0 * blendOpDest173 * blendOpSrc173 ) ),( 1.0 - _DepthColor.a ));
			float4 lerpResult375 = lerp( ( saturate( lerpBlendMode173 )) , _DepthColor1 , Foam62);
			float4 Albedo58 = lerpResult375;
			o.Albedo = Albedo58.rgb;
			o.Metallic = _MetallicPower;
			o.Smoothness = _SmoothnessPower;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18800
490;73;1037;655;242.873;4274.572;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;371;-2174,-2304;Inherit;False;3582.755;636.4388;Foam;29;62;409;381;413;411;401;408;394;393;402;370;395;259;355;258;84;213;254;83;364;345;363;358;360;224;359;223;361;362;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;163;-2176,-5632;Inherit;False;448;190;World Space UVs;2;32;343;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;343;-2144,-5568;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;362;-2110,-2160;Inherit;False;Property;_FoamSpeed;Foam Speed;12;0;Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;361;-1950,-2160;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-1920,-5568;Inherit;False;UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;360;-1950,-2032;Inherit;False;Constant;_Float5;Float 5;27;0;Create;True;0;0;0;False;0;False;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-1694,-1920;Inherit;False;Property;_FoamScale;Foam Scale;11;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;224;-1694,-2000;Inherit;False;32;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;359;-1950,-1888;Inherit;False;Constant;_Float4;Float 4;27;0;Create;True;0;0;0;False;0;False;-0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;358;-1950,-1792;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;364;-1694,-1808;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;345;-1502,-1968;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;363;-1694,-2160;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;254;-1310,-1904;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;83;-1360,-2240;Inherit;True;Property;_FoamMask;Foam Mask;6;0;Create;True;0;0;0;False;3;Space(10);Header(Foam);Space(10);False;None;df6a164e45088bb4eaee92b8d6ba7514;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;213;-1310,-2032;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;84;-1054,-2160;Inherit;True;Property;_TextureSample2;Texture Sample 2;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;258;-1054,-1936;Inherit;True;Property;_TextureSample3;Texture Sample 3;18;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;-718,-2048;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;355;-576,-2240;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;370;-384,-2128;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;395;-352,-1792;Inherit;False;Constant;_05;0.5;25;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;402;-208,-1808;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;393;-80,-1808;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;165;-2176,-3104;Inherit;False;2304.887;764.5673;Normals;21;81;238;237;80;239;66;65;11;73;64;72;337;350;67;338;386;354;349;70;353;78;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2144,-3040;Inherit;False;Property;_NormalSpeed;Normal Speed;5;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;394;160,-1808;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;408;320,-1808;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;411;480,-1984;Inherit;False;Property;_FoamPower;Foam Power;9;0;Create;True;0;0;0;False;0;False;0;0.27;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;350;-1952,-3040;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-1808,-2608;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;353;-1808,-2736;Inherit;False;Constant;_Float3;Float 3;27;0;Create;True;0;0;0;False;0;False;-0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;401;480,-2128;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;354;-1568,-2800;Inherit;False;Property;_NormalScale;Normal Scale;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-1616,-2912;Inherit;False;32;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;413;752,-1984;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;349;-1808,-2864;Inherit;False;Constant;_Float2;Float 2;27;0;Create;True;0;0;0;False;0;False;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;338;-1616,-2672;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;337;-1584,-3040;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;381;896,-2128;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;386;-1392,-2864;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;409;1024,-2128;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1184,-2544;Inherit;False;Property;_NormalPower;Normal Power;2;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;72;-1184,-3040;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;73;-1184,-2688;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;64;-1184,-2896;Inherit;True;Property;_NormalMap;Normal Map;1;0;Create;True;0;0;0;False;3;Space(10);Header(Main Parameters);Space(10);False;None;cc1fed187616b44458fb104365c8ecc3;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;1184,-2128;Inherit;False;Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;65;-832,-3040;Inherit;True;Property;_TextureSample0;Texture Sample 0;15;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;66;-832,-2832;Inherit;True;Property;_TextureSample1;Texture Sample 1;15;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;239;-832,-2624;Inherit;True;Property;_FoamNormal;Foam Normal Map;7;0;Create;False;0;0;0;False;0;False;-1;3cfd62d17e8d12640ab92f82a01da633;3cfd62d17e8d12640ab92f82a01da633;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;80;-480,-2944;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;238;-496,-2512;Inherit;False;62;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;169;-2176,-4224;Inherit;False;1443.003;480.4107;Refraction;9;60;21;20;19;16;18;228;105;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;237;-240,-2640;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-80,-2640;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2144,-3968;Inherit;False;Property;_RefractionPower;Refraction Power;3;0;Create;True;0;0;0;False;0;False;1;2.03;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;228;-1792,-3968;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;-1792,-3840;Inherit;False;81;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1536,-3968;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;16;-1792,-4160;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1408,-4096;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;20;-1280,-4096;Inherit;False;Global;_GrabScreen0;Grab Screen 0;9;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;21;-1088,-4096;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;166;-2176,-4768;Inherit;False;1150.221;510.5022;Color;8;58;375;173;374;376;292;175;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-928,-4096;Inherit;False;Refraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;53;-2144,-4640;Inherit;False;Property;_DepthColor;Depth Color (RGBA);8;0;Create;False;0;0;0;False;0;False;0,0.6810271,0.6886792,1;0.2667319,0.5651833,0.764151,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;175;-1920,-4704;Inherit;False;60;Refraction;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;292;-1920,-4512;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;374;-2144,-4448;Inherit;False;Property;_DepthColor1;Foam Color;10;0;Create;False;0;0;0;False;0;False;0,0.6810271,0.6886792,1;0.8867924,0.8867924,0.8867924,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;376;-1744,-4384;Inherit;False;62;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;173;-1728,-4640;Inherit;False;Overlay;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;375;-1408,-4512;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;164;-2176,-5408;Inherit;False;2014.17;602.8064;Waves;14;49;273;201;191;192;48;193;197;200;195;194;198;196;199;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;235;-2176,-3712;Inherit;False;1443.669;579.541;Caustics;11;47;132;131;129;110;28;160;159;158;27;162;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-1232,-4512;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TransformDirectionNode;273;-608,-5152;Inherit;False;World;Object;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;418;-176,-3632;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;420;-160,-3520;Inherit;False;Property;_Fade;Fade;20;0;Create;True;0;0;0;False;0;False;0;-0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;419;0,-3584;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;414;-16,-3712;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;417;240,-3712;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;195;-1920,-5168;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-1712,-3472;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;160;-1760,-3648;Inherit;False;32;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;27;-1920,-3376;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;-752,-5152;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;-1248,-3520;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;318.383,-4014.74;Inherit;False;Property;_SmoothnessPower;Smoothness Power;19;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;415;384,-3712;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1712,-3568;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;416;-368,-3616;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-2144,-5024;Inherit;False;Property;_WavesHeight;Waves Height;15;0;Create;True;0;0;0;False;3;Space(10);Header(Waves);Space(10);False;25;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;196;-1760,-5248;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;382.383,-4206.74;Inherit;False;81;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-944,-3520;Inherit;False;Caustics;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-1760,-5088;Inherit;False;Property;_WavesScale;Waves Scale;17;0;Create;True;0;0;0;False;0;False;10;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;382.383,-4302.74;Inherit;False;58;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;176;318.383,-4110.739;Inherit;False;Property;_MetallicPower;Metallic Power;18;0;Create;True;0;0;0;False;3;Space(10);Header(Metallic Smoothness);Space(10);False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-1904,-3248;Inherit;False;Property;_CausticsScale;Caustics Scale;13;0;Create;True;0;0;0;False;0;False;0.5;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;132;-1104,-3520;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-2144,-5152;Inherit;False;Property;_WavesSpeed;Waves Speed;16;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;-1024,-5040;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;199;-1344,-5248;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;110;-1472,-3648;Inherit;False;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.WorldPosInputsNode;192;-2144,-5344;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VoronoiNode;129;-1472,-3408;Inherit;False;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.GetLocalVarNode;285;382.8501,-3917.74;Inherit;False;62;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;198;-1504,-5088;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-384,-5152;Inherit;False;WavesHeight;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;194;-1952,-5312;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;200;-1024,-5296;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;162;-2128,-3376;Inherit;False;Property;_CausticsSpeed;Caustics Speed;14;0;Create;True;0;0;0;False;0;False;2;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;726.0051,-4288.199;Float;False;True;-1;6;;0;0;Standard;Custom/WaterFall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;1;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;2;Custom;0.5;True;True;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;50;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;361;0;362;0
WireConnection;32;0;343;0
WireConnection;358;0;361;0
WireConnection;364;0;359;0
WireConnection;364;1;358;0
WireConnection;345;0;224;0
WireConnection;345;1;223;0
WireConnection;363;0;360;0
WireConnection;363;1;361;0
WireConnection;254;0;345;0
WireConnection;254;2;364;0
WireConnection;213;0;345;0
WireConnection;213;2;363;0
WireConnection;84;0;83;0
WireConnection;84;1;213;0
WireConnection;258;0;83;0
WireConnection;258;1;254;0
WireConnection;259;0;84;1
WireConnection;259;1;258;1
WireConnection;370;0;355;1
WireConnection;370;1;259;0
WireConnection;402;0;370;0
WireConnection;402;1;395;0
WireConnection;393;0;402;0
WireConnection;394;0;393;0
WireConnection;408;0;394;0
WireConnection;350;0;78;0
WireConnection;70;0;350;0
WireConnection;401;0;370;0
WireConnection;401;1;408;0
WireConnection;413;0;411;0
WireConnection;338;0;353;0
WireConnection;338;1;70;0
WireConnection;337;0;349;0
WireConnection;337;1;350;0
WireConnection;381;0;401;0
WireConnection;381;1;413;0
WireConnection;386;0;67;0
WireConnection;386;1;354;0
WireConnection;409;0;381;0
WireConnection;72;0;386;0
WireConnection;72;2;337;0
WireConnection;73;0;386;0
WireConnection;73;2;338;0
WireConnection;62;0;409;0
WireConnection;65;0;64;0
WireConnection;65;1;72;0
WireConnection;65;5;11;0
WireConnection;66;0;64;0
WireConnection;66;1;73;0
WireConnection;66;5;11;0
WireConnection;239;1;213;0
WireConnection;239;5;11;0
WireConnection;80;0;65;0
WireConnection;80;1;66;0
WireConnection;237;0;80;0
WireConnection;237;1;239;0
WireConnection;237;2;238;0
WireConnection;81;0;237;0
WireConnection;228;0;17;0
WireConnection;18;0;228;0
WireConnection;18;1;105;0
WireConnection;19;0;16;0
WireConnection;19;1;18;0
WireConnection;20;0;19;0
WireConnection;21;0;20;0
WireConnection;60;0;21;0
WireConnection;292;0;53;4
WireConnection;173;0;175;0
WireConnection;173;1;53;0
WireConnection;173;2;292;0
WireConnection;375;0;173;0
WireConnection;375;1;374;0
WireConnection;375;2;376;0
WireConnection;58;0;375;0
WireConnection;273;0;201;0
WireConnection;418;0;416;4
WireConnection;419;0;418;0
WireConnection;419;1;420;0
WireConnection;417;0;414;0
WireConnection;417;1;419;0
WireConnection;195;0;193;0
WireConnection;195;1;193;0
WireConnection;159;0;158;0
WireConnection;27;0;162;0
WireConnection;201;0;200;0
WireConnection;201;1;191;0
WireConnection;131;0;110;0
WireConnection;131;1;129;0
WireConnection;415;0;417;0
WireConnection;28;0;27;0
WireConnection;196;2;195;0
WireConnection;47;0;132;0
WireConnection;132;0;131;0
WireConnection;191;0;199;0
WireConnection;191;1;48;0
WireConnection;199;0;196;0
WireConnection;199;1;198;0
WireConnection;110;0;160;0
WireConnection;110;1;28;0
WireConnection;110;2;159;0
WireConnection;129;0;160;0
WireConnection;129;1;27;0
WireConnection;129;2;158;0
WireConnection;198;0;197;0
WireConnection;49;0;273;0
WireConnection;194;0;192;1
WireConnection;194;1;192;3
WireConnection;0;0;59;0
WireConnection;0;1;82;0
WireConnection;0;3;176;0
WireConnection;0;4;13;0
ASEEND*/
//CHKSM=412F226368398F78253E7A648EBB05CC3170F9C9