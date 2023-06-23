// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/VegetationLeaves02"
{
	Properties
	{
		[Toggle(_HIDESIDES_ON)] _HideSides("Hide Sides", Float) = 0
		_HidePower("Hide Power", Float) = 2.5
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Header(Main Maps)][Space(10)]_MainColor("Main Color", Color) = (1,1,1,0)
		_Diffuse("Diffuse", 2D) = "white" {}
		[Space(10)][Header(Gradient Parameters)][Space(10)]_GradientColor("Gradient Color", Color) = (1,1,1,0)
		_GradientFalloff("Gradient Falloff", Range( 0 , 2)) = 2
		_GradientPosition("Gradient Position", Range( 0 , 1)) = 0.5
		[Toggle(_INVERTGRADIENT_ON)] _InvertGradient("Invert Gradient", Float) = 0
		[Space(10)][Header(Color Variation)][Space(10)]_ColorVariation("Color Variation", Color) = (1,0,0,0)
		_ColorVariationPower("Color Variation Power", Range( 0 , 1)) = 1
		_ColorVariationNoise("Color Variation Noise", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 0.5
		[Space(10)][Header(Multipliers)][Space(10)]_WindMultiplier("BaseWind Multiplier", Float) = 0
		_MicroWindMultiplier("MicroWind Multiplier", Float) = 1
		[Space(10)][KeywordEnum(R,G,B,A)] _BaseWindChannel("Base Wind Channel", Float) = 2
		[KeywordEnum(R,G,B,A)] _MicroWindChannel("Micro Wind Channel", Float) = 0
		[Space(10)]_WindTrunkPosition("Wind Trunk Position", Float) = 0
		_WindTrunkContrast("Wind Trunk Contrast", Float) = 10
		[Toggle(_WINDDEBUGVIEW_ON)] _WindDebugView("WindDebugView", Float) = 0
		[Toggle(_SEEVERTEXCOLOR_ON)] _SeeVertexColor("See Vertex Color", Float) = 0
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 5.0
		#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
		#pragma shader_feature_local _MICROWINDCHANNEL_R _MICROWINDCHANNEL_G _MICROWINDCHANNEL_B _MICROWINDCHANNEL_A
		#pragma shader_feature _SEEVERTEXCOLOR_ON
		#pragma shader_feature _WINDDEBUGVIEW_ON
		#pragma shader_feature_local _INVERTGRADIENT_ON
		#pragma shader_feature_local _HIDESIDES_ON
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPosition;
			float3 viewDir;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Translucency;
		};

		uniform float WindSpeed;
		uniform float WindPower;
		uniform float WindBurstsSpeed;
		uniform float WindBurstsScale;
		uniform float WindBurstsPower;
		uniform float _WindTrunkContrast;
		uniform float _WindTrunkPosition;
		uniform float _WindMultiplier;
		uniform float MicroFrequency;
		uniform float MicroSpeed;
		uniform float MicroPower;
		uniform float _MicroWindMultiplier;
		uniform float4 _MainColor;
		uniform float4 _GradientColor;
		uniform float _GradientPosition;
		uniform float _GradientFalloff;
		uniform float4 _ColorVariation;
		uniform float _ColorVariationPower;
		uniform sampler2D _ColorVariationNoise;
		uniform float _NoiseScale;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _HidePower;
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


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
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
			float temp_output_102_0 = ( _Time.y * WindSpeed );
			float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
			float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
			simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
			float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
			#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = v.color.r;
			#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = v.color.g;
			#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = v.color.b;
			#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = v.color.a;
			#else
				float staticSwitch285 = v.color.b;
			#endif
			float BaseWindColor288 = staticSwitch285;
			float4 temp_cast_0 = (pow( ( 1.0 - BaseWindColor288 ) , _WindTrunkPosition )).xxxx;
			float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
			float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
			float4 transform254 = mul(unity_WorldToObject,float4( appendResult113 , 0.0 ));
			float4 BaseWind151 = ( transform254 * _WindMultiplier );
			float2 temp_cast_4 = (MicroSpeed).xx;
			float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
			float2 panner175 = ( 1.0 * _Time.y * temp_cast_4 + appendResult174.xy);
			float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
			simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
			float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
			float3 ase_vertexNormal = v.normal.xyz;
			#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = v.color.r;
			#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = v.color.g;
			#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = v.color.b;
			#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = v.color.a;
			#else
				float staticSwitch284 = v.color.r;
			#endif
			float MicroWindColor287 = staticSwitch284;
			float3 MicroWind152 = ( ( ( ( clampResult49 * ase_vertexNormal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
			float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
			v.vertex.xyz += temp_output_115_0.xyz;
			v.vertex.w = 1;
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float3 ase_worldNormal = i.worldNormal;
			#ifdef _INVERTGRADIENT_ON
				float staticSwitch306 = ( 1.0 - ase_worldNormal.y );
			#else
				float staticSwitch306 = ase_worldNormal.y;
			#endif
			float clampResult39 = clamp( ( ( staticSwitch306 + (-2.0 + (_GradientPosition - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) / _GradientFalloff ) , 0.0 , 1.0 );
			float4 lerpResult46 = lerp( _MainColor , _GradientColor , clampResult39);
			float4 blendOpSrc53 = lerpResult46;
			float4 blendOpDest53 = _ColorVariation;
			float4 lerpBlendMode53 = lerp(blendOpDest53,( blendOpDest53/ max( 1.0 - blendOpSrc53, 0.00001 ) ),_ColorVariationPower);
			float3 ase_worldPos = i.worldPos;
			float2 appendResult71 = (float2(ase_worldPos.x , ase_worldPos.z));
			float4 temp_cast_0 = (3.0).xxxx;
			float4 lerpResult58 = lerp( lerpResult46 , ( saturate( lerpBlendMode53 )) , ( _ColorVariationPower * pow( tex2D( _ColorVariationNoise, ( appendResult71 * ( _NoiseScale / 100.0 ) ) ) , temp_cast_0 ) ));
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 tex2DNode56 = tex2D( _Diffuse, uv_Diffuse );
			float4 _Albedo339 = ( lerpResult58 * tex2DNode56 );
			float temp_output_102_0 = ( _Time.y * WindSpeed );
			float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
			float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
			float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
			simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
			float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
			#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = i.vertexColor.r;
			#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = i.vertexColor.g;
			#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = i.vertexColor.b;
			#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = i.vertexColor.a;
			#else
				float staticSwitch285 = i.vertexColor.b;
			#endif
			float BaseWindColor288 = staticSwitch285;
			float4 temp_cast_1 = (pow( ( 1.0 - BaseWindColor288 ) , _WindTrunkPosition )).xxxx;
			float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_1) );
			float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
			float4 transform254 = mul(unity_WorldToObject,float4( appendResult113 , 0.0 ));
			float4 BaseWind151 = ( transform254 * _WindMultiplier );
			float2 temp_cast_5 = (MicroSpeed).xx;
			float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
			float2 panner175 = ( 1.0 * _Time.y * temp_cast_5 + appendResult174.xy);
			float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
			simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
			float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = i.vertexColor.r;
			#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = i.vertexColor.g;
			#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = i.vertexColor.b;
			#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = i.vertexColor.a;
			#else
				float staticSwitch284 = i.vertexColor.r;
			#endif
			float MicroWindColor287 = staticSwitch284;
			float3 MicroWind152 = ( ( ( ( clampResult49 * ase_vertexNormal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
			float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
			#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch194 = temp_output_115_0;
			#else
				float4 staticSwitch194 = _Albedo339;
			#endif
			#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch310 = i.vertexColor;
			#else
				float4 staticSwitch310 = staticSwitch194;
			#endif
			o.Albedo = staticSwitch310.rgb;
			float3 temp_cast_10 = (1.0).xxx;
			o.Translucency = temp_cast_10;
			o.Alpha = 1;
			float _Opacity231 = tex2DNode56.a;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen217 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither217 = Dither8x8Bayer( fmod(clipScreen217.x, 8), fmod(clipScreen217.y, 8) );
			float3 normalizeResult214 = normalize( cross( ddy( ase_worldPos ) , ddx( ase_worldPos ) ) );
			float dotResult200 = dot( i.viewDir , normalizeResult214 );
			float clampResult222 = clamp( ( ( _Opacity231 * ( 1.0 - ( ( 1.0 - abs( dotResult200 ) ) * 2.0 ) ) ) * _HidePower ) , 0.0 , 1.0 );
			dither217 = step( dither217, clampResult222 );
			float OpacityDither205 = dither217;
			#ifdef _HIDESIDES_ON
				float staticSwitch234 = OpacityDither205;
			#else
				float staticSwitch234 = _Opacity231;
			#endif
			clip( staticSwitch234 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustom keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.screenPosition;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.screenPosition = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandardCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18900
947;73;652;634;6665.154;-1514.808;4.072208;True;False
Node;AmplifyShaderEditor.CommentaryNode;156;-6016,2816;Inherit;False;3067.315;862.5801;;22;152;304;62;303;54;289;52;51;49;44;40;36;32;34;176;190;175;174;26;172;305;372;MicroWind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;150;-6016,1792;Inherit;False;3196.337;864.2947;;37;291;151;164;165;254;113;111;112;114;299;104;109;298;106;103;108;296;107;102;297;129;105;295;100;148;101;294;290;149;135;126;139;127;131;125;128;379;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;302;-6656,-1664;Inherit;False;889.3333;538;;7;55;285;288;284;287;292;293;VertexColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;172;-5984,3040;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;174;-5728,3200;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-5760,3328;Float;False;Global;MicroSpeed;MicroSpeed;18;1;[HideInInspector];Create;False;0;0;0;False;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;226;-6016,1152;Inherit;False;2565;479;;18;205;217;222;224;223;215;283;209;216;207;204;200;214;199;213;211;212;210;Dithering;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-5968,2400;Inherit;False;Global;WindBurstsSpeed;Wind Bursts Speed;22;1;[HideInInspector];Create;True;0;0;0;False;0;False;50;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;125;-5968,2208;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VertexColorNode;55;-6608,-1440;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;175;-5536,3264;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;285;-6304,-1440;Inherit;False;Property;_BaseWindChannel;Base Wind Channel;17;0;Create;True;0;0;0;False;1;Space(10);False;0;2;2;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;210;-5968,1456;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;139;-5744,2384;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;131;-5776,2240;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-5760,2496;Inherit;False;Global;WindBurstsScale;Wind Bursts Scale;23;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;190;-5504,3456;Inherit;False;Constant;_Float2;Float 2;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;126;-5536,2304;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;288;-6016,-1440;Inherit;False;BaseWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;372;-5312,3360;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DdyOpNode;211;-5776,1392;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DdxOpNode;212;-5776,1488;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;359;-6656,-640;Inherit;False;1565;669;;12;43;38;39;37;46;35;33;306;29;155;28;27;Color Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;135;-5488,2480;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;176;-5152,3360;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;379;-5248,2304;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;290;-4544,2400;Inherit;False;288;BaseWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;213;-5648,1424;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-5184,2576;Inherit;False;Global;WindBurstsPower;Wind Bursts Power;24;1;[HideInInspector];Create;True;0;0;0;False;0;False;10;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;27;-6624,-512;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;100;-5056,1968;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-4832,2400;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;199;-5568,1200;Inherit;True;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;101;-5056,2096;Inherit;False;Global;WindSpeed;Wind Speed;20;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;214;-5488,1424;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-5136,2880;Float;False;Global;MicroFrequency;MicroFrequency;19;1;[HideInInspector];Create;False;0;0;0;False;0;False;5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;294;-4336,2480;Inherit;False;Property;_WindTrunkPosition;Wind Trunk Position;19;0;Create;True;0;0;0;False;1;Space(10);False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-6400,-384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-6528,-256;Float;False;Property;_GradientPosition;Gradient Position;9;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-4656,1984;Inherit;False;Global;WindPower;Wind Power;21;1;[HideInInspector];Create;True;0;0;0;False;0;False;0.01;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-5072,3040;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;360;-6016,128;Inherit;False;2322;838;Color blend controlled by world-space noise;17;41;71;72;42;162;161;45;53;50;58;56;231;339;63;362;48;367;Color Variation;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;295;-4320,2368;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;200;-5312,1312;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-4544,2224;Inherit;False;Constant;_Float8;Float 8;18;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;41;-5968,528;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-4912,2944;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-4800,1968;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-4448,1968;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;155;-6192,-256;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-5968,720;Inherit;False;Property;_NoiseScale;Noise Scale;14;0;Create;True;0;0;0;False;0;False;0.5;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;296;-4192,2560;Inherit;False;Property;_WindTrunkContrast;Wind Trunk Contrast;20;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;297;-4128,2432;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;306;-6240,-464;Inherit;False;Property;_InvertGradient;Invert Gradient;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;106;-4544,2096;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;40;-4688,2944;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;204;-5184,1312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-6016,-64;Float;False;Property;_GradientFalloff;Gradient Falloff;8;0;Create;True;0;0;0;False;0;False;2;0.9;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;103;-4544,1840;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;298;-3968,2464;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-4288,2160;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-5968,-368;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;-5776,560;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;72;-5776,720;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;362;-5632,640;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;56;-4560,528;Inherit;True;Property;_Diffuse;Diffuse;4;0;Create;True;0;0;0;False;0;False;-1;None;996b33df3b9555a41a87c53cf9f606f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;37;-5712,-192;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;49;-4496,2944;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;-1,-1,-1;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;207;-5056,1312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-4032,2096;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-4288,1840;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;284;-6304,-1600;Inherit;False;Property;_MicroWindChannel;Micro Wind Channel;18;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;44;-4816,3264;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;299;-3776,2464;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-3776,1840;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;287;-6016,-1600;Inherit;False;MicroWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;39;-5584,-192;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;-5712,-576;Float;False;Property;_MainColor;Main Color;3;0;Create;True;0;0;0;False;2;Header(Main Maps);Space(10);False;1,1,1,0;0.4101034,0.7264151,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;231;-4176,656;Inherit;False;_Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-4192,3072;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;38;-5712,-384;Float;False;Property;_GradientColor;Gradient Color;7;0;Create;True;0;0;0;False;3;Space(10);Header(Gradient Parameters);Space(10);False;1,1,1,0;0.2224449,0.490566,0.01619794,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-3776,2096;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-3776,1968;Inherit;False;Constant;_Float9;Float 9;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;367;-5456,608;Inherit;True;Property;_ColorVariationNoise;Color Variation Noise;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-4240,3200;Float;False;Global;MicroPower;MicroPower;20;0;Create;False;0;0;0;False;0;False;0.05;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-5328,848;Inherit;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-4880,1280;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;-5360,-320;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;48;-5328,224;Inherit;False;Property;_ColorVariation;Color Variation;11;0;Create;True;0;0;0;False;3;Space(10);Header(Color Variation);Space(10);False;1,0,0,0;0.1103595,0.2924528,0.2567374,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;161;-5120,784;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;283;-4752,1200;Inherit;False;231;_Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;113;-3600,1920;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-5328,400;Inherit;False;Property;_ColorVariationPower;Color Variation Power;12;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;-4256,3392;Inherit;False;287;MicroWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-4032,3104;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;209;-4736,1296;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;254;-3344,1920;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-4944,704;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-3776,3232;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;303;-3760,3488;Inherit;False;Property;_MicroWindMultiplier;MicroWind Multiplier;16;0;Create;True;0;0;0;False;3;;;;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;53;-4944,192;Inherit;True;ColorDodge;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-4576,1264;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-3376,2208;Inherit;False;Property;_WindMultiplier;BaseWind Multiplier;15;0;Create;False;0;0;0;False;3;Space(10);Header(Multipliers);Space(10);False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-4608,1472;Inherit;False;Property;_HidePower;Hide Power;1;0;Create;True;0;0;0;False;0;False;2.5;4.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-4560,176;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;304;-3472,3296;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;-4384,1328;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-3136,2096;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-3088,1920;Inherit;False;BaseWind;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-4176,400;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-3312,3296;Inherit;False;MicroWind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;222;-4176,1328;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;-4736,-640;Inherit;False;151;BaseWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;339;-3920,400;Inherit;False;_Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;217;-3968,1344;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;154;-4736,-528;Inherit;False;152;MicroWind;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-4544,-608;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;-4480,-1280;Inherit;False;339;_Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-3680,1344;Inherit;False;OpacityDither;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;309;-4224,-1168;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;232;-4288,-800;Inherit;False;231;_Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;194;-4288,-1280;Inherit;False;Property;_WindDebugView;WindDebugView;22;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;363;-6656,-1024;Inherit;False;866;280;;3;57;65;342;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;233;-4288,-704;Inherit;False;205;OpacityDither;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-6608,-976;Float;False;Property;_NormalPower;Normal Power;6;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;65;-6320,-976;Inherit;True;Property;_Normal;Normal;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;293;-6032,-1280;Inherit;False;DepositLayerColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;310;-4032,-1232;Inherit;False;Property;_SeeVertexColor;See Vertex Color;23;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;342;-6016,-976;Inherit;False;_Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;305;-4512,3392;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;354;-4096,-896;Inherit;False;Constant;_Transluency;Transluency;25;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;234;-4096,-768;Inherit;False;Property;_HideSides;Hide Sides;0;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;292;-6304,-1280;Inherit;False;Property;_DepositLayerChannel;DepositLayer Channel;21;0;Create;True;0;0;0;False;0;False;0;2;2;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;-4192,1968;Inherit;False;288;BaseWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-3712,-896;Float;False;True;-1;7;;0;0;Standard;Custom/VegetationLeaves02;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;24;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;174;0;172;1
WireConnection;174;1;172;3
WireConnection;174;2;172;2
WireConnection;175;0;174;0
WireConnection;175;2;26;0
WireConnection;285;1;55;1
WireConnection;285;0;55;2
WireConnection;285;2;55;3
WireConnection;285;3;55;4
WireConnection;139;0;128;0
WireConnection;139;1;128;0
WireConnection;131;0;125;1
WireConnection;131;1;125;3
WireConnection;126;0;131;0
WireConnection;126;2;139;0
WireConnection;288;0;285;0
WireConnection;372;0;175;0
WireConnection;372;1;190;0
WireConnection;211;0;210;0
WireConnection;212;0;210;0
WireConnection;135;0;127;0
WireConnection;176;0;372;0
WireConnection;379;0;126;0
WireConnection;379;1;135;0
WireConnection;213;0;211;0
WireConnection;213;1;212;0
WireConnection;148;0;379;0
WireConnection;148;1;149;0
WireConnection;214;0;213;0
WireConnection;29;0;27;2
WireConnection;32;0;172;0
WireConnection;32;1;176;0
WireConnection;295;0;290;0
WireConnection;200;0;199;0
WireConnection;200;1;214;0
WireConnection;36;0;34;0
WireConnection;36;1;32;0
WireConnection;102;0;100;0
WireConnection;102;1;101;0
WireConnection;129;0;105;0
WireConnection;129;1;148;0
WireConnection;155;0;28;0
WireConnection;297;0;295;0
WireConnection;297;1;294;0
WireConnection;306;1;27;2
WireConnection;306;0;29;0
WireConnection;106;0;102;0
WireConnection;40;0;36;0
WireConnection;204;0;200;0
WireConnection;103;0;102;0
WireConnection;298;1;297;0
WireConnection;298;0;296;0
WireConnection;108;0;129;0
WireConnection;108;1;107;0
WireConnection;33;0;306;0
WireConnection;33;1;155;0
WireConnection;71;0;41;1
WireConnection;71;1;41;3
WireConnection;72;0;42;0
WireConnection;362;0;71;0
WireConnection;362;1;72;0
WireConnection;37;0;33;0
WireConnection;37;1;35;0
WireConnection;49;0;40;0
WireConnection;207;0;204;0
WireConnection;109;0;106;0
WireConnection;109;1;108;0
WireConnection;104;0;103;0
WireConnection;104;1;129;0
WireConnection;284;1;55;1
WireConnection;284;0;55;2
WireConnection;284;2;55;3
WireConnection;284;3;55;4
WireConnection;299;0;298;0
WireConnection;112;0;104;0
WireConnection;112;1;299;0
WireConnection;287;0;284;0
WireConnection;39;0;37;0
WireConnection;231;0;56;4
WireConnection;52;0;49;0
WireConnection;52;1;44;0
WireConnection;111;0;109;0
WireConnection;111;1;299;0
WireConnection;367;1;362;0
WireConnection;216;0;207;0
WireConnection;46;0;43;0
WireConnection;46;1;38;0
WireConnection;46;2;39;0
WireConnection;161;0;367;0
WireConnection;161;1;162;0
WireConnection;113;0;112;0
WireConnection;113;1;114;0
WireConnection;113;2;111;0
WireConnection;54;0;52;0
WireConnection;54;1;51;0
WireConnection;209;0;216;0
WireConnection;254;0;113;0
WireConnection;50;0;45;0
WireConnection;50;1;161;0
WireConnection;62;0;54;0
WireConnection;62;1;289;0
WireConnection;53;0;46;0
WireConnection;53;1;48;0
WireConnection;53;2;45;0
WireConnection;215;0;283;0
WireConnection;215;1;209;0
WireConnection;58;0;46;0
WireConnection;58;1;53;0
WireConnection;58;2;50;0
WireConnection;304;0;62;0
WireConnection;304;1;303;0
WireConnection;224;0;215;0
WireConnection;224;1;223;0
WireConnection;164;0;254;0
WireConnection;164;1;165;0
WireConnection;151;0;164;0
WireConnection;63;0;58;0
WireConnection;63;1;56;0
WireConnection;152;0;304;0
WireConnection;222;0;224;0
WireConnection;339;0;63;0
WireConnection;217;0;222;0
WireConnection;115;0;153;0
WireConnection;115;1;154;0
WireConnection;205;0;217;0
WireConnection;194;1;346;0
WireConnection;194;0;115;0
WireConnection;65;5;57;0
WireConnection;293;0;292;0
WireConnection;310;1;194;0
WireConnection;310;0;309;0
WireConnection;342;0;65;0
WireConnection;305;0;44;0
WireConnection;234;1;232;0
WireConnection;234;0;233;0
WireConnection;292;1;55;1
WireConnection;292;0;55;2
WireConnection;292;2;55;3
WireConnection;292;3;55;4
WireConnection;0;0;310;0
WireConnection;0;7;354;0
WireConnection;0;10;234;0
WireConnection;0;11;115;0
ASEEND*/
//CHKSM=87ACD68835A59AF4946421F7DF3CD0653FEBDB55