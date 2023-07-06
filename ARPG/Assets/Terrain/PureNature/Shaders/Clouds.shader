// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Clouds"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_ViewDistance("View Distance", Float) = 5000
		_DirectionSpeed("Direction / Speed", Vector) = (20,5,10,-5)
		[Header(Main)][Space(10)]_Noise01("Noise 01", 2D) = "white" {}
		_Tiling01("Tiling", Float) = 0.02
		_Noise01Power("Power", Range( 0 , 1)) = 0.75
		[Space(30)]_Noise02("Noise 02", 2D) = "white" {}
		_Tiling02("Tiling", Float) = 0.03
		_Noise02Power("Power", Range( 0 , 1)) = 0.3
		[Space(10)][Header(Clouds)][Space(10)]_ScatteringColor("Color", Color) = (1,1,1,1)
		_Coverage("Coverage", Range( 0 , 1)) = 0.3
		_Softness("Softness", Range( 0 , 1)) = 0.25
		[Toggle(_USEFOG_ON)] _UseFog("Use Fog", Float) = 1
		[Space(10)][Header(Scattering)][Space(10)]_CloudsColor("Color", Color) = (0.1843137,0.3568628,0.4627451,1)
		[HideInInspector]_cloudsPosition("cloudsPosition", Float) = 1
		[ASEEnd]_ScatteringPower("Absorption", Range( 0 , 50)) = 10
		[HideInInspector]_cloudsHeight("cloudsHeight", Float) = 1

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

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull Off
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
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _USEFOG_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
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
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ScatteringColor;
			float4 _CloudsColor;
			float4 _DirectionSpeed;
			float _cloudsPosition;
			float _cloudsHeight;
			float _Coverage;
			float _ScatteringPower;
			float _ViewDistance;
			float _Tiling02;
			float _Tiling01;
			float _Noise01Power;
			float _Noise02Power;
			float _Softness;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Noise02;
			sampler2D _Noise01;


						
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord3.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.yzw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
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

			half4 frag ( VertexOutput IN  ) : SV_Target
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
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float dotResult109 = dot( ase_worldViewDir , -_MainLightPosition.xyz );
				float temp_output_114_0 = pow( saturate( dotResult109 ) , 5.0 );
				float Coverage211 = _Coverage;
				float CloudHeight130 = ( 1.0 - pow( saturate( ( abs( ( _cloudsPosition - WorldPosition.y ) ) / _cloudsHeight ) ) , ( 1.0 - Coverage211 ) ) );
				float temp_output_197_0 = pow( ( 1 * CloudHeight130 ) , _ScatteringPower );
				float4 Scattering119 = ( ( ( _MainLightColor * temp_output_114_0 ) + ( temp_output_114_0 * unity_AmbientSky * _MainLightColor ) ) * temp_output_197_0 );
				float Lerp223 = temp_output_197_0;
				float eyeDepth = IN.ase_texcoord3.x;
				float cameraDepthFade66 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ViewDistance);
				float DistanceFade124 = ( 1.0 - cameraDepthFade66 );
				#ifdef _USEFOG_ON
				float staticSwitch192 = saturate( ( DistanceFade124 / 1.0 ) );
				#else
				float staticSwitch192 = 1.0;
				#endif
				float4 lerpResult181 = lerp( unity_FogColor , _CloudsColor , staticSwitch192);
				float4 CloudLighting137 = ( Scattering119 + ( ( _ScatteringColor * Lerp223 ) + lerpResult181 ) );
				
				float2 appendResult161 = (float2(_DirectionSpeed.z , _DirectionSpeed.w));
				float2 appendResult4 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner159 = ( _TimeParameters.x * appendResult161 + appendResult4);
				float3 temp_cast_1 = (tex2D( _Noise02, ( panner159 * ( _Tiling02 / 10000.0 ) ) ).r).xxx;
				float3 temp_cast_2 = (tex2D( _Noise02, ( panner159 * ( _Tiling02 / 10000.0 ) ) ).r).xxx;
				float3 linearToGamma92 = FastLinearToSRGB( temp_cast_2 );
				float2 appendResult160 = (float2(_DirectionSpeed.x , _DirectionSpeed.y));
				float2 panner157 = ( _TimeParameters.x * appendResult160 + appendResult4);
				float3 temp_cast_3 = (tex2D( _Noise01, ( panner157 * ( _Tiling01 / 10000.0 ) ) ).r).xxx;
				float3 temp_cast_4 = (tex2D( _Noise01, ( panner157 * ( _Tiling01 / 10000.0 ) ) ).r).xxx;
				float3 linearToGamma91 = FastLinearToSRGB( temp_cast_4 );
				float3 blendOpSrc155 = linearToGamma92;
				float3 blendOpDest155 = ( linearToGamma91 * _Noise01Power );
				float3 lerpBlendMode155 = lerp(blendOpDest155,max( blendOpSrc155, blendOpDest155 ),_Noise02Power);
				float3 Noise121 = ( saturate( lerpBlendMode155 ));
				float3 temp_cast_5 = (( 1.0 - (0.0 + (Coverage211 - 0.0) * (1.0 - 0.0) / (0.98 - 0.0)) )).xxx;
				float3 temp_cast_6 = (_Softness).xxx;
				float3 CloudCoverage134 = pow( saturate( (float3( 0,0,0 ) + (( Noise121 * CloudHeight130 ) - temp_cast_5) * (float3( 1,0,0 ) - float3( 0,0,0 )) / (float3( 1,0,0 ) - temp_cast_5)) ) , temp_cast_6 );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = CloudLighting137.rgb;
				float Alpha = saturate( ( DistanceFade124 * CloudCoverage134 ) ).x;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
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
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ScatteringColor;
			float4 _CloudsColor;
			float4 _DirectionSpeed;
			float _cloudsPosition;
			float _cloudsHeight;
			float _Coverage;
			float _ScatteringPower;
			float _ViewDistance;
			float _Tiling02;
			float _Tiling01;
			float _Noise01Power;
			float _Noise02Power;
			float _Softness;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Noise02;
			sampler2D _Noise01;


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord2.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.yzw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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

				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

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

				float eyeDepth = IN.ase_texcoord2.x;
				float cameraDepthFade66 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ViewDistance);
				float DistanceFade124 = ( 1.0 - cameraDepthFade66 );
				float2 appendResult161 = (float2(_DirectionSpeed.z , _DirectionSpeed.w));
				float2 appendResult4 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner159 = ( _TimeParameters.x * appendResult161 + appendResult4);
				float3 temp_cast_0 = (tex2D( _Noise02, ( panner159 * ( _Tiling02 / 10000.0 ) ) ).r).xxx;
				float3 temp_cast_1 = (tex2D( _Noise02, ( panner159 * ( _Tiling02 / 10000.0 ) ) ).r).xxx;
				float3 linearToGamma92 = FastLinearToSRGB( temp_cast_1 );
				float2 appendResult160 = (float2(_DirectionSpeed.x , _DirectionSpeed.y));
				float2 panner157 = ( _TimeParameters.x * appendResult160 + appendResult4);
				float3 temp_cast_2 = (tex2D( _Noise01, ( panner157 * ( _Tiling01 / 10000.0 ) ) ).r).xxx;
				float3 temp_cast_3 = (tex2D( _Noise01, ( panner157 * ( _Tiling01 / 10000.0 ) ) ).r).xxx;
				float3 linearToGamma91 = FastLinearToSRGB( temp_cast_3 );
				float3 blendOpSrc155 = linearToGamma92;
				float3 blendOpDest155 = ( linearToGamma91 * _Noise01Power );
				float3 lerpBlendMode155 = lerp(blendOpDest155,max( blendOpSrc155, blendOpDest155 ),_Noise02Power);
				float3 Noise121 = ( saturate( lerpBlendMode155 ));
				float Coverage211 = _Coverage;
				float CloudHeight130 = ( 1.0 - pow( saturate( ( abs( ( _cloudsPosition - WorldPosition.y ) ) / _cloudsHeight ) ) , ( 1.0 - Coverage211 ) ) );
				float3 temp_cast_4 = (( 1.0 - (0.0 + (Coverage211 - 0.0) * (1.0 - 0.0) / (0.98 - 0.0)) )).xxx;
				float3 temp_cast_5 = (_Softness).xxx;
				float3 CloudCoverage134 = pow( saturate( (float3( 0,0,0 ) + (( Noise121 * CloudHeight130 ) - temp_cast_4) * (float3( 1,0,0 ) - float3( 0,0,0 )) / (float3( 1,0,0 ) - temp_cast_4)) ) , temp_cast_5 );
				
				float Alpha = saturate( ( DistanceFade124 * CloudCoverage134 ) ).x;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

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
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70503

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ScatteringColor;
			float4 _CloudsColor;
			float4 _DirectionSpeed;
			float _cloudsPosition;
			float _cloudsHeight;
			float _Coverage;
			float _ScatteringPower;
			float _ViewDistance;
			float _Tiling02;
			float _Tiling01;
			float _Noise01Power;
			float _Noise02Power;
			float _Softness;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Noise02;
			sampler2D _Noise01;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord2.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.yzw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
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

				float eyeDepth = IN.ase_texcoord2.x;
				float cameraDepthFade66 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ViewDistance);
				float DistanceFade124 = ( 1.0 - cameraDepthFade66 );
				float2 appendResult161 = (float2(_DirectionSpeed.z , _DirectionSpeed.w));
				float2 appendResult4 = (float2(WorldPosition.x , WorldPosition.z));
				float2 panner159 = ( _TimeParameters.x * appendResult161 + appendResult4);
				float3 temp_cast_0 = (tex2D( _Noise02, ( panner159 * ( _Tiling02 / 10000.0 ) ) ).r).xxx;
				float3 temp_cast_1 = (tex2D( _Noise02, ( panner159 * ( _Tiling02 / 10000.0 ) ) ).r).xxx;
				float3 linearToGamma92 = FastLinearToSRGB( temp_cast_1 );
				float2 appendResult160 = (float2(_DirectionSpeed.x , _DirectionSpeed.y));
				float2 panner157 = ( _TimeParameters.x * appendResult160 + appendResult4);
				float3 temp_cast_2 = (tex2D( _Noise01, ( panner157 * ( _Tiling01 / 10000.0 ) ) ).r).xxx;
				float3 temp_cast_3 = (tex2D( _Noise01, ( panner157 * ( _Tiling01 / 10000.0 ) ) ).r).xxx;
				float3 linearToGamma91 = FastLinearToSRGB( temp_cast_3 );
				float3 blendOpSrc155 = linearToGamma92;
				float3 blendOpDest155 = ( linearToGamma91 * _Noise01Power );
				float3 lerpBlendMode155 = lerp(blendOpDest155,max( blendOpSrc155, blendOpDest155 ),_Noise02Power);
				float3 Noise121 = ( saturate( lerpBlendMode155 ));
				float Coverage211 = _Coverage;
				float CloudHeight130 = ( 1.0 - pow( saturate( ( abs( ( _cloudsPosition - WorldPosition.y ) ) / _cloudsHeight ) ) , ( 1.0 - Coverage211 ) ) );
				float3 temp_cast_4 = (( 1.0 - (0.0 + (Coverage211 - 0.0) * (1.0 - 0.0) / (0.98 - 0.0)) )).xxx;
				float3 temp_cast_5 = (_Softness).xxx;
				float3 CloudCoverage134 = pow( saturate( (float3( 0,0,0 ) + (( Noise121 * CloudHeight130 ) - temp_cast_4) * (float3( 1,0,0 ) - float3( 0,0,0 )) / (float3( 1,0,0 ) - temp_cast_4)) ) , temp_cast_5 );
				
				float Alpha = saturate( ( DistanceFade124 * CloudCoverage134 ) ).x;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18900
663;73;907;926;5079.534;3400.566;6.428995;True;False
Node;AmplifyShaderEditor.CommentaryNode;123;-2432,-1792;Inherit;False;2686.682;736.951;;25;121;155;148;150;151;92;91;22;143;2;10;9;147;141;159;142;162;157;25;23;161;4;160;3;158;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;131;-2432,-896;Inherit;False;1409.247;449.601;;12;130;35;34;33;30;31;29;28;27;26;173;213;Clouds Height;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-2400,-1728;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;158;-2400,-1520;Inherit;False;Property;_DirectionSpeed;Direction / Speed;1;0;Create;True;0;0;0;False;0;False;20,5,10,-5;50,5,50,-20;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;4;-2208,-1680;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1792,-1472;Inherit;False;Property;_Tiling01;Tiling;3;0;Create;False;0;0;0;False;0;False;0.02;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;160;-2208,-1536;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;162;-2208,-1152;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2400,-832;Inherit;False;Property;_cloudsPosition;cloudsPosition;13;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;1025;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;-2400,-736;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;136;-2432,-256;Inherit;False;1538.37;354.3396;;13;211;16;134;19;20;18;17;90;37;122;189;132;212;Clouds Coverage;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;157;-2016,-1536;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;161;-2208,-1424;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-1792,-1168;Inherit;False;Property;_Tiling02;Tiling;6;0;Create;False;0;0;0;False;0;False;0.03;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;25;-1600,-1472;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10000;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1536,0;Inherit;False;Property;_Coverage;Coverage;9;0;Create;True;0;0;0;False;0;False;0.3;0.604;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-2176,-784;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;141;-1600,-1168;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10000;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1344,-1536;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;159;-2016,-1232;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;9;-1440,-1744;Inherit;True;Property;_Noise01;Noise 01;2;0;Create;True;0;0;0;False;2;Header(Main);Space(10);False;282028860199a5f49a51216f309f1410;ee42667f1ca696a4fa022b8637a52f89;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.AbsOpNode;29;-2016,-784;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;211;-1088,0;Inherit;False;Coverage;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2064,-624;Inherit;False;Property;_cloudsHeight;cloudsHeight;15;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;500;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-1344,-1232;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;213;-2400,-544;Inherit;False;211;Coverage;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;30;-1856,-704;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1152,-1744;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;147;-1440,-1424;Inherit;True;Property;_Noise02;Noise 02;5;0;Create;True;0;0;0;False;1;Space(30);False;282028860199a5f49a51216f309f1410;ee42667f1ca696a4fa022b8637a52f89;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LinearToGammaNode;91;-768,-1728;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-768,-1632;Inherit;False;Property;_Noise01Power;Power;4;0;Create;False;0;0;0;False;0;False;0.75;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;173;-2064,-528;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1152,-1424;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;33;-1728,-704;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-768,-1312;Inherit;False;Property;_Noise02Power;Power;7;0;Create;False;0;0;0;False;0;False;0.3;0.811;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LinearToGammaNode;92;-768,-1408;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;34;-1568,-640;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-448,-1728;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendOpsNode;155;-256,-1424;Inherit;False;Lighten;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;35;-1408,-640;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;0,-1424;Inherit;False;Noise;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;212;-2400,0;Inherit;False;211;Coverage;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;130;-1248,-640;Inherit;False;CloudHeight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;189;-2112,-80;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.98;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-2400,-96;Inherit;False;130;CloudHeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;-2400,-192;Inherit;False;121;Noise;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;90;-1888,-64;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2208,-192;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;126;-2432,-2176;Inherit;False;891;255;;4;124;67;66;65;Distance Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2384,-2096;Inherit;False;Property;_ViewDistance;View Distance;0;0;Create;True;0;0;0;False;0;False;5000;6000;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;17;-1728,-192;Inherit;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CameraDepthFade;66;-2176,-2112;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1536,-96;Inherit;False;Property;_Softness;Softness;10;0;Create;True;0;0;0;False;0;False;0.25;0.805;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;18;-1536,-192;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;19;-1248,-192;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;67;-1920,-2112;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;124;-1760,-2112;Inherit;False;DistanceFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;134;-1088,-192;Inherit;False;CloudCoverage;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;714.2541,25.66606;Inherit;False;124;DistanceFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;710.2541,109.6661;Inherit;False;134;CloudCoverage;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;219;-2432,896;Inherit;False;1661.602;863.8809;;15;137;210;200;181;199;202;178;192;120;184;193;185;188;179;224;Lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;127;-2432,256;Inherit;False;2301.591;446;;20;223;106;112;108;114;197;117;116;118;119;115;145;110;111;109;107;196;198;201;195;Scattering;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;942.2541,45.66607;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-2080,1568;Inherit;False;Constant;_Float3;Float 3;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;179;-2400,1568;Inherit;False;124;DistanceFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;181;-1696,1328;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-1920,416;Float;False;Constant;_ScatteringSize;5;14;0;Create;False;0;0;0;False;0;False;5;0.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;201;-1376,480;Inherit;False;Constant;_Int0;Int 0;17;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;-1376,368;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-1952,1152;Inherit;False;223;Lerp;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;184;-2080,1664;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;111;-1920,320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-1200,512;Inherit;False;2;2;0;INT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;188;-2400,1664;Inherit;False;Constant;_Float2;Float 2;18;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;115;-1840,624;Inherit;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-384,416;Inherit;False;Scattering;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;185;-2208,1600;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-1536,432;Inherit;False;3;3;0;FLOAT;1;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;112;-1760,480;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.PowerNode;197;-752,512;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;109;-2048,320;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;198;-1056,608;Float;False;Property;_ScatteringPower;Absorption;14;0;Create;False;0;0;0;False;0;False;10;24.2;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;114;-1760,352;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;223;-384,544;Inherit;False;Lerp;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-1440,1216;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-592,368;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;108;-2400,320;Float;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;-1536,320;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;225;-1188.401,876.9905;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-1696,1088;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;178;-1952,1248;Inherit;False;unity_FogColor;0;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;192;-1952,1600;Inherit;False;Property;_UseFog;Use Fog;11;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;202;-1952,1344;Inherit;False;Property;_CloudsColor;Color;12;0;Create;False;0;0;0;False;3;Space(10);Header(Scattering);Space(10);False;0.1843137,0.3568628,0.4627451,1;0.4765931,0.7118332,0.8490566,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;106;-2400,480;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;69;1094.254,45.66607;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;224;-1408,1088;Inherit;False;119;Scattering;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;199;-1952,960;Inherit;False;Property;_ScatteringColor;Color;8;0;Create;False;0;0;0;False;3;Space(10);Header(Clouds);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;107;-2176,480;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;137;-1024,1216;Inherit;False;CloudLighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;1040.254,-37.4339;Inherit;False;137;CloudLighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;-1424,576;Inherit;False;130;CloudHeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;227;1270.543,-32.7339;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Custom/Clouds;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;False;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;  Blend;0;Two Sided;0;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;DOTS Instancing;0;Meta Pass;0;Extra Pre Pass;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;5;False;True;True;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;226;1272.543,-146.7339;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;228;1272.543,-146.7339;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;229;1272.543,-146.7339;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;230;1272.543,-146.7339;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;4;0;3;1
WireConnection;4;1;3;3
WireConnection;160;0;158;1
WireConnection;160;1;158;2
WireConnection;157;0;4;0
WireConnection;157;2;160;0
WireConnection;157;1;162;0
WireConnection;161;0;158;3
WireConnection;161;1;158;4
WireConnection;25;0;23;0
WireConnection;28;0;26;0
WireConnection;28;1;27;2
WireConnection;141;0;142;0
WireConnection;22;0;157;0
WireConnection;22;1;25;0
WireConnection;159;0;4;0
WireConnection;159;2;161;0
WireConnection;159;1;162;0
WireConnection;29;0;28;0
WireConnection;211;0;16;0
WireConnection;143;0;159;0
WireConnection;143;1;141;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;10;0;9;0
WireConnection;10;1;22;0
WireConnection;91;0;10;1
WireConnection;173;0;213;0
WireConnection;2;0;147;0
WireConnection;2;1;143;0
WireConnection;33;0;30;0
WireConnection;92;0;2;1
WireConnection;34;0;33;0
WireConnection;34;1;173;0
WireConnection;148;0;91;0
WireConnection;148;1;150;0
WireConnection;155;0;92;0
WireConnection;155;1;148;0
WireConnection;155;2;151;0
WireConnection;35;0;34;0
WireConnection;121;0;155;0
WireConnection;130;0;35;0
WireConnection;189;0;212;0
WireConnection;90;0;189;0
WireConnection;37;0;122;0
WireConnection;37;1;132;0
WireConnection;17;0;37;0
WireConnection;17;1;90;0
WireConnection;66;0;65;0
WireConnection;18;0;17;0
WireConnection;19;0;18;0
WireConnection;19;1;20;0
WireConnection;67;0;66;0
WireConnection;124;0;67;0
WireConnection;134;0;19;0
WireConnection;68;0;125;0
WireConnection;68;1;135;0
WireConnection;181;0;178;0
WireConnection;181;1;202;0
WireConnection;181;2;192;0
WireConnection;117;0;118;0
WireConnection;117;1;116;0
WireConnection;184;0;185;0
WireConnection;111;0;109;0
WireConnection;196;0;201;0
WireConnection;196;1;195;0
WireConnection;119;0;145;0
WireConnection;185;0;179;0
WireConnection;185;1;188;0
WireConnection;116;0;114;0
WireConnection;116;1;115;0
WireConnection;116;2;112;0
WireConnection;197;0;196;0
WireConnection;197;1;198;0
WireConnection;109;0;108;0
WireConnection;109;1;107;0
WireConnection;114;0;111;0
WireConnection;114;1;110;0
WireConnection;223;0;197;0
WireConnection;210;0;200;0
WireConnection;210;1;181;0
WireConnection;145;0;117;0
WireConnection;145;1;197;0
WireConnection;118;0;112;0
WireConnection;118;1;114;0
WireConnection;225;0;224;0
WireConnection;225;1;210;0
WireConnection;200;0;199;0
WireConnection;200;1;120;0
WireConnection;192;1;193;0
WireConnection;192;0;184;0
WireConnection;69;0;68;0
WireConnection;107;0;106;0
WireConnection;137;0;225;0
WireConnection;227;2;139;0
WireConnection;227;3;69;0
ASEEND*/
//CHKSM=8638749AD7B05F05F1DA89D80B876A041E7A3CCB