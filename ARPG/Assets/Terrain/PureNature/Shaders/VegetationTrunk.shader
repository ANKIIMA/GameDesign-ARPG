// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/VegetationTrunk"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin][Header(Main Maps)][Space(10)]_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump" {}
		_NormalPower("Normal Power", Range( 0 , 1)) = 1
		_MetallicROcclusionGSmoothnessA("Metallic (R) Occlusion (G) Smoothness (A)", 2D) = "white" {}
		_MetallicPower("Metallic Power", Range( 0 , 1)) = 0.5
		_SmoothnessPower("Smoothness Power", Range( 0 , 1)) = 0.5
		_OcclusionPower("Occlusion Power", Range( 0 , 1)) = 1
		[Space(10)][Header(Deposit Maps)][Space(10)]_2ndColor("Color", Color) = (1,1,1,1)
		_DetailAlbedoMap("Albedo", 2D) = "white" {}
		_DetailNormalMap("Normal", 2D) = "bump" {}
		_2ndNormalPower("Normal Power", Range( 0 , 1)) = 1
		[Toggle]_BlendNormals("Blend Normals", Float) = 1
		_DetailMetallicGlossMap("Metallic (R) Occlusion (G) Smoothness (A)", 2D) = "black" {}
		_LayerMetallicPower("Layer Metallic Power", Range( 0 , 1)) = 0.5
		_LayerSmoothnessPower("Layer Smoothness Power", Range( 0 , 1)) = 0.5
		_LayerOcclusionPower("Layer Occlusion Power", Range( 0 , 1)) = 1
		_LayerMask("Layer Mask (R)", 2D) = "white" {}
		[Toggle(_INVERTMASK_ON)] _InvertMask("Invert Mask", Float) = 0
		[Space(10)][Header(Layer)][Space(10)][Toggle]_UseVertexColor("Use Vertex Color", Float) = 1
		[KeywordEnum(R,G,B,A)] _LayerChannel("Layer Channel", Float) = 1
		_LayerPower("Layer Power", Range( 0 , 1)) = 0.5
		_LayerThreshold("Layer Threshold", Range( 0 , 50)) = 50
		_LayerPosition("Layer Position", Float) = 0
		_LayerContrast("Layer Contrast", Float) = 0
		[Space(10)][Header(Wind)][Space(10)][KeywordEnum(R,G,B,A)] _BaseWindChannel("Base Wind Channel", Float) = 2
		_WindMultiplier("Wind Multiplier", Float) = 0
		_WindTrunkPosition("Wind Trunk Position", Float) = 0
		_WindTrunkContrast("Wind Trunk Contrast", Float) = 10
		[Space(10)][Header(Debug)][Space(10)][Toggle(_SEEVERTEXCOLOR_ON)] _SeeVertexColor("See Vertex Color", Float) = 0
		[KeywordEnum(RGBA,R,G,B,A)] _VertexColorChannel("Vertex Color Channel", Float) = 0
		[ASEEnd][Toggle(_WINDDEBUGVIEW_ON)] _WindDebugView("WindDebugView", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
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

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 2.0

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
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
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

			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature _SEEVERTEXCOLOR_ON
			#pragma shader_feature _WINDDEBUGVIEW_ON
			#pragma shader_feature_local _INVERTMASK_ON
			#pragma shader_feature_local _LAYERCHANNEL_R _LAYERCHANNEL_G _LAYERCHANNEL_B _LAYERCHANNEL_A
			#pragma shader_feature_local _VERTEXCOLORCHANNEL_RGBA _VERTEXCOLORCHANNEL_R _VERTEXCOLORCHANNEL_G _VERTEXCOLORCHANNEL_B _VERTEXCOLORCHANNEL_A


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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Color;
			float4 _MainTex_ST;
			float4 _2ndColor;
			float4 _DetailAlbedoMap_ST;
			float4 _LayerMask_ST;
			float4 _DetailNormalMap_ST;
			float4 _DetailMetallicGlossMap_ST;
			float4 _MetallicROcclusionGSmoothnessA_ST;
			float4 _BumpMap_ST;
			float _WindTrunkContrast;
			float _LayerSmoothnessPower;
			float _SmoothnessPower;
			float _LayerMetallicPower;
			float _MetallicPower;
			float _NormalPower;
			float _LayerPosition;
			float _LayerThreshold;
			float _LayerPower;
			float _OcclusionPower;
			float _LayerContrast;
			float _2ndNormalPower;
			float _UseVertexColor;
			float _WindMultiplier;
			float _WindTrunkPosition;
			float _BlendNormals;
			float _LayerOcclusionPower;
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
			sampler2D _MainTex;
			sampler2D _DetailAlbedoMap;
			sampler2D _LayerMask;
			sampler2D _DetailNormalMap;
			sampler2D _BumpMap;
			sampler2D _MetallicROcclusionGSmoothnessA;
			sampler2D _DetailMetallicGlossMap;


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

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = v.ase_color.a;
				#else
				float staticSwitch202 = v.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				
				o.ase_texcoord7.xy = v.texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = BaseWind163.xyz;
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

				float2 uv_MainTex = IN.ase_texcoord7.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 uv_DetailAlbedoMap = IN.ase_texcoord7.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
				float2 uv_LayerMask = IN.ase_texcoord7.xy * _LayerMask_ST.xy + _LayerMask_ST.zw;
				float4 tex2DNode141 = tex2D( _LayerMask, uv_LayerMask );
				#ifdef _INVERTMASK_ON
				float staticSwitch228 = ( 1.0 - tex2DNode141.r );
				#else
				float staticSwitch228 = tex2DNode141.r;
				#endif
				float2 uv_DetailNormalMap = IN.ase_texcoord7.xy * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
				float3 unpack137 = UnpackNormalScale( tex2D( _DetailNormalMap, uv_DetailNormalMap ), _2ndNormalPower );
				unpack137.z = lerp( 1, unpack137.z, saturate(_2ndNormalPower) );
				float3 tex2DNode137 = unpack137;
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal14 = tex2DNode137;
				float3 worldNormal14 = float3(dot(tanToWorld0,tanNormal14), dot(tanToWorld1,tanNormal14), dot(tanToWorld2,tanNormal14));
				float4 temp_cast_0 = (worldNormal14.y).xxxx;
				#if defined(_LAYERCHANNEL_R)
				float staticSwitch204 = IN.ase_color.r;
				#elif defined(_LAYERCHANNEL_G)
				float staticSwitch204 = IN.ase_color.g;
				#elif defined(_LAYERCHANNEL_B)
				float staticSwitch204 = IN.ase_color.b;
				#elif defined(_LAYERCHANNEL_A)
				float staticSwitch204 = IN.ase_color.a;
				#else
				float staticSwitch204 = IN.ase_color.g;
				#endif
				float DepositLayerColor205 = staticSwitch204;
				float saferPower109 = max( DepositLayerColor205 , 0.0001 );
				float4 temp_cast_1 = (pow( saferPower109 , _LayerPosition )).xxxx;
				float4 clampResult105 = clamp( CalculateContrast(_LayerContrast,temp_cast_1) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				float4 temp_cast_2 = (( 1.0 - _LayerPower )).xxxx;
				float4 temp_cast_3 = (_LayerThreshold).xxxx;
				float4 BlendAlpha85 = pow( saturate( ( ( staticSwitch228 + (( _UseVertexColor )?( ( pow( clampResult105 , temp_cast_2 ) * clampResult105 ) ):( temp_cast_0 )) ) + _LayerPower ) ) , temp_cast_3 );
				float4 lerpResult26 = lerp( ( _Color * tex2D( _MainTex, uv_MainTex ) ) , ( _2ndColor * tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap ) ) , BlendAlpha85);
				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float2 appendResult153 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = IN.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = IN.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = IN.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = IN.ase_color.a;
				#else
				float staticSwitch202 = IN.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_4 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_4) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch179 = BaseWind163;
				#else
				float4 staticSwitch179 = lerpResult26;
				#endif
				float4 temp_cast_9 = (IN.ase_color.r).xxxx;
				float4 temp_cast_10 = (IN.ase_color.g).xxxx;
				float4 temp_cast_11 = (IN.ase_color.b).xxxx;
				float4 temp_cast_12 = (IN.ase_color.a).xxxx;
				#if defined(_VERTEXCOLORCHANNEL_RGBA)
				float4 staticSwitch224 = IN.ase_color;
				#elif defined(_VERTEXCOLORCHANNEL_R)
				float4 staticSwitch224 = temp_cast_9;
				#elif defined(_VERTEXCOLORCHANNEL_G)
				float4 staticSwitch224 = temp_cast_10;
				#elif defined(_VERTEXCOLORCHANNEL_B)
				float4 staticSwitch224 = temp_cast_11;
				#elif defined(_VERTEXCOLORCHANNEL_A)
				float4 staticSwitch224 = temp_cast_12;
				#else
				float4 staticSwitch224 = IN.ase_color;
				#endif
				#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch209 = staticSwitch224;
				#else
				float4 staticSwitch209 = staticSwitch179;
				#endif
				float4 Albedo187 = staticSwitch209;
				
				float2 uv_BumpMap = IN.ase_texcoord7.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
				float3 unpack3 = UnpackNormalScale( tex2D( _BumpMap, uv_BumpMap ), _NormalPower );
				unpack3.z = lerp( 1, unpack3.z, saturate(_NormalPower) );
				float3 tex2DNode3 = unpack3;
				float3 lerpResult13 = lerp( tex2DNode3 , tex2DNode137 , BlendAlpha85.rgb);
				float4 color81 = IsGammaSpace() ? float4(0.01176471,0,1,1) : float4(0.0009105813,0,1,1);
				float4 lerpResult78 = lerp( color81 , float4( tex2DNode137 , 0.0 ) , BlendAlpha85);
				float3 Normals184 = (( _BlendNormals )?( BlendNormal( tex2DNode3 , lerpResult78.rgb ) ):( lerpResult13 ));
				
				float2 uv_MetallicROcclusionGSmoothnessA = IN.ase_texcoord7.xy * _MetallicROcclusionGSmoothnessA_ST.xy + _MetallicROcclusionGSmoothnessA_ST.zw;
				float4 tex2DNode7 = tex2D( _MetallicROcclusionGSmoothnessA, uv_MetallicROcclusionGSmoothnessA );
				float2 uv_DetailMetallicGlossMap = IN.ase_texcoord7.xy * _DetailMetallicGlossMap_ST.xy + _DetailMetallicGlossMap_ST.zw;
				float4 tex2DNode139 = tex2D( _DetailMetallicGlossMap, uv_DetailMetallicGlossMap );
				float lerpResult30 = lerp( ( tex2DNode7.r * _MetallicPower ) , ( tex2DNode139.r * _LayerMetallicPower ) , BlendAlpha85.r);
				float Metallic192 = lerpResult30;
				
				float lerpResult31 = lerp( ( tex2DNode7.a * _SmoothnessPower ) , ( tex2DNode139.a * _LayerSmoothnessPower ) , BlendAlpha85.r);
				float Smoothness193 = lerpResult31;
				
				float saferPower220 = max( tex2DNode7.g , 0.0001 );
				float temp_output_220_0 = pow( saferPower220 , _OcclusionPower );
				float saferPower221 = max( tex2DNode139.g , 0.0001 );
				float lerpResult33 = lerp( temp_output_220_0 , ( temp_output_220_0 * pow( saferPower221 , _LayerOcclusionPower ) ) , BlendAlpha85.r);
				float Occlusion191 = lerpResult33;
				
				float3 Albedo = Albedo187.rgb;
				float3 Normal = Normals184;
				float3 Emission = 0;
				float3 Specular = 0.5;
				float Metallic = Metallic192;
				float Smoothness = Smoothness193;
				float Occlusion = Occlusion191;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
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
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
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

			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Color;
			float4 _MainTex_ST;
			float4 _2ndColor;
			float4 _DetailAlbedoMap_ST;
			float4 _LayerMask_ST;
			float4 _DetailNormalMap_ST;
			float4 _DetailMetallicGlossMap_ST;
			float4 _MetallicROcclusionGSmoothnessA_ST;
			float4 _BumpMap_ST;
			float _WindTrunkContrast;
			float _LayerSmoothnessPower;
			float _SmoothnessPower;
			float _LayerMetallicPower;
			float _MetallicPower;
			float _NormalPower;
			float _LayerPosition;
			float _LayerThreshold;
			float _LayerPower;
			float _OcclusionPower;
			float _LayerContrast;
			float _2ndNormalPower;
			float _UseVertexColor;
			float _WindMultiplier;
			float _WindTrunkPosition;
			float _BlendNormals;
			float _LayerOcclusionPower;
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

			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = v.ase_color.a;
				#else
				float staticSwitch202 = v.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = BaseWind163.xyz;
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

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
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
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
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

			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Color;
			float4 _MainTex_ST;
			float4 _2ndColor;
			float4 _DetailAlbedoMap_ST;
			float4 _LayerMask_ST;
			float4 _DetailNormalMap_ST;
			float4 _DetailMetallicGlossMap_ST;
			float4 _MetallicROcclusionGSmoothnessA_ST;
			float4 _BumpMap_ST;
			float _WindTrunkContrast;
			float _LayerSmoothnessPower;
			float _SmoothnessPower;
			float _LayerMetallicPower;
			float _MetallicPower;
			float _NormalPower;
			float _LayerPosition;
			float _LayerThreshold;
			float _LayerPower;
			float _OcclusionPower;
			float _LayerContrast;
			float _2ndNormalPower;
			float _UseVertexColor;
			float _WindMultiplier;
			float _WindTrunkPosition;
			float _BlendNormals;
			float _LayerOcclusionPower;
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

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = v.ase_color.a;
				#else
				float staticSwitch202 = v.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = BaseWind163.xyz;
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

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
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
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
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
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature _SEEVERTEXCOLOR_ON
			#pragma shader_feature _WINDDEBUGVIEW_ON
			#pragma shader_feature_local _INVERTMASK_ON
			#pragma shader_feature_local _LAYERCHANNEL_R _LAYERCHANNEL_G _LAYERCHANNEL_B _LAYERCHANNEL_A
			#pragma shader_feature_local _VERTEXCOLORCHANNEL_RGBA _VERTEXCOLORCHANNEL_R _VERTEXCOLORCHANNEL_G _VERTEXCOLORCHANNEL_B _VERTEXCOLORCHANNEL_A


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
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
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Color;
			float4 _MainTex_ST;
			float4 _2ndColor;
			float4 _DetailAlbedoMap_ST;
			float4 _LayerMask_ST;
			float4 _DetailNormalMap_ST;
			float4 _DetailMetallicGlossMap_ST;
			float4 _MetallicROcclusionGSmoothnessA_ST;
			float4 _BumpMap_ST;
			float _WindTrunkContrast;
			float _LayerSmoothnessPower;
			float _SmoothnessPower;
			float _LayerMetallicPower;
			float _MetallicPower;
			float _NormalPower;
			float _LayerPosition;
			float _LayerThreshold;
			float _LayerPower;
			float _OcclusionPower;
			float _LayerContrast;
			float _2ndNormalPower;
			float _UseVertexColor;
			float _WindMultiplier;
			float _WindTrunkPosition;
			float _BlendNormals;
			float _LayerOcclusionPower;
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
			sampler2D _MainTex;
			sampler2D _DetailAlbedoMap;
			sampler2D _LayerMask;
			sampler2D _DetailNormalMap;


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

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = v.ase_color.a;
				#else
				float staticSwitch202 = v.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord3.xyz = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord5.xyz = ase_worldBitangent;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = BaseWind163.xyz;
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
				float4 ase_tangent : TANGENT;

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
				o.ase_tangent = v.ase_tangent;
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
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
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

				float2 uv_MainTex = IN.ase_texcoord2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 uv_DetailAlbedoMap = IN.ase_texcoord2.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
				float2 uv_LayerMask = IN.ase_texcoord2.xy * _LayerMask_ST.xy + _LayerMask_ST.zw;
				float4 tex2DNode141 = tex2D( _LayerMask, uv_LayerMask );
				#ifdef _INVERTMASK_ON
				float staticSwitch228 = ( 1.0 - tex2DNode141.r );
				#else
				float staticSwitch228 = tex2DNode141.r;
				#endif
				float2 uv_DetailNormalMap = IN.ase_texcoord2.xy * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
				float3 unpack137 = UnpackNormalScale( tex2D( _DetailNormalMap, uv_DetailNormalMap ), _2ndNormalPower );
				unpack137.z = lerp( 1, unpack137.z, saturate(_2ndNormalPower) );
				float3 tex2DNode137 = unpack137;
				float3 ase_worldTangent = IN.ase_texcoord3.xyz;
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord5.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal14 = tex2DNode137;
				float3 worldNormal14 = float3(dot(tanToWorld0,tanNormal14), dot(tanToWorld1,tanNormal14), dot(tanToWorld2,tanNormal14));
				float4 temp_cast_0 = (worldNormal14.y).xxxx;
				#if defined(_LAYERCHANNEL_R)
				float staticSwitch204 = IN.ase_color.r;
				#elif defined(_LAYERCHANNEL_G)
				float staticSwitch204 = IN.ase_color.g;
				#elif defined(_LAYERCHANNEL_B)
				float staticSwitch204 = IN.ase_color.b;
				#elif defined(_LAYERCHANNEL_A)
				float staticSwitch204 = IN.ase_color.a;
				#else
				float staticSwitch204 = IN.ase_color.g;
				#endif
				float DepositLayerColor205 = staticSwitch204;
				float saferPower109 = max( DepositLayerColor205 , 0.0001 );
				float4 temp_cast_1 = (pow( saferPower109 , _LayerPosition )).xxxx;
				float4 clampResult105 = clamp( CalculateContrast(_LayerContrast,temp_cast_1) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				float4 temp_cast_2 = (( 1.0 - _LayerPower )).xxxx;
				float4 temp_cast_3 = (_LayerThreshold).xxxx;
				float4 BlendAlpha85 = pow( saturate( ( ( staticSwitch228 + (( _UseVertexColor )?( ( pow( clampResult105 , temp_cast_2 ) * clampResult105 ) ):( temp_cast_0 )) ) + _LayerPower ) ) , temp_cast_3 );
				float4 lerpResult26 = lerp( ( _Color * tex2D( _MainTex, uv_MainTex ) ) , ( _2ndColor * tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap ) ) , BlendAlpha85);
				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float2 appendResult153 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = IN.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = IN.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = IN.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = IN.ase_color.a;
				#else
				float staticSwitch202 = IN.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_4 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_4) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch179 = BaseWind163;
				#else
				float4 staticSwitch179 = lerpResult26;
				#endif
				float4 temp_cast_9 = (IN.ase_color.r).xxxx;
				float4 temp_cast_10 = (IN.ase_color.g).xxxx;
				float4 temp_cast_11 = (IN.ase_color.b).xxxx;
				float4 temp_cast_12 = (IN.ase_color.a).xxxx;
				#if defined(_VERTEXCOLORCHANNEL_RGBA)
				float4 staticSwitch224 = IN.ase_color;
				#elif defined(_VERTEXCOLORCHANNEL_R)
				float4 staticSwitch224 = temp_cast_9;
				#elif defined(_VERTEXCOLORCHANNEL_G)
				float4 staticSwitch224 = temp_cast_10;
				#elif defined(_VERTEXCOLORCHANNEL_B)
				float4 staticSwitch224 = temp_cast_11;
				#elif defined(_VERTEXCOLORCHANNEL_A)
				float4 staticSwitch224 = temp_cast_12;
				#else
				float4 staticSwitch224 = IN.ase_color;
				#endif
				#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch209 = staticSwitch224;
				#else
				float4 staticSwitch209 = staticSwitch179;
				#endif
				float4 Albedo187 = staticSwitch209;
				
				
				float3 Albedo = Albedo187.rgb;
				float3 Emission = 0;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

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
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMALMAP 1
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
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
			#pragma shader_feature _SEEVERTEXCOLOR_ON
			#pragma shader_feature _WINDDEBUGVIEW_ON
			#pragma shader_feature_local _INVERTMASK_ON
			#pragma shader_feature_local _LAYERCHANNEL_R _LAYERCHANNEL_G _LAYERCHANNEL_B _LAYERCHANNEL_A
			#pragma shader_feature_local _VERTEXCOLORCHANNEL_RGBA _VERTEXCOLORCHANNEL_R _VERTEXCOLORCHANNEL_G _VERTEXCOLORCHANNEL_B _VERTEXCOLORCHANNEL_A


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
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
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Color;
			float4 _MainTex_ST;
			float4 _2ndColor;
			float4 _DetailAlbedoMap_ST;
			float4 _LayerMask_ST;
			float4 _DetailNormalMap_ST;
			float4 _DetailMetallicGlossMap_ST;
			float4 _MetallicROcclusionGSmoothnessA_ST;
			float4 _BumpMap_ST;
			float _WindTrunkContrast;
			float _LayerSmoothnessPower;
			float _SmoothnessPower;
			float _LayerMetallicPower;
			float _MetallicPower;
			float _NormalPower;
			float _LayerPosition;
			float _LayerThreshold;
			float _LayerPower;
			float _OcclusionPower;
			float _LayerContrast;
			float _2ndNormalPower;
			float _UseVertexColor;
			float _WindMultiplier;
			float _WindTrunkPosition;
			float _BlendNormals;
			float _LayerOcclusionPower;
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
			sampler2D _MainTex;
			sampler2D _DetailAlbedoMap;
			sampler2D _LayerMask;
			sampler2D _DetailNormalMap;


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

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = v.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = v.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = v.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = v.ase_color.a;
				#else
				float staticSwitch202 = v.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_0 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord3.xyz = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord5.xyz = ase_worldBitangent;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = BaseWind163.xyz;
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
				float4 ase_tangent : TANGENT;

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
				o.ase_tangent = v.ase_tangent;
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
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
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

				float2 uv_MainTex = IN.ase_texcoord2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 uv_DetailAlbedoMap = IN.ase_texcoord2.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
				float2 uv_LayerMask = IN.ase_texcoord2.xy * _LayerMask_ST.xy + _LayerMask_ST.zw;
				float4 tex2DNode141 = tex2D( _LayerMask, uv_LayerMask );
				#ifdef _INVERTMASK_ON
				float staticSwitch228 = ( 1.0 - tex2DNode141.r );
				#else
				float staticSwitch228 = tex2DNode141.r;
				#endif
				float2 uv_DetailNormalMap = IN.ase_texcoord2.xy * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
				float3 unpack137 = UnpackNormalScale( tex2D( _DetailNormalMap, uv_DetailNormalMap ), _2ndNormalPower );
				unpack137.z = lerp( 1, unpack137.z, saturate(_2ndNormalPower) );
				float3 tex2DNode137 = unpack137;
				float3 ase_worldTangent = IN.ase_texcoord3.xyz;
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord5.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal14 = tex2DNode137;
				float3 worldNormal14 = float3(dot(tanToWorld0,tanNormal14), dot(tanToWorld1,tanNormal14), dot(tanToWorld2,tanNormal14));
				float4 temp_cast_0 = (worldNormal14.y).xxxx;
				#if defined(_LAYERCHANNEL_R)
				float staticSwitch204 = IN.ase_color.r;
				#elif defined(_LAYERCHANNEL_G)
				float staticSwitch204 = IN.ase_color.g;
				#elif defined(_LAYERCHANNEL_B)
				float staticSwitch204 = IN.ase_color.b;
				#elif defined(_LAYERCHANNEL_A)
				float staticSwitch204 = IN.ase_color.a;
				#else
				float staticSwitch204 = IN.ase_color.g;
				#endif
				float DepositLayerColor205 = staticSwitch204;
				float saferPower109 = max( DepositLayerColor205 , 0.0001 );
				float4 temp_cast_1 = (pow( saferPower109 , _LayerPosition )).xxxx;
				float4 clampResult105 = clamp( CalculateContrast(_LayerContrast,temp_cast_1) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				float4 temp_cast_2 = (( 1.0 - _LayerPower )).xxxx;
				float4 temp_cast_3 = (_LayerThreshold).xxxx;
				float4 BlendAlpha85 = pow( saturate( ( ( staticSwitch228 + (( _UseVertexColor )?( ( pow( clampResult105 , temp_cast_2 ) * clampResult105 ) ):( temp_cast_0 )) ) + _LayerPower ) ) , temp_cast_3 );
				float4 lerpResult26 = lerp( ( _Color * tex2D( _MainTex, uv_MainTex ) ) , ( _2ndColor * tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap ) ) , BlendAlpha85);
				float temp_output_127_0 = ( _TimeParameters.x * WindSpeed );
				float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
				float2 appendResult153 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
				float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
				simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
				float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
				#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = IN.ase_color.r;
				#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = IN.ase_color.g;
				#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = IN.ase_color.b;
				#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = IN.ase_color.a;
				#else
				float staticSwitch202 = IN.ase_color.b;
				#endif
				float BaseWindColor203 = staticSwitch202;
				float saferPower131 = max( ( 1.0 - BaseWindColor203 ) , 0.0001 );
				float4 temp_cast_4 = (pow( saferPower131 , _WindTrunkPosition )).xxxx;
				float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_4) );
				float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
				float4 transform183 = mul(GetWorldToObjectMatrix(),float4( appendResult124 , 0.0 ));
				float4 BaseWind163 = ( transform183 * _WindMultiplier );
				#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch179 = BaseWind163;
				#else
				float4 staticSwitch179 = lerpResult26;
				#endif
				float4 temp_cast_9 = (IN.ase_color.r).xxxx;
				float4 temp_cast_10 = (IN.ase_color.g).xxxx;
				float4 temp_cast_11 = (IN.ase_color.b).xxxx;
				float4 temp_cast_12 = (IN.ase_color.a).xxxx;
				#if defined(_VERTEXCOLORCHANNEL_RGBA)
				float4 staticSwitch224 = IN.ase_color;
				#elif defined(_VERTEXCOLORCHANNEL_R)
				float4 staticSwitch224 = temp_cast_9;
				#elif defined(_VERTEXCOLORCHANNEL_G)
				float4 staticSwitch224 = temp_cast_10;
				#elif defined(_VERTEXCOLORCHANNEL_B)
				float4 staticSwitch224 = temp_cast_11;
				#elif defined(_VERTEXCOLORCHANNEL_A)
				float4 staticSwitch224 = temp_cast_12;
				#else
				float4 staticSwitch224 = IN.ase_color;
				#endif
				#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch209 = staticSwitch224;
				#else
				float4 staticSwitch209 = staticSwitch179;
				#endif
				float4 Albedo187 = staticSwitch209;
				
				
				float3 Albedo = Albedo187.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

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
0;73;1920;928;3598.838;-296.0677;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;208;-3328,2304;Inherit;False;929.6667;543;;7;201;205;203;202;204;200;199;VertexColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;199;-3296,2496;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;204;-2912,2528;Inherit;False;Property;_LayerChannel;Layer Channel;20;0;Create;True;0;0;0;False;0;False;0;1;1;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;159;-3328,1152;Inherit;False;3645.007;1018.856;;36;163;162;161;183;124;120;123;122;177;125;129;115;117;133;116;131;114;148;134;127;121;132;119;126;157;128;158;155;150;152;156;153;154;149;206;240;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;160;-3328,256;Inherit;False;2925.065;756.1433;;22;35;16;17;25;24;22;73;72;93;110;113;109;112;105;69;141;37;14;85;207;227;228;Blend Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-2624,2528;Inherit;False;DepositLayerColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-3280,1728;Inherit;False;Global;WindBurstsSpeed;Wind Bursts Speed;23;0;Create;True;0;0;0;False;1;Space(10);False;50;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;154;-3248,1552;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;110;-3296,576;Inherit;False;Property;_LayerPosition;Layer Position;23;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;207;-3296,448;Inherit;False;205;DepositLayerColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;202;-2912,2688;Inherit;False;Property;_BaseWindChannel;Base Wind Channel;26;0;Create;True;0;0;0;False;3;Space(10);Header(Wind);Space(10);False;0;2;2;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-3296,704;Inherit;False;Property;_LayerContrast;Layer Contrast;24;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;109;-3072,448;Inherit;True;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;152;-3056,1728;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-3072,1856;Inherit;False;Global;WindBurstsScale;Wind Bursts Scale;24;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;153;-3056,1584;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;150;-2880,1664;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;-2624,2688;Inherit;False;BaseWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;155;-2832,1856;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2816,352;Float;False;Property;_LayerPower;Layer Power;21;0;Create;True;0;0;0;False;0;False;0.5;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;185;-3328,-768;Inherit;False;2150;847;;13;64;9;137;8;3;86;81;78;87;75;13;76;184;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;112;-2816,512;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-2608,1920;Inherit;False;Global;WindBurstsPower;Wind Bursts Power;25;0;Create;True;0;0;0;False;0;False;10;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;105;-2592,656;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;73;-2528,320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;240;-2560,1664;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-3280,-96;Inherit;False;Property;_2ndNormalPower;Normal Power;11;0;Create;False;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;64;-3280,-304;Inherit;True;Property;_DetailNormalMap;Normal;10;0;Create;False;0;0;0;False;0;False;None;9302f85d940c1e24abf248a813b1ef87;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;206;-1920,1824;Inherit;False;203;BaseWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;69;-2944,800;Inherit;True;Property;_LayerMask;Layer Mask (R);17;0;Create;False;0;0;0;False;0;False;None;e1f9e4b4f78e10041804ffee938870e2;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;126;-2496,1456;Inherit;False;Global;WindSpeed;Wind Speed;21;0;Create;True;0;0;0;False;1;Space(10);False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;119;-1648,1824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-1984,1328;Inherit;False;Global;WindPower;Wind Power;22;0;Create;True;0;0;0;False;0;False;0.01;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;121;-2496,1328;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;72;-2400,480;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-2304,1792;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;137;-2992,-304;Inherit;True;Property;_TextureSample0;Texture Sample 0;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;132;-1712,1968;Inherit;False;Property;_WindTrunkPosition;Wind Trunk Position;28;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;141;-2688,800;Inherit;True;Property;_TextureSample4;Texture Sample 4;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;14;-1920,320;Inherit;True;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-2240,1328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2144,640;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-1504,2064;Inherit;False;Property;_WindTrunkContrast;Wind Trunk Contrast;29;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;227;-2368,896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-1680,1584;Inherit;False;Constant;_Float8;Float 8;18;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;131;-1472,1904;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1696,1328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;228;-2160,816;Inherit;False;Property;_InvertMask;Invert Mask;18;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;115;-1984,1456;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;35;-1802,588;Inherit;False;Property;_UseVertexColor;Use Vertex Color;19;0;Create;True;0;0;0;False;3;Space(10);Header(Layer);Space(10);False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;116;-1984,1200;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;133;-1296,1952;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-1472,1568;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;177;-1088,1952;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-1472,1200;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1535,583;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-1136,1472;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-880,1216;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-880,1360;Inherit;False;Constant;_Float9;Float 9;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-880,1472;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1376,384;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;124;-624,1344;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1280,704;Float;False;Property;_LayerThreshold;Layer Threshold;22;0;Create;True;0;0;0;False;0;False;50;50;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-1152,384;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;186;-3328,-1792;Inherit;False;2144;868;;15;187;179;178;26;12;88;10;1;11;2;138;61;209;210;224;Diffuse / Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;61;-3296,-1136;Inherit;True;Property;_DetailAlbedoMap;Albedo;9;0;Create;False;0;0;0;False;0;False;None;1bbb6f363f884124aaa1cad8329f78cf;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PowerNode;24;-960,384;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;183;-480,1344;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;161;-384,1536;Inherit;False;Property;_WindMultiplier;Wind Multiplier;27;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-3040,-1552;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;0;False;0;False;-1;None;e5ef5502120f8f34c8b43394a9d15cc3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-3040,-1744;Inherit;False;Property;_Color;Main Color;0;0;Create;False;0;0;0;False;2;Header(Main Maps);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-128,1408;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;11;-3040,-1328;Inherit;False;Property;_2ndColor;Color;8;0;Create;False;0;0;0;False;3;Space(10);Header(Deposit Maps);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;138;-3040,-1136;Inherit;True;Property;_TextureSample1;Texture Sample 1;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-640,384;Inherit;False;BlendAlpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;163;128,1408;Inherit;False;BaseWind;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2656,-1232;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2656,-1616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-2656,-1104;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;210;-2048,-1152;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;178;-2336,-1232;Inherit;False;163;BaseWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;26;-2400,-1488;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;224;-1792,-1152;Inherit;False;Property;_VertexColorChannel;Vertex Color Channel;31;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;5;RGBA;R;G;B;A;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;179;-2080,-1360;Inherit;False;Property;_WindDebugView;WindDebugView;32;0;Create;False;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;209;-1760,-1360;Inherit;False;Property;_SeeVertexColor;See Vertex Color;30;0;Create;True;0;0;0;False;3;Space(10);Header(Debug);Space(10);False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;187;-1376,-1360;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;194;-3328,-3072;Inherit;False;2042.584;1153.664;;26;217;215;89;33;31;191;193;30;233;91;90;229;192;211;231;232;66;212;218;221;220;222;139;223;230;7;Metallic / Smoothness / Occlusion;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-2112,-3008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-2992,-560;Inherit;True;Property;_BumpMap;Normal;2;0;Create;False;0;0;0;False;0;False;-1;None;df89806e8af9bd243ad7d2756c2bc349;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-1920,-2688;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;75;-1952,-336;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-3280,-560;Inherit;False;Property;_NormalPower;Normal Power;3;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;81;-2560,-288;Inherit;False;Constant;_Color0;Color 0;22;0;Create;True;0;0;0;False;0;False;0.01176471,0,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;13;-1952,-720;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-2560,-64;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-2208,-592;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-1408,-464;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;78;-2256,-176;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-2944,-3008;Inherit;True;Property;_MetallicGlossMap;Metallic (R) Occlusion (G) Smoothness (A);4;0;Create;False;0;0;0;False;0;False;-1;None;9b90f87fd4161d84cbe99dbd8e9bad23;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;189;639.2126,383.9426;Inherit;False;184;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;191;-1536,-2304;Inherit;False;Occlusion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;201;-2624,2368;Inherit;False;MicroWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;196;639.2126,607.9427;Inherit;False;193;Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;639.2126,799.9426;Inherit;False;163;BaseWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;639.2126,255.9426;Inherit;False;187;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;200;-2912,2368;Inherit;False;Property;_MicroWindChannel;Micro Wind Channel;25;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;211;-3296,-3008;Inherit;True;Property;_MetallicROcclusionGSmoothnessA;Metallic (R) Occlusion (G) Smoothness (A);4;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;139;-2944,-2784;Inherit;True;Property;_TextureSample2;Texture Sample 2;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;-1920,-2256;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;638.9548,507.2086;Inherit;False;192;Metallic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;193;-1664,-2688;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;233;-2112,-2880;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;30;-1920,-3008;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-2112,-2688;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;192;-1664,-3008;Inherit;False;Metallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;-2112,-2048;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-2112,-2560;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;66;-3296,-2784;Inherit;True;Property;_DetailMetallicGlossMap;Metallic (R) Occlusion (G) Smoothness (A);13;0;Create;False;0;0;0;False;0;False;None;8d961c07e5665584094383581d95e88e;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;197;639.2126,703.9426;Inherit;False;191;Occlusion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-2432,-2880;Inherit;False;Property;_LayerMetallicPower;Layer Metallic Power;14;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-2112,-2464;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-2432,-2304;Inherit;False;Property;_OcclusionPower;Occlusion Power;7;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1776,-2304;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-2432,-3008;Inherit;False;Property;_MetallicPower;Metallic Power;5;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;76;-1616,-464;Inherit;False;Property;_BlendNormals;Blend Normals;12;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;212;-2432,-2688;Inherit;False;Property;_SmoothnessPower;Smoothness Power;6;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;220;-2112,-2304;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;221;-2112,-2176;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-2112,-2784;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-2432,-2176;Inherit;False;Property;_LayerOcclusionPower;Layer Occlusion Power;16;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;-2432,-2560;Inherit;False;Property;_LayerSmoothnessPower;Layer Smoothness Power;15;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;243;953.9794,386.8527;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;245;953.9794,386.8527;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;244;953.9794,386.8527;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;242;953.9794,386.8527;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;Custom/VegetationTrunk;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;38;Workflow;1;Surface;0;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Write Depth;0;  Early Z;0;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;241;953.9794,386.8527;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;246;953.9794,386.8527;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;204;1;199;1
WireConnection;204;0;199;2
WireConnection;204;2;199;3
WireConnection;204;3;199;4
WireConnection;205;0;204;0
WireConnection;202;1;199;1
WireConnection;202;0;199;2
WireConnection;202;2;199;3
WireConnection;202;3;199;4
WireConnection;109;0;207;0
WireConnection;109;1;110;0
WireConnection;152;0;149;0
WireConnection;152;1;149;0
WireConnection;153;0;154;1
WireConnection;153;1;154;3
WireConnection;150;0;153;0
WireConnection;150;2;152;0
WireConnection;203;0;202;0
WireConnection;155;0;156;0
WireConnection;112;1;109;0
WireConnection;112;0;113;0
WireConnection;105;0;112;0
WireConnection;73;0;22;0
WireConnection;240;0;150;0
WireConnection;240;1;155;0
WireConnection;119;0;206;0
WireConnection;72;0;105;0
WireConnection;72;1;73;0
WireConnection;157;0;240;0
WireConnection;157;1;158;0
WireConnection;137;0;64;0
WireConnection;137;5;9;0
WireConnection;141;0;69;0
WireConnection;14;0;137;0
WireConnection;127;0;121;0
WireConnection;127;1;126;0
WireConnection;93;0;72;0
WireConnection;93;1;105;0
WireConnection;227;0;141;1
WireConnection;131;0;119;0
WireConnection;131;1;132;0
WireConnection;148;0;128;0
WireConnection;148;1;157;0
WireConnection;228;1;141;1
WireConnection;228;0;227;0
WireConnection;115;0;127;0
WireConnection;35;0;14;2
WireConnection;35;1;93;0
WireConnection;116;0;127;0
WireConnection;133;1;131;0
WireConnection;133;0;134;0
WireConnection;117;0;148;0
WireConnection;117;1;114;0
WireConnection;177;0;133;0
WireConnection;125;0;116;0
WireConnection;125;1;148;0
WireConnection;37;0;228;0
WireConnection;37;1;35;0
WireConnection;129;0;115;0
WireConnection;129;1;117;0
WireConnection;122;0;125;0
WireConnection;122;1;177;0
WireConnection;123;0;129;0
WireConnection;123;1;177;0
WireConnection;16;0;37;0
WireConnection;16;1;22;0
WireConnection;124;0;122;0
WireConnection;124;1;120;0
WireConnection;124;2;123;0
WireConnection;17;0;16;0
WireConnection;24;0;17;0
WireConnection;24;1;25;0
WireConnection;183;0;124;0
WireConnection;162;0;183;0
WireConnection;162;1;161;0
WireConnection;138;0;61;0
WireConnection;85;0;24;0
WireConnection;163;0;162;0
WireConnection;12;0;11;0
WireConnection;12;1;138;0
WireConnection;10;0;2;0
WireConnection;10;1;1;0
WireConnection;26;0;10;0
WireConnection;26;1;12;0
WireConnection;26;2;88;0
WireConnection;224;1;210;0
WireConnection;224;0;210;1
WireConnection;224;2;210;2
WireConnection;224;3;210;3
WireConnection;224;4;210;4
WireConnection;179;1;26;0
WireConnection;179;0;178;0
WireConnection;209;1;179;0
WireConnection;209;0;224;0
WireConnection;187;0;209;0
WireConnection;232;0;7;1
WireConnection;232;1;230;0
WireConnection;3;5;8;0
WireConnection;31;0;215;0
WireConnection;31;1;217;0
WireConnection;31;2;90;0
WireConnection;75;0;3;0
WireConnection;75;1;78;0
WireConnection;13;0;3;0
WireConnection;13;1;137;0
WireConnection;13;2;87;0
WireConnection;184;0;76;0
WireConnection;78;0;81;0
WireConnection;78;1;137;0
WireConnection;78;2;86;0
WireConnection;7;0;211;0
WireConnection;191;0;33;0
WireConnection;201;0;200;0
WireConnection;200;1;199;1
WireConnection;200;0;199;2
WireConnection;200;2;199;3
WireConnection;200;3;199;4
WireConnection;139;0;66;0
WireConnection;229;0;220;0
WireConnection;229;1;221;0
WireConnection;193;0;31;0
WireConnection;233;0;139;1
WireConnection;233;1;231;0
WireConnection;30;0;232;0
WireConnection;30;1;233;0
WireConnection;30;2;89;0
WireConnection;215;0;7;4
WireConnection;215;1;212;0
WireConnection;192;0;30;0
WireConnection;217;0;139;4
WireConnection;217;1;218;0
WireConnection;33;0;220;0
WireConnection;33;1;229;0
WireConnection;33;2;91;0
WireConnection;76;0;13;0
WireConnection;76;1;75;0
WireConnection;220;0;7;2
WireConnection;220;1;222;0
WireConnection;221;0;139;2
WireConnection;221;1;223;0
WireConnection;242;0;188;0
WireConnection;242;1;189;0
WireConnection;242;3;195;0
WireConnection;242;4;196;0
WireConnection;242;5;197;0
WireConnection;242;8;164;0
ASEEND*/
//CHKSM=9BD4D762C2F995999D2F6759ECC01EB5A59FAD19