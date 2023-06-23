// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/VegetationTrunk"
{
	Properties
	{
		[Header(Main Maps)][Space(10)]_Color("Main Color", Color) = (1,1,1,1)
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
		[Toggle(_WINDDEBUGVIEW_ON)] _WindDebugView("WindDebugView", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature_local _BASEWINDCHANNEL_R _BASEWINDCHANNEL_G _BASEWINDCHANNEL_B _BASEWINDCHANNEL_A
		#pragma shader_feature_local _INVERTMASK_ON
		#pragma shader_feature_local _LAYERCHANNEL_R _LAYERCHANNEL_G _LAYERCHANNEL_B _LAYERCHANNEL_A
		#pragma shader_feature _SEEVERTEXCOLOR_ON
		#pragma shader_feature _WINDDEBUGVIEW_ON
		#pragma shader_feature_local _VERTEXCOLORCHANNEL_RGBA _VERTEXCOLORCHANNEL_R _VERTEXCOLORCHANNEL_G _VERTEXCOLORCHANNEL_B _VERTEXCOLORCHANNEL_A
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
			float4 vertexColor : COLOR;
		};

		uniform float WindSpeed;
		uniform float WindPower;
		uniform float WindBurstsSpeed;
		uniform float WindBurstsScale;
		uniform float WindBurstsPower;
		uniform float _WindTrunkContrast;
		uniform float _WindTrunkPosition;
		uniform float _WindMultiplier;
		uniform float _BlendNormals;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _NormalPower;
		uniform sampler2D _DetailNormalMap;
		uniform float4 _DetailNormalMap_ST;
		uniform float _2ndNormalPower;
		uniform sampler2D _LayerMask;
		uniform float4 _LayerMask_ST;
		uniform float _UseVertexColor;
		uniform float _LayerContrast;
		uniform float _LayerPosition;
		uniform float _LayerPower;
		uniform float _LayerThreshold;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _2ndColor;
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform sampler2D _MetallicROcclusionGSmoothnessA;
		uniform float4 _MetallicROcclusionGSmoothnessA_ST;
		uniform float _MetallicPower;
		uniform sampler2D _DetailMetallicGlossMap;
		uniform float4 _DetailMetallicGlossMap_ST;
		uniform float _LayerMetallicPower;
		uniform float _SmoothnessPower;
		uniform float _LayerSmoothnessPower;
		uniform float _OcclusionPower;
		uniform float _LayerOcclusionPower;


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

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_127_0 = ( _Time.y * WindSpeed );
			float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
			float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
			simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
			float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
			#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = v.color.r;
			#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = v.color.g;
			#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = v.color.b;
			#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = v.color.a;
			#else
				float staticSwitch202 = v.color.b;
			#endif
			float BaseWindColor203 = staticSwitch202;
			float4 temp_cast_0 = (pow( ( 1.0 - BaseWindColor203 ) , _WindTrunkPosition )).xxxx;
			float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_0) );
			float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
			float4 transform183 = mul(unity_WorldToObject,float4( appendResult124 , 0.0 ));
			float4 BaseWind163 = ( transform183 * _WindMultiplier );
			v.vertex.xyz += BaseWind163.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 tex2DNode3 = UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _NormalPower );
			float2 uv_DetailNormalMap = i.uv_texcoord * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
			float3 tex2DNode137 = UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailNormalMap ), _2ndNormalPower );
			float2 uv_LayerMask = i.uv_texcoord * _LayerMask_ST.xy + _LayerMask_ST.zw;
			float4 tex2DNode141 = tex2D( _LayerMask, uv_LayerMask );
			#ifdef _INVERTMASK_ON
				float staticSwitch228 = ( 1.0 - tex2DNode141.r );
			#else
				float staticSwitch228 = tex2DNode141.r;
			#endif
			float4 temp_cast_0 = ((WorldNormalVector( i , tex2DNode137 )).y).xxxx;
			#if defined(_LAYERCHANNEL_R)
				float staticSwitch204 = i.vertexColor.r;
			#elif defined(_LAYERCHANNEL_G)
				float staticSwitch204 = i.vertexColor.g;
			#elif defined(_LAYERCHANNEL_B)
				float staticSwitch204 = i.vertexColor.b;
			#elif defined(_LAYERCHANNEL_A)
				float staticSwitch204 = i.vertexColor.a;
			#else
				float staticSwitch204 = i.vertexColor.g;
			#endif
			float DepositLayerColor205 = staticSwitch204;
			float4 temp_cast_1 = (pow( DepositLayerColor205 , _LayerPosition )).xxxx;
			float4 clampResult105 = clamp( CalculateContrast(_LayerContrast,temp_cast_1) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_cast_2 = (( 1.0 - _LayerPower )).xxxx;
			float4 temp_cast_3 = (_LayerThreshold).xxxx;
			float4 BlendAlpha85 = pow( saturate( ( ( staticSwitch228 + (( _UseVertexColor )?( ( pow( clampResult105 , temp_cast_2 ) * clampResult105 ) ):( temp_cast_0 )) ) + _LayerPower ) ) , temp_cast_3 );
			float3 lerpResult13 = lerp( tex2DNode3 , tex2DNode137 , BlendAlpha85.rgb);
			float4 color81 = IsGammaSpace() ? float4(0.01176471,0,1,1) : float4(0.0009105813,0,1,1);
			float4 lerpResult78 = lerp( color81 , float4( tex2DNode137 , 0.0 ) , BlendAlpha85);
			float3 Normals184 = (( _BlendNormals )?( BlendNormals( tex2DNode3 , lerpResult78.rgb ) ):( lerpResult13 ));
			o.Normal = Normals184;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float4 lerpResult26 = lerp( ( _Color * tex2D( _MainTex, uv_MainTex ) ) , ( _2ndColor * tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap ) ) , BlendAlpha85);
			float temp_output_127_0 = ( _Time.y * WindSpeed );
			float2 appendResult152 = (float2(WindBurstsSpeed , WindBurstsSpeed));
			float3 ase_worldPos = i.worldPos;
			float2 appendResult153 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner150 = ( 1.0 * _Time.y * appendResult152 + appendResult153);
			float simplePerlin2D240 = snoise( panner150*( WindBurstsScale / 100.0 ) );
			simplePerlin2D240 = simplePerlin2D240*0.5 + 0.5;
			float temp_output_148_0 = ( WindPower * ( simplePerlin2D240 * WindBurstsPower ) );
			#if defined(_BASEWINDCHANNEL_R)
				float staticSwitch202 = i.vertexColor.r;
			#elif defined(_BASEWINDCHANNEL_G)
				float staticSwitch202 = i.vertexColor.g;
			#elif defined(_BASEWINDCHANNEL_B)
				float staticSwitch202 = i.vertexColor.b;
			#elif defined(_BASEWINDCHANNEL_A)
				float staticSwitch202 = i.vertexColor.a;
			#else
				float staticSwitch202 = i.vertexColor.b;
			#endif
			float BaseWindColor203 = staticSwitch202;
			float4 temp_cast_7 = (pow( ( 1.0 - BaseWindColor203 ) , _WindTrunkPosition )).xxxx;
			float4 temp_output_177_0 = saturate( CalculateContrast(_WindTrunkContrast,temp_cast_7) );
			float3 appendResult124 = (float3(( ( sin( temp_output_127_0 ) * temp_output_148_0 ) * temp_output_177_0 ).r , 0.0 , ( ( cos( temp_output_127_0 ) * ( temp_output_148_0 * 0.5 ) ) * temp_output_177_0 ).r));
			float4 transform183 = mul(unity_WorldToObject,float4( appendResult124 , 0.0 ));
			float4 BaseWind163 = ( transform183 * _WindMultiplier );
			#ifdef _WINDDEBUGVIEW_ON
				float4 staticSwitch179 = BaseWind163;
			#else
				float4 staticSwitch179 = lerpResult26;
			#endif
			float4 temp_cast_12 = (i.vertexColor.r).xxxx;
			float4 temp_cast_13 = (i.vertexColor.g).xxxx;
			float4 temp_cast_14 = (i.vertexColor.b).xxxx;
			float4 temp_cast_15 = (i.vertexColor.a).xxxx;
			#if defined(_VERTEXCOLORCHANNEL_RGBA)
				float4 staticSwitch224 = i.vertexColor;
			#elif defined(_VERTEXCOLORCHANNEL_R)
				float4 staticSwitch224 = temp_cast_12;
			#elif defined(_VERTEXCOLORCHANNEL_G)
				float4 staticSwitch224 = temp_cast_13;
			#elif defined(_VERTEXCOLORCHANNEL_B)
				float4 staticSwitch224 = temp_cast_14;
			#elif defined(_VERTEXCOLORCHANNEL_A)
				float4 staticSwitch224 = temp_cast_15;
			#else
				float4 staticSwitch224 = i.vertexColor;
			#endif
			#ifdef _SEEVERTEXCOLOR_ON
				float4 staticSwitch209 = staticSwitch224;
			#else
				float4 staticSwitch209 = staticSwitch179;
			#endif
			float4 Albedo187 = staticSwitch209;
			o.Albedo = Albedo187.rgb;
			float2 uv_MetallicROcclusionGSmoothnessA = i.uv_texcoord * _MetallicROcclusionGSmoothnessA_ST.xy + _MetallicROcclusionGSmoothnessA_ST.zw;
			float4 tex2DNode7 = tex2D( _MetallicROcclusionGSmoothnessA, uv_MetallicROcclusionGSmoothnessA );
			float2 uv_DetailMetallicGlossMap = i.uv_texcoord * _DetailMetallicGlossMap_ST.xy + _DetailMetallicGlossMap_ST.zw;
			float4 tex2DNode139 = tex2D( _DetailMetallicGlossMap, uv_DetailMetallicGlossMap );
			float lerpResult30 = lerp( ( tex2DNode7.r * _MetallicPower ) , ( tex2DNode139.r * _LayerMetallicPower ) , BlendAlpha85.r);
			float Metallic192 = lerpResult30;
			o.Metallic = Metallic192;
			float lerpResult31 = lerp( ( tex2DNode7.a * _SmoothnessPower ) , ( tex2DNode139.a * _LayerSmoothnessPower ) , BlendAlpha85.r);
			float Smoothness193 = lerpResult31;
			o.Smoothness = Smoothness193;
			float temp_output_220_0 = pow( tex2DNode7.g , _OcclusionPower );
			float lerpResult33 = lerp( temp_output_220_0 , ( temp_output_220_0 * pow( tex2DNode139.g , _LayerOcclusionPower ) ) , BlendAlpha85.r);
			float Occlusion191 = lerpResult33;
			o.Occlusion = Occlusion191;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
947;73;652;634;3513.358;827.8904;6.217291;True;False
Node;AmplifyShaderEditor.CommentaryNode;208;-3328,2304;Inherit;False;929.6667;543;;7;201;205;203;202;204;200;199;VertexColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;199;-3296,2496;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;204;-2912,2528;Inherit;False;Property;_LayerChannel;Layer Channel;20;0;Create;True;0;0;0;False;0;False;0;1;1;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;159;-3328,1152;Inherit;False;3645.007;1018.856;;36;163;162;161;183;124;120;123;122;177;125;129;115;117;133;116;131;114;148;134;127;121;132;119;126;157;128;158;155;150;152;156;153;154;149;206;240;Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;160;-3328,256;Inherit;False;2925.065;756.1433;;22;35;16;17;25;24;22;73;72;93;110;113;109;112;105;69;141;37;14;85;207;227;228;Blend Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-2624,2528;Inherit;False;DepositLayerColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;207;-3296,448;Inherit;False;205;DepositLayerColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-3296,576;Inherit;False;Property;_LayerPosition;Layer Position;23;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-3280,1728;Inherit;False;Global;WindBurstsSpeed;Wind Bursts Speed;23;0;Create;True;0;0;0;False;1;Space(10);False;50;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;154;-3248,1552;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;156;-3072,1856;Inherit;False;Global;WindBurstsScale;Wind Bursts Scale;24;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;152;-3056,1728;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;153;-3056,1584;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-3296,704;Inherit;False;Property;_LayerContrast;Layer Contrast;24;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;202;-2912,2688;Inherit;False;Property;_BaseWindChannel;Base Wind Channel;26;0;Create;True;0;0;0;False;3;Space(10);Header(Wind);Space(10);False;0;2;2;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;109;-3072,448;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;112;-2816,512;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2816,352;Float;False;Property;_LayerPower;Layer Power;21;0;Create;True;0;0;0;False;0;False;0.5;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;185;-3328,-768;Inherit;False;2150;847;;13;64;9;137;8;3;86;81;78;87;75;13;76;184;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;-2624,2688;Inherit;False;BaseWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;150;-2880,1664;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;155;-2832,1856;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;206;-1920,1824;Inherit;False;203;BaseWindColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;64;-3280,-304;Inherit;True;Property;_DetailNormalMap;Normal;10;0;Create;False;0;0;0;False;0;False;None;9302f85d940c1e24abf248a813b1ef87;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;9;-3280,-96;Inherit;False;Property;_2ndNormalPower;Normal Power;11;0;Create;False;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;69;-2944,800;Inherit;True;Property;_LayerMask;Layer Mask (R);17;0;Create;False;0;0;0;False;0;False;None;e1f9e4b4f78e10041804ffee938870e2;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;73;-2528,320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;105;-2592,656;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;240;-2560,1664;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-2608,1920;Inherit;False;Global;WindBurstsPower;Wind Bursts Power;25;0;Create;True;0;0;0;False;0;False;10;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;137;-2992,-304;Inherit;True;Property;_TextureSample0;Texture Sample 0;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;121;-2496,1328;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;119;-1648,1824;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;141;-2688,800;Inherit;True;Property;_TextureSample4;Texture Sample 4;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;128;-1984,1328;Inherit;False;Global;WindPower;Wind Power;22;0;Create;True;0;0;0;False;0;False;0.01;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-1712,1968;Inherit;False;Property;_WindTrunkPosition;Wind Trunk Position;28;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-2304,1792;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;72;-2400,480;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-2496,1456;Inherit;False;Global;WindSpeed;Wind Speed;21;0;Create;True;0;0;0;False;1;Space(10);False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;131;-1472,1904;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-1504,2064;Inherit;False;Property;_WindTrunkContrast;Wind Trunk Contrast;29;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-1680,1584;Inherit;False;Constant;_Float8;Float 8;18;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-2240,1328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;227;-2368,896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1696,1328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;14;-1920,320;Inherit;True;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2144,640;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-1472,1568;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;133;-1296,1952;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;116;-1984,1200;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;115;-1984,1456;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;228;-2160,816;Inherit;False;Property;_InvertMask;Invert Mask;18;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;35;-1802,588;Inherit;False;Property;_UseVertexColor;Use Vertex Color;19;0;Create;True;0;0;0;False;3;Space(10);Header(Layer);Space(10);False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-1136,1472;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;177;-1088,1952;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-1472,1200;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1535,583;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-880,1216;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-880,1360;Inherit;False;Constant;_Float9;Float 9;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-880,1472;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1376,384;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;186;-3328,-1792;Inherit;False;2144;868;;15;187;179;178;26;12;88;10;1;11;2;138;61;209;210;224;Diffuse / Colors;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;124;-624,1344;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1280,704;Float;False;Property;_LayerThreshold;Layer Threshold;22;0;Create;True;0;0;0;False;0;False;50;50;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-1152,384;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;24;-960,384;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;61;-3264,-1136;Inherit;True;Property;_DetailAlbedoMap;Albedo;9;0;Create;False;0;0;0;False;0;False;None;1bbb6f363f884124aaa1cad8329f78cf;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;161;-384,1536;Inherit;False;Property;_WindMultiplier;Wind Multiplier;27;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;183;-480,1344;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;194;-3328,-3072;Inherit;False;2046.412;1119.21;;26;191;193;192;30;31;33;89;215;217;220;221;90;91;139;7;222;218;223;212;211;66;229;230;231;232;233;Metallic / Smoothness / Occlusion;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;211;-3296,-3008;Inherit;True;Property;_MetallicROcclusionGSmoothnessA;Metallic (R) Occlusion (G) Smoothness (A);4;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;2;-3040,-1744;Inherit;False;Property;_Color;Main Color;0;0;Create;False;0;0;0;False;2;Header(Main Maps);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;66;-3296,-2784;Inherit;True;Property;_DetailMetallicGlossMap;Metallic (R) Occlusion (G) Smoothness (A);13;0;Create;False;0;0;0;False;0;False;None;8d961c07e5665584094383581d95e88e;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;11;-3040,-1328;Inherit;False;Property;_2ndColor;Color;8;0;Create;False;0;0;0;False;3;Space(10);Header(Deposit Maps);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-128,1408;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;1;-3040,-1552;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;0;False;0;False;-1;None;e5ef5502120f8f34c8b43394a9d15cc3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;138;-3040,-1136;Inherit;True;Property;_TextureSample1;Texture Sample 1;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-640,384;Inherit;False;BlendAlpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2656,-1232;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-2656,-1104;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-2944,-3008;Inherit;True;Property;_MetallicGlossMap;Metallic (R) Occlusion (G) Smoothness (A);4;0;Create;False;0;0;0;False;0;False;-1;None;9b90f87fd4161d84cbe99dbd8e9bad23;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;139;-2944,-2784;Inherit;True;Property;_TextureSample2;Texture Sample 2;24;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2656,-1616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-3280,-560;Inherit;False;Property;_NormalPower;Normal Power;3;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-2400,-2368;Inherit;False;Property;_OcclusionPower;Occlusion Power;7;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;163;128,1408;Inherit;False;BaseWind;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;81;-2560,-288;Inherit;False;Constant;_Color0;Color 0;22;0;Create;True;0;0;0;False;0;False;0.01176471,0,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;223;-2400,-2240;Inherit;False;Property;_LayerOcclusionPower;Layer Occlusion Power;16;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-2560,-64;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;26;-2400,-1488;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-2400,-2960;Inherit;False;Property;_MetallicPower;Metallic Power;5;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-2208,-592;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-2400,-2864;Inherit;False;Property;_LayerMetallicPower;Layer Metallic Power;14;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;221;-2016,-2240;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;78;-2256,-176;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-2992,-560;Inherit;True;Property;_BumpMap;Normal;2;0;Create;False;0;0;0;False;0;False;-1;None;df89806e8af9bd243ad7d2756c2bc349;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;210;-2048,-1152;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;178;-2336,-1232;Inherit;False;163;BaseWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;220;-2016,-2368;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;212;-2400,-2752;Inherit;False;Property;_SmoothnessPower;Smoothness Power;6;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;-2400,-2624;Inherit;False;Property;_LayerSmoothnessPower;Layer Smoothness Power;15;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;224;-1792,-1152;Inherit;False;Property;_VertexColorChannel;Vertex Color Channel;31;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;5;RGBA;R;G;B;A;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-2016,-2496;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;13;-1952,-720;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-2016,-2624;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;179;-2080,-1360;Inherit;False;Property;_WindDebugView;WindDebugView;32;0;Create;False;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-2112,-3008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-1952,-2864;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;233;-2112,-2880;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-2016,-2752;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;-1904,-2128;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;75;-1952,-336;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;-1856,-2240;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;-1760,-2624;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;76;-1616,-464;Inherit;False;Property;_BlendNormals;Blend Normals;12;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;30;-1744,-3008;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1712,-2320;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;209;-1760,-1360;Inherit;False;Property;_SeeVertexColor;See Vertex Color;30;0;Create;True;0;0;0;False;3;Space(10);Header(Debug);Space(10);False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;191;-1472,-2320;Inherit;False;Occlusion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;193;-1504,-2624;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-1408,-464;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;192;-1488,-3008;Inherit;False;Metallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;187;-1376,-1360;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;196;639.2126,607.9427;Inherit;False;193;Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;638.9548,507.2086;Inherit;False;192;Metallic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;639.2126,799.9426;Inherit;False;163;BaseWind;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;639.2126,255.9426;Inherit;False;187;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;200;-2912,2368;Inherit;False;Property;_MicroWindChannel;Micro Wind Channel;25;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;639.2126,703.9426;Inherit;False;191;Occlusion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;639.2126,383.9426;Inherit;False;184;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;201;-2624,2368;Inherit;False;MicroWindColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;953.9794,386.8527;Float;False;True;-1;2;;0;0;Standard;Custom/VegetationTrunk;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;204;1;199;1
WireConnection;204;0;199;2
WireConnection;204;2;199;3
WireConnection;204;3;199;4
WireConnection;205;0;204;0
WireConnection;152;0;149;0
WireConnection;152;1;149;0
WireConnection;153;0;154;1
WireConnection;153;1;154;3
WireConnection;202;1;199;1
WireConnection;202;0;199;2
WireConnection;202;2;199;3
WireConnection;202;3;199;4
WireConnection;109;0;207;0
WireConnection;109;1;110;0
WireConnection;112;1;109;0
WireConnection;112;0;113;0
WireConnection;203;0;202;0
WireConnection;150;0;153;0
WireConnection;150;2;152;0
WireConnection;155;0;156;0
WireConnection;73;0;22;0
WireConnection;105;0;112;0
WireConnection;240;0;150;0
WireConnection;240;1;155;0
WireConnection;137;0;64;0
WireConnection;137;5;9;0
WireConnection;119;0;206;0
WireConnection;141;0;69;0
WireConnection;157;0;240;0
WireConnection;157;1;158;0
WireConnection;72;0;105;0
WireConnection;72;1;73;0
WireConnection;131;0;119;0
WireConnection;131;1;132;0
WireConnection;127;0;121;0
WireConnection;127;1;126;0
WireConnection;227;0;141;1
WireConnection;148;0;128;0
WireConnection;148;1;157;0
WireConnection;14;0;137;0
WireConnection;93;0;72;0
WireConnection;93;1;105;0
WireConnection;117;0;148;0
WireConnection;117;1;114;0
WireConnection;133;1;131;0
WireConnection;133;0;134;0
WireConnection;116;0;127;0
WireConnection;115;0;127;0
WireConnection;228;1;141;1
WireConnection;228;0;227;0
WireConnection;35;0;14;2
WireConnection;35;1;93;0
WireConnection;129;0;115;0
WireConnection;129;1;117;0
WireConnection;177;0;133;0
WireConnection;125;0;116;0
WireConnection;125;1;148;0
WireConnection;37;0;228;0
WireConnection;37;1;35;0
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
WireConnection;12;0;11;0
WireConnection;12;1;138;0
WireConnection;7;0;211;0
WireConnection;139;0;66;0
WireConnection;10;0;2;0
WireConnection;10;1;1;0
WireConnection;163;0;162;0
WireConnection;26;0;10;0
WireConnection;26;1;12;0
WireConnection;26;2;88;0
WireConnection;221;0;139;2
WireConnection;221;1;223;0
WireConnection;78;0;81;0
WireConnection;78;1;137;0
WireConnection;78;2;86;0
WireConnection;3;5;8;0
WireConnection;220;0;7;2
WireConnection;220;1;222;0
WireConnection;224;1;210;0
WireConnection;224;0;210;1
WireConnection;224;2;210;2
WireConnection;224;3;210;3
WireConnection;224;4;210;4
WireConnection;13;0;3;0
WireConnection;13;1;137;0
WireConnection;13;2;87;0
WireConnection;217;0;139;4
WireConnection;217;1;218;0
WireConnection;179;1;26;0
WireConnection;179;0;178;0
WireConnection;232;0;7;1
WireConnection;232;1;230;0
WireConnection;233;0;139;1
WireConnection;233;1;231;0
WireConnection;215;0;7;4
WireConnection;215;1;212;0
WireConnection;75;0;3;0
WireConnection;75;1;78;0
WireConnection;229;0;220;0
WireConnection;229;1;221;0
WireConnection;31;0;215;0
WireConnection;31;1;217;0
WireConnection;31;2;90;0
WireConnection;76;0;13;0
WireConnection;76;1;75;0
WireConnection;30;0;232;0
WireConnection;30;1;233;0
WireConnection;30;2;89;0
WireConnection;33;0;220;0
WireConnection;33;1;229;0
WireConnection;33;2;91;0
WireConnection;209;1;179;0
WireConnection;209;0;224;0
WireConnection;191;0;33;0
WireConnection;193;0;31;0
WireConnection;184;0;76;0
WireConnection;192;0;30;0
WireConnection;187;0;209;0
WireConnection;200;1;199;1
WireConnection;200;0;199;2
WireConnection;200;2;199;3
WireConnection;200;3;199;4
WireConnection;201;0;200;0
WireConnection;0;0;188;0
WireConnection;0;1;189;0
WireConnection;0;3;195;0
WireConnection;0;4;196;0
WireConnection;0;5;197;0
WireConnection;0;11;164;0
ASEEND*/
//CHKSM=BE727E551F40F34936A65C784F951C64C417E12C