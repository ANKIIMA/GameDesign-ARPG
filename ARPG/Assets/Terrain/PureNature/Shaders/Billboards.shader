// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BK/Billboards"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Header(Main Maps)][Space(10)]_MainColor("Leaves Color", Color) = (1,1,1,0)
		_TrunkColor("Trunk Color", Color) = (0,0,0,0)
		_DetailColor("Detail Color", Color) = (1,1,1,1)
		_ColorID("Color ID", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_NormalPower("Normal Power", Range( 0 , 1)) = 1
		[Space(10)][Header(Gradient Parameters)][Space(10)]_GradientColor("Gradient Color", Color) = (1,1,1,0)
		_GradientFalloff("Gradient Falloff", Range( 0 , 2)) = 2
		_GradientPosition("Gradient Position", Range( 0 , 1)) = 0.5
		[Toggle(_INVERTGRADIENT_ON)] _InvertGradient("Invert Gradient", Float) = 0
		[Space(10)][Header(Color Variation)][Space(10)]_ColorVariation("Color Variation", Color) = (1,0,0,0)
		_ColorVariationPower("Color Variation Power", Range( 0 , 1)) = 1
		_ColorVariationNoise("Color Variation Noise", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 0.5
		[Space(10)][Header(Wind)][Space(10)]_WindMultiplier("Wind Multiplier", Float) = 0
		[Toggle(_WINDDEBUGVIEW_ON)] _WindDebugView("WindDebugView", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TreeBillboard"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 4.5
		#pragma multi_compile_instancing
		#pragma shader_feature _WINDDEBUGVIEW_ON
		#pragma shader_feature_local _INVERTGRADIENT_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float WindSpeed;
		uniform float WindPower;
		uniform float WindBurstsSpeed;
		uniform float WindBurstsScale;
		uniform float WindBurstsPower;
		uniform float _WindMultiplier;
		uniform sampler2D _ColorID;
		uniform float4 _ColorID_ST;
		uniform float4 _DetailColor;
		uniform float4 _MainColor;
		uniform float4 _GradientColor;
		uniform float _GradientPosition;
		uniform float _GradientFalloff;
		uniform float4 _ColorVariation;
		uniform float _ColorVariationPower;
		uniform sampler2D _ColorVariationNoise;
		uniform float _NoiseScale;
		uniform float4 _TrunkColor;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float _NormalPower;
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_85_0 = ( _Time.y * WindSpeed );
			float2 appendResult76 = (float2(WindBurstsSpeed , WindBurstsSpeed));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult75 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner77 = ( 1.0 * _Time.y * appendResult76 + appendResult75);
			float simplePerlin2D80 = snoise( panner77*( WindBurstsScale / 100.0 ) );
			simplePerlin2D80 = simplePerlin2D80*0.5 + 0.5;
			float temp_output_86_0 = ( WindPower * ( simplePerlin2D80 * WindBurstsPower ) );
			float3 appendResult98 = (float3(( ( sin( temp_output_85_0 ) * temp_output_86_0 ) * v.texcoord.xy.y ) , 0.0 , ( ( cos( temp_output_85_0 ) * ( temp_output_86_0 * 0.5 ) ) * v.texcoord.xy.y )));
			float3 BaseWind99 = appendResult98;
			v.vertex.xyz += ( BaseWind99 * _WindMultiplier );
			v.vertex.w = 1;
			//Calculate new billboard vertex position and normal;
			float3 upCamVec = float3( 0, 1, 0 );
			float3 forwardCamVec = -normalize ( UNITY_MATRIX_V._m20_m21_m22 );
			float3 rightCamVec = normalize( UNITY_MATRIX_V._m00_m01_m02 );
			float4x4 rotationCamMatrix = float4x4( rightCamVec, 0, upCamVec, 0, forwardCamVec, 0, 0, 0, 0, 1 );
			v.normal = normalize( mul( float4( v.normal , 0 ), rotationCamMatrix )).xyz;
			v.vertex.x *= length( unity_ObjectToWorld._m00_m10_m20 );
			v.vertex.y *= length( unity_ObjectToWorld._m01_m11_m21 );
			v.vertex.z *= length( unity_ObjectToWorld._m02_m12_m22 );
			v.vertex = mul( v.vertex, rotationCamMatrix );
			v.vertex.xyz += unity_ObjectToWorld._m03_m13_m23;
			//Need to nullify rotation inserted by generated surface shader;
			v.vertex = mul( unity_WorldToObject, v.vertex );
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_ColorID = i.uv_texcoord * _ColorID_ST.xy + _ColorID_ST.zw;
			float4 ColorID146 = tex2D( _ColorID, uv_ColorID );
			#ifdef _INVERTGRADIENT_ON
				float staticSwitch165 = i.uv_texcoord.y;
			#else
				float staticSwitch165 = ( 1.0 - i.uv_texcoord.y );
			#endif
			float clampResult25 = clamp( ( ( staticSwitch165 + (-2.0 + (_GradientPosition - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) / _GradientFalloff ) , 0.0 , 1.0 );
			float4 lerpResult28 = lerp( _MainColor , _GradientColor , clampResult25);
			float4 blendOpSrc32 = lerpResult28;
			float4 blendOpDest32 = _ColorVariation;
			float4 lerpBlendMode32 = lerp(blendOpDest32,( blendOpDest32/ max( 1.0 - blendOpSrc32, 0.00001 ) ),_ColorVariationPower);
			float3 ase_worldPos = i.worldPos;
			float2 appendResult22 = (float2(ase_worldPos.x , ase_worldPos.z));
			float4 lerpResult34 = lerp( lerpResult28 , ( saturate( lerpBlendMode32 )) , ( _ColorVariationPower * pow( tex2D( _ColorVariationNoise, ( appendResult22 * ( _NoiseScale / 100.0 ) ) ).r , 3.0 ) ));
			float temp_output_85_0 = ( _Time.y * WindSpeed );
			float2 appendResult76 = (float2(WindBurstsSpeed , WindBurstsSpeed));
			float2 appendResult75 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner77 = ( 1.0 * _Time.y * appendResult76 + appendResult75);
			float simplePerlin2D80 = snoise( panner77*( WindBurstsScale / 100.0 ) );
			simplePerlin2D80 = simplePerlin2D80*0.5 + 0.5;
			float temp_output_86_0 = ( WindPower * ( simplePerlin2D80 * WindBurstsPower ) );
			float3 appendResult98 = (float3(( ( sin( temp_output_85_0 ) * temp_output_86_0 ) * i.uv_texcoord.y ) , 0.0 , ( ( cos( temp_output_85_0 ) * ( temp_output_86_0 * 0.5 ) ) * i.uv_texcoord.y )));
			float3 BaseWind99 = appendResult98;
			#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch103 = float4( BaseWind99 , 0.0 );
			#else
				float4 staticSwitch103 = ( ( _DetailColor * (ColorID146).b ) + ( ( lerpResult34 * (ColorID146).g ) + ( _TrunkColor * (ColorID146).r ) ) );
			#endif
			float4 Diffuse145 = staticSwitch103;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode6 = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalPower );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 worldToViewDir52 = mul( UNITY_MATRIX_V, float4( ase_worldlightDir, 0 ) ).xyz;
			float dotResult46 = dot( tex2DNode6 , worldToViewDir52 );
			float4 appendResult48 = (float4(dotResult46 , dotResult46 , dotResult46 , 1.0));
			float4 VertexNormals134 = saturate( appendResult48 );
			float4 clampResult178 = clamp( ( ( ase_lightAtten * VertexNormals134 ) + 0.25 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float3 Normals170 = tex2DNode6;
			UnityGI gi55 = gi;
			float3 diffNorm55 = WorldNormalVector( i , Normals170 );
			gi55 = UnityGI_Base( data, 1, diffNorm55 );
			float3 indirectDiffuse55 = gi55.indirect.diffuse + diffNorm55 * 0.0001;
			float4 CustomLighting144 = ( ( ase_lightColor * clampResult178 ) + float4( indirectDiffuse55 , 0.0 ) );
			c.rgb = ( Diffuse145 * CustomLighting144 ).rgb;
			c.a = 1;
			clip( (ColorID146).a - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows noforwardadd vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.5
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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
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
1114;81;480;623;6481.113;4312.158;8.126493;True;False
Node;AmplifyShaderEditor.CommentaryNode;71;-3456,128;Inherit;False;2972.731;896.0708;;27;98;97;96;95;94;92;91;90;89;87;86;85;84;83;82;81;80;79;78;77;76;75;74;73;72;101;99;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;141;-3456,-2048;Inherit;False;4218.669;1540.058;;39;8;11;10;145;103;102;149;148;9;154;156;34;147;33;32;31;30;29;28;27;25;24;23;21;22;20;17;18;19;16;15;14;36;13;158;159;165;188;187;Diffuse / Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;136;-3456,1152;Inherit;False;1492.594;532.9998;;10;47;6;52;50;46;48;53;134;170;186;Vertex Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;72;-3408,544;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-3392,-992;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;73;-3408,736;Inherit;False;Global;WindBurstsSpeed;Wind Bursts Speed;22;1;[HideInInspector];Create;True;0;0;0;False;0;False;50;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;76;-3184,720;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;15;-3168,-992;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;47;-3408,1456;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;186;-3280,1376;Inherit;False;Property;_NormalPower;Normal Power;6;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;75;-3216,576;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-3232,-784;Float;False;Property;_GradientPosition;Gradient Position;9;0;Create;True;0;0;0;False;0;False;0.5;0.682;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-3024,800;Inherit;False;Global;WindBurstsScale;Wind Bursts Scale;23;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;14;-2944,-800;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;19;-3408,-1744;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;16;-3328,-1472;Inherit;False;Property;_NoiseScale;Noise Scale;14;0;Create;True;0;0;0;False;0;False;0.5;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;165;-3008,-976;Inherit;False;Property;_InvertGradient;Invert Gradient;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;78;-2768,800;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;52;-3152,1456;Inherit;False;World;View;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;6;-3280,1200;Inherit;True;Property;_Normal;Normal;5;0;Create;True;0;0;0;False;0;False;-1;None;3e33323b1eef59d41b0febf544b8ad12;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;77;-3024,640;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;46;-2896,1328;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2832,1568;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;80;-2608,640;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;21;-3168,-1472;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-2704,-864;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-3168,-1696;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2752,-608;Float;False;Property;_GradientFalloff;Gradient Falloff;8;0;Create;True;0;0;0;False;0;False;2;0.5;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2624,912;Inherit;False;Global;WindBurstsPower;Wind Bursts Power;24;1;[HideInInspector];Create;True;0;0;0;False;0;False;10;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;48;-2640,1328;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;189;-3455,-2432;Inherit;False;581;285;;2;146;5;ID map;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;20;-2448,-752;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2496,432;Inherit;False;Global;WindSpeed;Wind Speed;20;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2272,736;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;82;-2496,304;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2080,320;Inherit;False;Global;WindPower;Wind Power;21;1;[HideInInspector];Create;True;0;0;0;False;0;False;0.01;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-3024,-1536;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;25;-2304,-752;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-2512,-1168;Float;False;Property;_GradientColor;Gradient Color;7;0;Create;True;0;0;0;False;3;Space(10);Header(Gradient Parameters);Space(10);False;1,1,1,0;0.3235203,0.5754716,0.1248664,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;-1984,560;Inherit;False;Constant;_Float8;Float 8;18;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1888,304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2816,-1440;Inherit;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-2368,1408;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;188;-2912,-1728;Inherit;True;Property;_ColorVariationNoise;Color Variation Noise;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-3423,-2368;Inherit;True;Property;_ColorID;Color ID;4;0;Create;True;0;0;0;False;0;False;-1;None;8821829e9fe2d2d4291ed537ba980085;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-2240,304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-2512,-1360;Float;False;Property;_MainColor;Leaves Color;1;0;Create;False;0;0;0;False;2;Header(Main Maps);Space(10);False;1,1,1,0;0.5042214,0.8113207,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosOpNode;90;-1984,432;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;134;-2192,1408;Inherit;False;VertexNormals;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;140;-3456,-384;Inherit;False;1665.286;426.6389;;12;144;56;138;55;137;178;171;176;177;163;135;162;Lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2640,-1808;Inherit;False;Property;_ColorVariationPower;Color Variation Power;12;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;30;-2608,-1648;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-2576,-2000;Inherit;False;Property;_ColorVariation;Color Variation;11;0;Create;True;0;0;0;False;3;Space(10);Header(Color Variation);Space(10);False;1,0,0,0;0.1103595,0.2924527,0.2567373,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;28;-2128,-1264;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-1728,496;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;91;-1984,176;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;146;-3071,-2368;Inherit;False;ColorID;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2320,-1680;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-1728,176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;32;-1872,-2000;Inherit;True;ColorDodge;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;101;-1664,288;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;147;-1584,-1040;Inherit;False;146;ColorID;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-1472,432;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;-3424,-112;Inherit;False;134;VertexNormals;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LightAttenuation;162;-3424,-208;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-3200,-48;Inherit;False;Constant;_Float3;Float 3;17;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;34;-1280,-1728;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1152,672;Inherit;False;Constant;_Float2;Float 2;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-1216,192;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;149;-1280,-1408;Inherit;False;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-1216,432;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-3200,-176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;148;-1280,-1088;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-1280,-1280;Inherit;False;Property;_TrunkColor;Trunk Color;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.8584906,0.8584906,0.8584906,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;176;-3024,-128;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;98;-960,304;Inherit;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-896,-1536;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1056,-1104;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;154;-1280,-768;Inherit;False;False;False;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;156;-1280,-976;Inherit;False;Property;_DetailColor;Detail Color;3;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;170;-2896,1200;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-1056,-784;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-688,320;Inherit;False;BaseWind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;137;-3424,-336;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;178;-2880,-128;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;171;-2688,-160;Inherit;False;170;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-768,-1280;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;55;-2432,-160;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-16,-1072;Inherit;False;99;BaseWind;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-2720,-336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-640,-1152;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;103;224,-1152;Inherit;False;Property;_WindDebugView;WindDebugView;16;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-2176,-336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;145;512,-1152;Inherit;False;Diffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-2016,-336;Inherit;False;CustomLighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;384,96;Inherit;False;144;CustomLighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;105;384,384;Inherit;False;Property;_WindMultiplier;Wind Multiplier;15;0;Create;True;0;0;0;False;3;Space(10);Header(Wind);Space(10);False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;151;384,-128;Inherit;False;146;ColorID;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;384,256;Inherit;False;99;BaseWind;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;384,0;Inherit;False;145;Diffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;150;640,-128;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;640,32;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;640,304;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;896,-128;Float;False;True;-1;5;;0;0;CustomLighting;BK/Billboards;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TreeBillboard;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;True;Cylindrical;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;76;0;73;0
WireConnection;76;1;73;0
WireConnection;15;0;36;2
WireConnection;75;0;72;1
WireConnection;75;1;72;3
WireConnection;14;0;13;0
WireConnection;165;1;15;0
WireConnection;165;0;36;2
WireConnection;78;0;74;0
WireConnection;52;0;47;0
WireConnection;6;5;186;0
WireConnection;77;0;75;0
WireConnection;77;2;76;0
WireConnection;46;0;6;0
WireConnection;46;1;52;0
WireConnection;80;0;77;0
WireConnection;80;1;78;0
WireConnection;21;0;16;0
WireConnection;18;0;165;0
WireConnection;18;1;14;0
WireConnection;22;0;19;1
WireConnection;22;1;19;3
WireConnection;48;0;46;0
WireConnection;48;1;46;0
WireConnection;48;2;46;0
WireConnection;48;3;50;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;84;0;80;0
WireConnection;84;1;79;0
WireConnection;187;0;22;0
WireConnection;187;1;21;0
WireConnection;25;0;20;0
WireConnection;86;0;81;0
WireConnection;86;1;84;0
WireConnection;53;0;48;0
WireConnection;188;1;187;0
WireConnection;85;0;82;0
WireConnection;85;1;83;0
WireConnection;90;0;85;0
WireConnection;134;0;53;0
WireConnection;30;0;188;1
WireConnection;30;1;27;0
WireConnection;28;0;24;0
WireConnection;28;1;23;0
WireConnection;28;2;25;0
WireConnection;89;0;86;0
WireConnection;89;1;87;0
WireConnection;91;0;85;0
WireConnection;146;0;5;0
WireConnection;33;0;29;0
WireConnection;33;1;30;0
WireConnection;94;0;91;0
WireConnection;94;1;86;0
WireConnection;32;0;28;0
WireConnection;32;1;31;0
WireConnection;32;2;29;0
WireConnection;92;0;90;0
WireConnection;92;1;89;0
WireConnection;34;0;28;0
WireConnection;34;1;32;0
WireConnection;34;2;33;0
WireConnection;97;0;94;0
WireConnection;97;1;101;2
WireConnection;149;0;147;0
WireConnection;96;0;92;0
WireConnection;96;1;101;2
WireConnection;163;0;162;0
WireConnection;163;1;135;0
WireConnection;148;0;147;0
WireConnection;176;0;163;0
WireConnection;176;1;177;0
WireConnection;98;0;97;0
WireConnection;98;1;95;0
WireConnection;98;2;96;0
WireConnection;8;0;34;0
WireConnection;8;1;149;0
WireConnection;10;0;9;0
WireConnection;10;1;148;0
WireConnection;154;0;147;0
WireConnection;170;0;6;0
WireConnection;159;0;156;0
WireConnection;159;1;154;0
WireConnection;99;0;98;0
WireConnection;178;0;176;0
WireConnection;11;0;8;0
WireConnection;11;1;10;0
WireConnection;55;0;171;0
WireConnection;138;0;137;0
WireConnection;138;1;178;0
WireConnection;158;0;159;0
WireConnection;158;1;11;0
WireConnection;103;1;158;0
WireConnection;103;0;102;0
WireConnection;56;0;138;0
WireConnection;56;1;55;0
WireConnection;145;0;103;0
WireConnection;144;0;56;0
WireConnection;150;0;151;0
WireConnection;54;0;153;0
WireConnection;54;1;152;0
WireConnection;104;0;100;0
WireConnection;104;1;105;0
WireConnection;0;10;150;0
WireConnection;0;13;54;0
WireConnection;0;11;104;0
ASEEND*/
//CHKSM=FC3211B6B6E905D0A3A1060655ED5C8A8C360269