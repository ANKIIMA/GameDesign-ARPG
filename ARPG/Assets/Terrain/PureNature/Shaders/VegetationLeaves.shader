// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VegetationLeaves"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin][Toggle(_HIDESIDES_ON)] _HideSides("Hide Sides", Float) = 0
		_HidePower("Hide Power", Float) = 2.5
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
		[ASEEnd][Space(30)]_Scattering("Scattering", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		_TransStrength( "Strength", Range( 0, 50 ) ) = 1
		_TransNormal( "Normal Distortion", Range( 0, 1 ) ) = 0.5
		_TransScattering( "Scattering", Range( 1, 50 ) ) = 2
		_TransDirect( "Direct", Range( 0, 1 ) ) = 0.9
		_TransAmbient( "Ambient", Range( 0, 1 ) ) = 0.1
		_TransShadow( "Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="AlphaTest" }
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 3.0

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSLUCENCY_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature_local _MICROWINDCHANNEL_R _MICROWINDCHANNEL_G _MICROWINDCHANNEL_B _MICROWINDCHANNEL_A
			#pragma shader_feature _SEEVERTEXCOLOR_ON
			#pragma shader_feature _WINDDEBUGVIEW_ON
			#pragma shader_feature_local _INVERTGRADIENT_ON
			#pragma shader_feature_local _HIDESIDES_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_color : COLOR;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _GradientColor;
			float4 _ColorVariation;
			float4 _Diffuse_ST;
			float _WindTrunkContrast;
			float _WindTrunkPosition;
			float _WindMultiplier;
			float _MicroWindMultiplier;
			float _GradientPosition;
			float _GradientFalloff;
			float _ColorVariationPower;
			float _NoiseScale;
			float _HidePower;
			float _Scattering;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float WindSpeed;
			float WindPower;
			float WindBurstsSpeed;
			float WindBurstsScale;
			float WindBurstsPower;
			float MicroFrequency;
			float MicroSpeed;
			float MicroPower;
			sampler2D _ColorVariationNoise;
			sampler2D _Diffuse;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = v.ase_color.a;
				#else
				float staticSwitch285 = v.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_4 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_4 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = v.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = v.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = v.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = v.ase_color.a;
				#else
				float staticSwitch284 = v.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * v.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				
				o.ase_texcoord7.xy = v.texcoord.xy;
				o.ase_color = v.ase_color;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = temp_output_115_0.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				#ifdef _INVERTGRADIENT_ON
				float staticSwitch306 = ( 1.0 - WorldNormal.y );
				#else
				float staticSwitch306 = WorldNormal.y;
				#endif
				float clampResult39 = clamp( ( ( staticSwitch306 + (-2.0 + (_GradientPosition - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) / _GradientFalloff ) , 0.0 , 1.0 );
				float4 lerpResult46 = lerp( _MainColor , _GradientColor , clampResult39);
				float4 blendOpSrc53 = lerpResult46;
				float4 blendOpDest53 = _ColorVariation;
				float4 lerpBlendMode53 = lerp(blendOpDest53,( blendOpDest53/ max( 1.0 - blendOpSrc53, 0.00001 ) ),_ColorVariationPower);
				float2 appendResult71 = (float2(WorldPosition.x , WorldPosition.z));
				float4 saferPower161 = max( tex2D( _ColorVariationNoise, ( appendResult71 * ( _NoiseScale / 100.0 ) ) ) , 0.0001 );
				float4 temp_cast_0 = (3.0).xxxx;
				float4 lerpResult58 = lerp( lerpResult46 , ( saturate( lerpBlendMode53 )) , ( _ColorVariationPower * pow( saferPower161 , temp_cast_0 ) ));
				float2 uv_Diffuse = IN.ase_texcoord7.xy * _Diffuse_ST.xy + _Diffuse_ST.zw;
				float4 tex2DNode56 = tex2D( _Diffuse, uv_Diffuse );
				float4 _Albedo339 = ( lerpResult58 * tex2DNode56 );
				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float2 appendResult131 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = IN.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = IN.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = IN.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = IN.ase_color.a;
				#else
				float staticSwitch285 = IN.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_1 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_1) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_5 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(WorldPosition.x , WorldPosition.z , WorldPosition.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_5 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( WorldPosition + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = IN.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = IN.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = IN.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = IN.ase_color.a;
				#else
				float staticSwitch284 = IN.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * IN.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch194 = temp_output_115_0;
				#else
				float4 staticSwitch194 = _Albedo339;
				#endif
				#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch310 = IN.ase_color;
				#else
				float4 staticSwitch310 = staticSwitch194;
				#endif
				
				float _Opacity231 = tex2DNode56.a;
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 clipScreen217 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither217 = Dither8x8Bayer( fmod(clipScreen217.x, 8), fmod(clipScreen217.y, 8) );
				float3 normalizeResult214 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float dotResult200 = dot( WorldViewDirection , normalizeResult214 );
				float clampResult222 = clamp( ( ( _Opacity231 * ( 1.0 - ( ( 1.0 - abs( dotResult200 ) ) * 2.0 ) ) ) * _HidePower ) , 0.0 , 1.0 );
				dither217 = step( dither217, clampResult222 );
				float OpacityDither205 = dither217;
				#ifdef _HIDESIDES_ON
				float staticSwitch234 = OpacityDither205;
				#else
				float staticSwitch234 = _Opacity231;
				#endif
				
				float3 temp_cast_10 = (_Scattering).xxx;
				
				float3 Albedo = staticSwitch310.rgb;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = 0;
				float3 Specular = 0.5;
				float Metallic = 0;
				float Smoothness = 0.0;
				float Occlusion = 1;
				float Alpha = staticSwitch234;
				float AlphaClipThreshold = 0.25;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = temp_cast_10;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSLUCENCY_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature_local _MICROWINDCHANNEL_R _MICROWINDCHANNEL_G _MICROWINDCHANNEL_B _MICROWINDCHANNEL_A
			#pragma shader_feature_local _HIDESIDES_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _GradientColor;
			float4 _ColorVariation;
			float4 _Diffuse_ST;
			float _WindTrunkContrast;
			float _WindTrunkPosition;
			float _WindMultiplier;
			float _MicroWindMultiplier;
			float _GradientPosition;
			float _GradientFalloff;
			float _ColorVariationPower;
			float _NoiseScale;
			float _HidePower;
			float _Scattering;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float WindSpeed;
			float WindPower;
			float WindBurstsSpeed;
			float WindBurstsScale;
			float WindBurstsPower;
			float MicroFrequency;
			float MicroSpeed;
			float MicroPower;
			sampler2D _Diffuse;


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
			

			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = v.ase_color.a;
				#else
				float staticSwitch285 = v.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_4 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_4 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = v.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = v.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = v.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = v.ase_color.a;
				#else
				float staticSwitch284 = v.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * v.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = temp_output_115_0.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Diffuse = IN.ase_texcoord2.xy * _Diffuse_ST.xy + _Diffuse_ST.zw;
				float4 tex2DNode56 = tex2D( _Diffuse, uv_Diffuse );
				float _Opacity231 = tex2DNode56.a;
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 clipScreen217 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither217 = Dither8x8Bayer( fmod(clipScreen217.x, 8), fmod(clipScreen217.y, 8) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult214 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float dotResult200 = dot( ase_worldViewDir , normalizeResult214 );
				float clampResult222 = clamp( ( ( _Opacity231 * ( 1.0 - ( ( 1.0 - abs( dotResult200 ) ) * 2.0 ) ) ) * _HidePower ) , 0.0 , 1.0 );
				dither217 = step( dither217, clampResult222 );
				float OpacityDither205 = dither217;
				#ifdef _HIDESIDES_ON
				float staticSwitch234 = OpacityDither205;
				#else
				float staticSwitch234 = _Opacity231;
				#endif
				
				float Alpha = staticSwitch234;
				float AlphaClipThreshold = 0.25;
				float AlphaClipThresholdShadow = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSLUCENCY_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature_local _MICROWINDCHANNEL_R _MICROWINDCHANNEL_G _MICROWINDCHANNEL_B _MICROWINDCHANNEL_A
			#pragma shader_feature_local _HIDESIDES_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _GradientColor;
			float4 _ColorVariation;
			float4 _Diffuse_ST;
			float _WindTrunkContrast;
			float _WindTrunkPosition;
			float _WindMultiplier;
			float _MicroWindMultiplier;
			float _GradientPosition;
			float _GradientFalloff;
			float _ColorVariationPower;
			float _NoiseScale;
			float _HidePower;
			float _Scattering;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float WindSpeed;
			float WindPower;
			float WindBurstsSpeed;
			float WindBurstsScale;
			float WindBurstsPower;
			float MicroFrequency;
			float MicroSpeed;
			float MicroPower;
			sampler2D _Diffuse;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = v.ase_color.a;
				#else
				float staticSwitch285 = v.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_4 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_4 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = v.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = v.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = v.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = v.ase_color.a;
				#else
				float staticSwitch284 = v.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * v.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = temp_output_115_0.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Diffuse = IN.ase_texcoord2.xy * _Diffuse_ST.xy + _Diffuse_ST.zw;
				float4 tex2DNode56 = tex2D( _Diffuse, uv_Diffuse );
				float _Opacity231 = tex2DNode56.a;
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 clipScreen217 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither217 = Dither8x8Bayer( fmod(clipScreen217.x, 8), fmod(clipScreen217.y, 8) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult214 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float dotResult200 = dot( ase_worldViewDir , normalizeResult214 );
				float clampResult222 = clamp( ( ( _Opacity231 * ( 1.0 - ( ( 1.0 - abs( dotResult200 ) ) * 2.0 ) ) ) * _HidePower ) , 0.0 , 1.0 );
				dither217 = step( dither217, clampResult222 );
				float OpacityDither205 = dither217;
				#ifdef _HIDESIDES_ON
				float staticSwitch234 = OpacityDither205;
				#else
				float staticSwitch234 = _Opacity231;
				#endif
				
				float Alpha = staticSwitch234;
				float AlphaClipThreshold = 0.25;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSLUCENCY_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature_local _MICROWINDCHANNEL_R _MICROWINDCHANNEL_G _MICROWINDCHANNEL_B _MICROWINDCHANNEL_A
			#pragma shader_feature _SEEVERTEXCOLOR_ON
			#pragma shader_feature _WINDDEBUGVIEW_ON
			#pragma shader_feature_local _INVERTGRADIENT_ON
			#pragma shader_feature_local _HIDESIDES_ON


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _GradientColor;
			float4 _ColorVariation;
			float4 _Diffuse_ST;
			float _WindTrunkContrast;
			float _WindTrunkPosition;
			float _WindMultiplier;
			float _MicroWindMultiplier;
			float _GradientPosition;
			float _GradientFalloff;
			float _ColorVariationPower;
			float _NoiseScale;
			float _HidePower;
			float _Scattering;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float WindSpeed;
			float WindPower;
			float WindBurstsSpeed;
			float WindBurstsScale;
			float WindBurstsPower;
			float MicroFrequency;
			float MicroSpeed;
			float MicroPower;
			sampler2D _ColorVariationNoise;
			sampler2D _Diffuse;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = v.ase_color.a;
				#else
				float staticSwitch285 = v.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_4 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_4 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = v.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = v.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = v.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = v.ase_color.a;
				#else
				float staticSwitch284 = v.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * v.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = temp_output_115_0.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
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
				float2 appendResult71 = (float2(WorldPosition.x , WorldPosition.z));
				float4 saferPower161 = max( tex2D( _ColorVariationNoise, ( appendResult71 * ( _NoiseScale / 100.0 ) ) ) , 0.0001 );
				float4 temp_cast_0 = (3.0).xxxx;
				float4 lerpResult58 = lerp( lerpResult46 , ( saturate( lerpBlendMode53 )) , ( _ColorVariationPower * pow( saferPower161 , temp_cast_0 ) ));
				float2 uv_Diffuse = IN.ase_texcoord3.xy * _Diffuse_ST.xy + _Diffuse_ST.zw;
				float4 tex2DNode56 = tex2D( _Diffuse, uv_Diffuse );
				float4 _Albedo339 = ( lerpResult58 * tex2DNode56 );
				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float2 appendResult131 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = IN.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = IN.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = IN.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = IN.ase_color.a;
				#else
				float staticSwitch285 = IN.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_1 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_1) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_5 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(WorldPosition.x , WorldPosition.z , WorldPosition.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_5 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( WorldPosition + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = IN.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = IN.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = IN.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = IN.ase_color.a;
				#else
				float staticSwitch284 = IN.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * IN.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch194 = temp_output_115_0;
				#else
				float4 staticSwitch194 = _Albedo339;
				#endif
				#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch310 = IN.ase_color;
				#else
				float4 staticSwitch310 = staticSwitch194;
				#endif
				
				float _Opacity231 = tex2DNode56.a;
				float4 screenPos = IN.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 clipScreen217 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither217 = Dither8x8Bayer( fmod(clipScreen217.x, 8), fmod(clipScreen217.y, 8) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult214 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float dotResult200 = dot( ase_worldViewDir , normalizeResult214 );
				float clampResult222 = clamp( ( ( _Opacity231 * ( 1.0 - ( ( 1.0 - abs( dotResult200 ) ) * 2.0 ) ) ) * _HidePower ) , 0.0 , 1.0 );
				dither217 = step( dither217, clampResult222 );
				float OpacityDither205 = dither217;
				#ifdef _HIDESIDES_ON
				float staticSwitch234 = OpacityDither205;
				#else
				float staticSwitch234 = _Opacity231;
				#endif
				
				
				float3 Albedo = staticSwitch310.rgb;
				float3 Emission = 0;
				float Alpha = staticSwitch234;
				float AlphaClipThreshold = 0.25;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSLUCENCY_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature_local _MICROWINDCHANNEL_R _MICROWINDCHANNEL_G _MICROWINDCHANNEL_B _MICROWINDCHANNEL_A
			#pragma shader_feature _SEEVERTEXCOLOR_ON
			#pragma shader_feature _WINDDEBUGVIEW_ON
			#pragma shader_feature_local _INVERTGRADIENT_ON
			#pragma shader_feature_local _HIDESIDES_ON


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			float4 _GradientColor;
			float4 _ColorVariation;
			float4 _Diffuse_ST;
			float _WindTrunkContrast;
			float _WindTrunkPosition;
			float _WindMultiplier;
			float _MicroWindMultiplier;
			float _GradientPosition;
			float _GradientFalloff;
			float _ColorVariationPower;
			float _NoiseScale;
			float _HidePower;
			float _Scattering;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float WindSpeed;
			float WindPower;
			float WindBurstsSpeed;
			float WindBurstsScale;
			float WindBurstsPower;
			float MicroFrequency;
			float MicroSpeed;
			float MicroPower;
			sampler2D _ColorVariationNoise;
			sampler2D _Diffuse;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult131 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = v.ase_color.a;
				#else
				float staticSwitch285 = v.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_4 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(ase_worldPos.x , ase_worldPos.z , ase_worldPos.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_4 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( ase_worldPos + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = v.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = v.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = v.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = v.ase_color.a;
				#else
				float staticSwitch284 = v.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * v.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = temp_output_115_0.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
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
				float2 appendResult71 = (float2(WorldPosition.x , WorldPosition.z));
				float4 saferPower161 = max( tex2D( _ColorVariationNoise, ( appendResult71 * ( _NoiseScale / 100.0 ) ) ) , 0.0001 );
				float4 temp_cast_0 = (3.0).xxxx;
				float4 lerpResult58 = lerp( lerpResult46 , ( saturate( lerpBlendMode53 )) , ( _ColorVariationPower * pow( saferPower161 , temp_cast_0 ) ));
				float2 uv_Diffuse = IN.ase_texcoord3.xy * _Diffuse_ST.xy + _Diffuse_ST.zw;
				float4 tex2DNode56 = tex2D( _Diffuse, uv_Diffuse );
				float4 _Albedo339 = ( lerpResult58 * tex2DNode56 );
				float temp_output_102_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult139 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float2 appendResult131 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner126 = ( 1.0 * _Time.y * appendResult139 + appendResult131);
				float simplePerlin2D379 = snoise( panner126*( WindBurstsScale / 10.0 ) );
				simplePerlin2D379 = simplePerlin2D379*0.5 + 0.5;
				float temp_output_129_0 = ( WindPower * ( simplePerlin2D379 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch285 = IN.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch285 = IN.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch285 = IN.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch285 = IN.ase_color.a;
				#else
				float staticSwitch285 = IN.ase_color.b;
				#endif
				float BaseWindColor288 = staticSwitch285;
				float saferPower297 = max( ( 1.0 - BaseWindColor288 ) , 0.0001 );
				float4 temp_cast_1 = (pow( saferPower297 , _WindTrunkPosition )).xxxx;
				float4 temp_output_299_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_1) );
				float3 appendResult113 = (float3(( ( sin( temp_output_102_0 ) * temp_output_129_0 ) * temp_output_299_0 ).r , 0.0 , ( ( cos( temp_output_102_0 ) * ( temp_output_129_0 * 0.5 ) ) * temp_output_299_0 ).r));
				float4 transform254 = mul(GetWorldToObjectMatrix(),float4( appendResult113 , 0.0 ));
				float4 BaseWind151 = ( transform254 * _WindMultiplier );
				float2 temp_cast_5 = (MicroSpeed).xx;
				float3 appendResult174 = (float3(WorldPosition.x , WorldPosition.z , WorldPosition.y));
				float2 panner175 = ( 1.0 * _Time.y * temp_cast_5 + appendResult174.xy);
				float simplePerlin2D176 = snoise( ( panner175 * 1.0 ) );
				simplePerlin2D176 = simplePerlin2D176*0.5 + 0.5;
				float3 clampResult49 = clamp( sin( ( MicroFrequency * ( WorldPosition + simplePerlin2D176 ) ) ) , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
				#if defined(_MICROWINDCHANNEL_R)
				float staticSwitch284 = IN.ase_color.r;
				#elif defined(_MICROWINDCHANNEL_G)
				float staticSwitch284 = IN.ase_color.g;
				#elif defined(_MICROWINDCHANNEL_B)
				float staticSwitch284 = IN.ase_color.b;
				#elif defined(_MICROWINDCHANNEL_A)
				float staticSwitch284 = IN.ase_color.a;
				#else
				float staticSwitch284 = IN.ase_color.r;
				#endif
				float MicroWindColor287 = staticSwitch284;
				float3 MicroWind152 = ( ( ( ( clampResult49 * IN.ase_normal ) * MicroPower ) * MicroWindColor287 ) * _MicroWindMultiplier );
				float4 temp_output_115_0 = ( BaseWind151 + float4( MicroWind152 , 0.0 ) );
				#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch194 = temp_output_115_0;
				#else
				float4 staticSwitch194 = _Albedo339;
				#endif
				#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch310 = IN.ase_color;
				#else
				float4 staticSwitch310 = staticSwitch194;
				#endif
				
				float _Opacity231 = tex2DNode56.a;
				float4 screenPos = IN.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 clipScreen217 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither217 = Dither8x8Bayer( fmod(clipScreen217.x, 8), fmod(clipScreen217.y, 8) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult214 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float dotResult200 = dot( ase_worldViewDir , normalizeResult214 );
				float clampResult222 = clamp( ( ( _Opacity231 * ( 1.0 - ( ( 1.0 - abs( dotResult200 ) ) * 2.0 ) ) ) * _HidePower ) , 0.0 , 1.0 );
				dither217 = step( dither217, clampResult222 );
				float OpacityDither205 = dither217;
				#ifdef _HIDESIDES_ON
				float staticSwitch234 = OpacityDither205;
				#else
				float staticSwitch234 = _Opacity231;
				#endif
				
				
				float3 Albedo = staticSwitch310.rgb;
				float Alpha = staticSwitch234;
				float AlphaClipThreshold = 0.25;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	/*ase_lod*/
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18900
884;73;480;641;4416.158;891.7382;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;156;-6016,2816;Inherit;False;3072.1;799.3823;;22;34;32;305;152;304;303;62;289;54;52;51;44;49;40;36;176;372;190;175;26;174;172;MicroWind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;150;-6016,1792;Inherit;False;3196.337;864.2947;;37;291;151;164;165;254;113;111;112;114;299;104;109;298;106;103;108;296;107;102;297;129;105;295;100;148;101;294;290;149;135;126;139;127;131;125;128;379;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;302;-6656,-1664;Inherit;False;889.3333;538;;7;55;285;288;284;287;292;293;VertexColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;172;-5984,3040;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VertexColorNode;55;-6608,-1440;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;125;-5968,2208;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;226;-6016,1152;Inherit;False;2565;479;;18;205;217;222;224;223;215;283;209;216;207;204;200;214;199;213;211;212;210;Dithering;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-5760,3328;Float;False;Global;MicroSpeed;MicroSpeed;18;1;[HideInInspector];Create;False;0;0;0;False;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;174;-5728,3200;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-5968,2400;Inherit;False;Global;WindBurstsSpeed;Wind Bursts Speed;22;1;[HideInInspector];Create;True;0;0;0;False;0;False;50;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;210;-5968,1456;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;175;-5536,3264;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;285;-6304,-1440;Inherit;False;Property;_BaseWindChannel;Base Wind Channel;16;0;Create;True;0;0;0;False;1;Space(10);False;0;2;0;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;190;-5504,3456;Inherit;False;Constant;_Float2;Float 2;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;139;-5744,2384;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;131;-5776,2240;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-5760,2496;Inherit;False;Global;WindBurstsScale;Wind Bursts Scale;23;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;372;-5312,3360;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;288;-6016,-1440;Inherit;False;BaseWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DdxOpNode;212;-5776,1488;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;126;-5536,2304;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;135;-5488,2480;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;359;-6656,-640;Inherit;False;1565;669;;12;43;38;39;37;46;35;33;306;29;155;28;27;Color Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.DdyOpNode;211;-5776,1392;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CrossProductOpNode;213;-5648,1424;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;290;-4544,2400;Inherit;False;288;BaseWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;176;-5152,3360;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;379;-5248,2304;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-5184,2576;Inherit;False;Global;WindBurstsPower;Wind Bursts Power;24;1;[HideInInspector];Create;True;0;0;0;False;0;False;10;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;27;-6624,-512;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;105;-4656,1984;Inherit;False;Global;WindPower;Wind Power;21;1;[HideInInspector];Create;True;0;0;0;False;0;False;0.01;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-6400,-384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-4928,3040;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-5056,2096;Inherit;False;Global;WindSpeed;Wind Speed;20;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;199;-5568,1200;Inherit;True;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;295;-4320,2368;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;360;-6016,128;Inherit;False;2322;838;Color blend controlled by world-space noise;17;41;71;72;42;162;161;45;53;50;58;56;231;339;63;362;48;367;Color Variation;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-6528,-256;Float;False;Property;_GradientPosition;Gradient Position;8;0;Create;True;0;0;0;False;0;False;0.5;0.655;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;100;-5056,1968;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;214;-5488,1424;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-4992,2880;Float;False;Global;MicroFrequency;MicroFrequency;19;1;[HideInInspector];Create;False;0;0;0;False;0;False;5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;294;-4336,2480;Inherit;False;Property;_WindTrunkPosition;Wind Trunk Position;18;0;Create;True;0;0;0;False;1;Space(10);False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-4832,2400;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-4800,1968;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;200;-5312,1312;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-4544,2224;Inherit;False;Constant;_Float8;Float 8;18;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-5968,720;Inherit;False;Property;_NoiseScale;Noise Scale;13;0;Create;True;0;0;0;False;0;False;0.5;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;155;-6192,-256;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-4768,2944;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;296;-4192,2560;Inherit;False;Property;_WindTrunkContrast;Wind Trunk Contrast;19;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-4448,1968;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;306;-6240,-464;Inherit;False;Property;_InvertGradient;Invert Gradient;9;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;41;-5968,528;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;297;-4128,2432;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;40;-4544,2944;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-5968,-368;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-4288,2160;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;298;-3968,2464;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;103;-4544,1840;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-6016,-64;Float;False;Property;_GradientFalloff;Gradient Falloff;7;0;Create;True;0;0;0;False;0;False;2;0.58;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;106;-4544,2096;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;-5776,560;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;204;-5184,1312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;72;-5776,720;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-4032,2096;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;44;-4672,3264;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;207;-5056,1312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;49;-4352,2944;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;-1,-1,-1;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;37;-5712,-192;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;284;-6304,-1600;Inherit;False;Property;_MicroWindChannel;Micro Wind Channel;17;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-4560,528;Inherit;True;Property;_Diffuse;Diffuse;3;0;Create;True;0;0;0;False;0;False;-1;None;2b5768e7bad8aa04f86fe1c422f08194;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-4288,1840;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;362;-5632,640;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;299;-3776,2464;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-4880,1280;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;231;-4176,656;Inherit;False;_Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-5328,848;Inherit;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-3776,1968;Inherit;False;Constant;_Float9;Float 9;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-3776,1840;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;38;-5712,-384;Float;False;Property;_GradientColor;Gradient Color;6;0;Create;True;0;0;0;False;3;Space(10);Header(Gradient Parameters);Space(10);False;1,1,1,0;0.764151,0.764151,0.764151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;39;-5584,-192;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;-5712,-576;Float;False;Property;_MainColor;Main Color;2;0;Create;True;0;0;0;False;2;Header(Main Maps);Space(10);False;1,1,1,0;0.2592569,0.3679245,0.2308205,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;367;-5456,608;Inherit;True;Property;_ColorVariationNoise;Color Variation Noise;12;0;Create;True;0;0;0;False;0;False;-1;None;56fc026eb79effe4b85400592e33d639;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;287;-6016,-1600;Inherit;False;MicroWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-4096,3200;Float;False;Global;MicroPower;MicroPower;20;0;Create;False;0;0;0;False;0;False;0.05;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-4048,3072;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-3776,2096;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;209;-4736,1296;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;113;-3600,1920;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;-4112,3392;Inherit;False;287;MicroWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;-5360,-320;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;161;-5120,784;Inherit;False;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-5328,400;Inherit;False;Property;_ColorVariationPower;Color Variation Power;11;0;Create;True;0;0;0;False;0;False;1;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-3888,3104;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;283;-4752,1200;Inherit;False;231;_Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;-5328,224;Inherit;False;Property;_ColorVariation;Color Variation;10;0;Create;True;0;0;0;False;3;Space(10);Header(Color Variation);Space(10);False;1,0,0,0;0,0.505281,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-3632,3232;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-4608,1472;Inherit;False;Property;_HidePower;Hide Power;1;0;Create;True;0;0;0;False;0;False;2.5;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-4944,704;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;53;-4944,192;Inherit;True;ColorDodge;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;254;-3344,1920;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;303;-3616,3488;Inherit;False;Property;_MicroWindMultiplier;MicroWind Multiplier;15;0;Create;True;0;0;0;False;3;;;;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-4576,1264;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-3376,2208;Inherit;False;Property;_WindMultiplier;BaseWind Multiplier;14;0;Create;False;0;0;0;False;3;Space(10);Header(Multipliers);Space(10);False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;304;-3328,3296;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-3136,2096;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;-4384,1328;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-4560,176;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-4176,400;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-3088,1920;Inherit;False;BaseWind;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-3168,3296;Inherit;False;MicroWind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;222;-4176,1328;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;217;-3968,1344;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;-4736,-640;Inherit;False;151;BaseWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;339;-3920,400;Inherit;False;_Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;154;-4736,-528;Inherit;False;152;MicroWind;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-4544,-608;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-3680,1344;Inherit;False;OpacityDither;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;346;-4480,-1280;Inherit;False;339;_Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;363;-6656,-1024;Inherit;False;866;280;;3;57;65;342;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;233;-4288,-704;Inherit;False;205;OpacityDither;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;194;-4288,-1280;Inherit;False;Property;_WindDebugView;WindDebugView;21;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;309;-4224,-1168;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;232;-4288,-800;Inherit;False;231;_Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;386;-4021.625,-552.6804;Inherit;False;Property;_Scattering;Scattering;23;0;Create;True;0;0;0;False;1;Space(30);False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;310;-4032,-1232;Inherit;False;Property;_SeeVertexColor;See Vertex Color;22;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;293;-6032,-1280;Inherit;False;DepositLayerColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;305;-4368,3392;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StaticSwitch;292;-6304,-1280;Inherit;False;Property;_DepositLayerChannel;DepositLayer Channel;20;0;Create;True;0;0;0;False;0;False;0;2;2;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;234;-4096,-768;Inherit;False;Property;_HideSides;Hide Sides;0;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;65;-6320,-976;Inherit;True;Property;_Normal;Normal;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-6608,-976;Float;False;Property;_NormalPower;Normal Power;5;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;388;-4000.839,-995.3417;Inherit;False;Constant;_Float4;Float 4;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;342;-6016,-976;Inherit;False;_Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;387;-4016.102,-876.0679;Inherit;False;Constant;_Float3;Float 3;25;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;-4192,1968;Inherit;False;288;BaseWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;383;-3712,-895;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;380;-3712,-895;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;382;-3712,-895;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;385;-3712,-895;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;384;-3712,-895;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;381;-3712,-895;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;VegetationLeaves;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=AlphaTest=Queue=0;True;2;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;38;Workflow;1;Surface;0;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;1;  Translucency Strength;1,False,-1;  Normal Distortion;1,False,-1;  Scattering;1,False,-1;  Direct;1,False,-1;  Ambient;1,False,-1;  Shadow;0,False,-1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Write Depth;0;  Early Z;0;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;True;True;True;False;;False;0
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
WireConnection;372;0;175;0
WireConnection;372;1;190;0
WireConnection;288;0;285;0
WireConnection;212;0;210;0
WireConnection;126;0;131;0
WireConnection;126;2;139;0
WireConnection;135;0;127;0
WireConnection;211;0;210;0
WireConnection;213;0;211;0
WireConnection;213;1;212;0
WireConnection;176;0;372;0
WireConnection;379;0;126;0
WireConnection;379;1;135;0
WireConnection;29;0;27;2
WireConnection;32;0;172;0
WireConnection;32;1;176;0
WireConnection;295;0;290;0
WireConnection;214;0;213;0
WireConnection;148;0;379;0
WireConnection;148;1;149;0
WireConnection;102;0;100;0
WireConnection;102;1;101;0
WireConnection;200;0;199;0
WireConnection;200;1;214;0
WireConnection;155;0;28;0
WireConnection;36;0;34;0
WireConnection;36;1;32;0
WireConnection;129;0;105;0
WireConnection;129;1;148;0
WireConnection;306;1;27;2
WireConnection;306;0;29;0
WireConnection;297;0;295;0
WireConnection;297;1;294;0
WireConnection;40;0;36;0
WireConnection;33;0;306;0
WireConnection;33;1;155;0
WireConnection;108;0;129;0
WireConnection;108;1;107;0
WireConnection;298;1;297;0
WireConnection;298;0;296;0
WireConnection;103;0;102;0
WireConnection;106;0;102;0
WireConnection;71;0;41;1
WireConnection;71;1;41;3
WireConnection;204;0;200;0
WireConnection;72;0;42;0
WireConnection;109;0;106;0
WireConnection;109;1;108;0
WireConnection;207;0;204;0
WireConnection;49;0;40;0
WireConnection;37;0;33;0
WireConnection;37;1;35;0
WireConnection;284;1;55;1
WireConnection;284;0;55;2
WireConnection;284;2;55;3
WireConnection;284;3;55;4
WireConnection;104;0;103;0
WireConnection;104;1;129;0
WireConnection;362;0;71;0
WireConnection;362;1;72;0
WireConnection;299;0;298;0
WireConnection;216;0;207;0
WireConnection;231;0;56;4
WireConnection;112;0;104;0
WireConnection;112;1;299;0
WireConnection;39;0;37;0
WireConnection;367;1;362;0
WireConnection;287;0;284;0
WireConnection;52;0;49;0
WireConnection;52;1;44;0
WireConnection;111;0;109;0
WireConnection;111;1;299;0
WireConnection;209;0;216;0
WireConnection;113;0;112;0
WireConnection;113;1;114;0
WireConnection;113;2;111;0
WireConnection;46;0;43;0
WireConnection;46;1;38;0
WireConnection;46;2;39;0
WireConnection;161;0;367;0
WireConnection;161;1;162;0
WireConnection;54;0;52;0
WireConnection;54;1;51;0
WireConnection;62;0;54;0
WireConnection;62;1;289;0
WireConnection;50;0;45;0
WireConnection;50;1;161;0
WireConnection;53;0;46;0
WireConnection;53;1;48;0
WireConnection;53;2;45;0
WireConnection;254;0;113;0
WireConnection;215;0;283;0
WireConnection;215;1;209;0
WireConnection;304;0;62;0
WireConnection;304;1;303;0
WireConnection;164;0;254;0
WireConnection;164;1;165;0
WireConnection;224;0;215;0
WireConnection;224;1;223;0
WireConnection;58;0;46;0
WireConnection;58;1;53;0
WireConnection;58;2;50;0
WireConnection;63;0;58;0
WireConnection;63;1;56;0
WireConnection;151;0;164;0
WireConnection;152;0;304;0
WireConnection;222;0;224;0
WireConnection;217;0;222;0
WireConnection;339;0;63;0
WireConnection;115;0;153;0
WireConnection;115;1;154;0
WireConnection;205;0;217;0
WireConnection;194;1;346;0
WireConnection;194;0;115;0
WireConnection;310;1;194;0
WireConnection;310;0;309;0
WireConnection;293;0;292;0
WireConnection;305;0;44;0
WireConnection;292;1;55;1
WireConnection;292;0;55;2
WireConnection;292;2;55;3
WireConnection;292;3;55;4
WireConnection;234;1;232;0
WireConnection;234;0;233;0
WireConnection;65;5;57;0
WireConnection;342;0;65;0
WireConnection;381;0;310;0
WireConnection;381;4;388;0
WireConnection;381;6;234;0
WireConnection;381;7;387;0
WireConnection;381;15;386;0
WireConnection;381;8;115;0
ASEEND*/
//CHKSM=F5D62301F7FACA0FD8D9BD14C7279ADE449E0E64