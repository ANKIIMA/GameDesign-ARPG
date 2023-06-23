// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/StandardLayered"
{
	Properties
	{
		[Header(Mesh Maps)][Space(10)]_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump" {}
		_NormalPower("Normal Power", Range( 0 , 1)) = 1
		_MetallicGlossMap("Metallic (R) Occlusion (G) Smoothness (A)", 2D) = "black" {}
		_MetallicPower("Metallic Power", Range( 0 , 1)) = 0
		_SmoothnessPower("Smoothness Power", Range( 0 , 1)) = 0
		_OcclusionPower("Occlusion Power", Range( 0 , 1)) = 0
		[Space(10)][Header(Second Maps)][Space(10)]_SecondAlbedo("Albedo", 2D) = "white" {}
		_DetailNormalMap1("Normal", 2D) = "bump" {}
		_SecondNormalPower("Normal Power", Range( 0 , 1)) = 1
		_DetailNormalMap2("Metallic (R) Occlusion (G) Smoothness (A)", 2D) = "bump" {}
		_Tiling2("Tiling", Float) = 1
		[Space(10)][Header(Layer)][Space(10)][Toggle]_UseVertexColor("Use Vertex Color", Float) = 0
		[KeywordEnum(R,G,B,A)] _VertexColorChannel("Vertex Color Channel", Float) = 2
		[Space(10)]_LayerPower("Layer Power", Range( 0 , 1)) = 0.5
		_LayerThreshold("Layer Threshold", Range( 0 , 50)) = 50
		_LayerPosition("Layer Position", Float) = 0
		_LayerContrast("Layer Contrast", Float) = 0
		[Space(10)][Toggle]_BlendNormals("Blend Normals", Float) = 1
		[Space(10)][Header(Layer Maps)][Space(10)]_2ndColor("Color", Color) = (1,1,1,1)
		_DetailAlbedoMap("Albedo", 2D) = "white" {}
		_DetailNormalMap("Normal", 2D) = "bump" {}
		_2ndNormalPower("Normal Power", Range( 0 , 1)) = 1
		_DetailMetallicGlossMap("Metallic (R) Occlusion (G) Smoothness (A)", 2D) = "black" {}
		_Tiling("Tiling", Float) = 1
		[Toggle(_SEEVERTEXCOLORS_ON)] _SeeVertexColors("See Vertex Colors", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 5.0
		#pragma shader_feature_local _VERTEXCOLORCHANNEL_R _VERTEXCOLORCHANNEL_G _VERTEXCOLORCHANNEL_B _VERTEXCOLORCHANNEL_A
		#pragma shader_feature_local _SEEVERTEXCOLORS_ON
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
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 vertexColor : COLOR;
		};

		uniform float _BlendNormals;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _NormalPower;
		uniform sampler2D _DetailNormalMap1;
		uniform float _Tiling2;
		uniform float _SecondNormalPower;
		uniform sampler2D _DetailNormalMap;
		uniform float _Tiling;
		uniform float _2ndNormalPower;
		uniform float _UseVertexColor;
		uniform float _LayerContrast;
		uniform float _LayerPosition;
		uniform float _LayerPower;
		uniform float _LayerThreshold;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _SecondAlbedo;
		uniform float4 _2ndColor;
		uniform sampler2D _DetailAlbedoMap;
		uniform sampler2D _MetallicGlossMap;
		uniform float4 _MetallicGlossMap_ST;
		uniform sampler2D _DetailNormalMap2;
		uniform sampler2D _DetailMetallicGlossMap;
		uniform float _MetallicPower;
		uniform float _SmoothnessPower;
		uniform float _OcclusionPower;


		inline float3 TriplanarSampling150( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			xNorm.xyz  = half3( UnpackScaleNormal( xNorm, normalScale.y ).xy * float2(  nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz  = half3( UnpackScaleNormal( yNorm, normalScale.x ).xy * float2(  nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz  = half3( UnpackScaleNormal( zNorm, normalScale.y ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float3 TriplanarSampling63( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			xNorm.xyz  = half3( UnpackScaleNormal( xNorm, normalScale.y ).xy * float2(  nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz  = half3( UnpackScaleNormal( yNorm, normalScale.x ).xy * float2(  nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz  = half3( UnpackScaleNormal( zNorm, normalScale.y ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		inline float4 TriplanarSampling148( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling60( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling152( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling65( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float2 appendResult146 = (float2(_Tiling2 , _Tiling2));
			float2 WorldSpaceSecondMaps147 = appendResult146;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 triplanar150 = TriplanarSampling150( _DetailNormalMap1, ase_worldPos, ase_worldNormal, 1.0, WorldSpaceSecondMaps147, _SecondNormalPower, 0 );
			float3 tanTriplanarNormal150 = mul( ase_worldToTangent, triplanar150 );
			float3 temp_output_138_0 = BlendNormals( UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _NormalPower ) , tanTriplanarNormal150 );
			float2 appendResult71 = (float2(_Tiling , _Tiling));
			float2 WorldSpaceDeposit48 = appendResult71;
			float3 triplanar63 = TriplanarSampling63( _DetailNormalMap, ase_worldPos, ase_worldNormal, 1.0, WorldSpaceDeposit48, _2ndNormalPower, 0 );
			float3 tanTriplanarNormal63 = mul( ase_worldToTangent, triplanar63 );
			float4 temp_cast_0 = ((WorldNormalVector( i , tanTriplanarNormal63 )).y).xxxx;
			#if defined(_VERTEXCOLORCHANNEL_R)
				float staticSwitch162 = i.vertexColor.r;
			#elif defined(_VERTEXCOLORCHANNEL_G)
				float staticSwitch162 = i.vertexColor.g;
			#elif defined(_VERTEXCOLORCHANNEL_B)
				float staticSwitch162 = i.vertexColor.b;
			#elif defined(_VERTEXCOLORCHANNEL_A)
				float staticSwitch162 = i.vertexColor.a;
			#else
				float staticSwitch162 = i.vertexColor.b;
			#endif
			float4 temp_cast_1 = (pow( staticSwitch162 , _LayerPosition )).xxxx;
			float4 clampResult105 = clamp( CalculateContrast(_LayerContrast,temp_cast_1) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_cast_2 = (( 1.0 - _LayerPower )).xxxx;
			float4 temp_cast_3 = ((0.001 + (_LayerThreshold - 0.0) * (1.0 - 0.001) / (1.0 - 0.0))).xxxx;
			float4 BlendAlpha85 = pow( saturate( ( (( _UseVertexColor )?( ( pow( clampResult105 , temp_cast_2 ) * clampResult105 ) ):( temp_cast_0 )) + _LayerPower ) ) , temp_cast_3 );
			float3 lerpResult13 = lerp( temp_output_138_0 , tanTriplanarNormal63 , BlendAlpha85.rgb);
			float4 color81 = IsGammaSpace() ? float4(0.01176471,0,1,1) : float4(0.0009105813,0,1,1);
			float4 lerpResult78 = lerp( color81 , float4( tanTriplanarNormal63 , 0.0 ) , BlendAlpha85);
			float3 Normal118 = (( _BlendNormals )?( BlendNormals( temp_output_138_0 , lerpResult78.rgb ) ):( lerpResult13 ));
			o.Normal = Normal118;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 triplanar148 = TriplanarSampling148( _SecondAlbedo, ase_worldPos, ase_worldNormal, 1.0, WorldSpaceSecondMaps147, 1.0, 0 );
			float4 triplanar60 = TriplanarSampling60( _DetailAlbedoMap, ase_worldPos, ase_worldNormal, 1.0, WorldSpaceDeposit48, 1.0, 0 );
			float4 lerpResult26 = lerp( ( _Color * ( tex2D( _MainTex, uv_MainTex ) * triplanar148 ) ) , ( _2ndColor * triplanar60 ) , BlendAlpha85);
			#ifdef _SEEVERTEXCOLORS_ON
				float4 staticSwitch165 = i.vertexColor;
			#else
				float4 staticSwitch165 = lerpResult26;
			#endif
			float4 Albedo116 = staticSwitch165;
			o.Albedo = Albedo116.rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			float4 tex2DNode7 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
			float4 triplanar152 = TriplanarSampling152( _DetailNormalMap2, ase_worldPos, ase_worldNormal, 1.0, WorldSpaceSecondMaps147, 1.0, 0 );
			float4 triplanar65 = TriplanarSampling65( _DetailMetallicGlossMap, ase_worldPos, ase_worldNormal, 1.0, WorldSpaceDeposit48, 1.0, 0 );
			float lerpResult30 = lerp( ( tex2DNode7.r * triplanar152.x ) , triplanar65.x , BlendAlpha85.r);
			float Metallic120 = ( lerpResult30 + _MetallicPower );
			o.Metallic = Metallic120;
			float lerpResult31 = lerp( ( tex2DNode7.a * triplanar152.w ) , triplanar65.w , BlendAlpha85.r);
			float Smoothness121 = ( lerpResult31 * _SmoothnessPower );
			o.Smoothness = Smoothness121;
			float lerpResult33 = lerp( ( tex2DNode7.g * triplanar152.y ) , triplanar65.y , BlendAlpha85.r);
			float Occlusion122 = pow( lerpResult33 , _OcclusionPower );
			o.Occlusion = Occlusion122;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
878;81;716;623;3741.562;1590.504;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;115;-4224,384;Inherit;False;3409.094;635.7684;Deposit Mask;19;85;24;17;25;16;35;14;93;72;105;73;112;22;113;109;34;110;162;163;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;114;-3584,-2176;Inherit;False;604;191;World-Space UVs - Deposit;3;48;71;54;;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;34;-4144,640;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;162;-3968,640;Inherit;False;Property;_VertexColorChannel;Vertex Color Channel;14;0;Create;True;0;0;0;False;0;False;0;2;2;True;;KeywordEnum;4;R;G;B;A;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-3968,832;Inherit;False;Property;_LayerPosition;Layer Position;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-3536,-2112;Inherit;False;Property;_Tiling;Tiling;25;0;Create;True;0;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;109;-3712,688;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;-3376,-2112;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-3712,896;Inherit;False;Property;_LayerContrast;Layer Contrast;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;-3232,-2112;Inherit;False;WorldSpaceDeposit;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;128;-4224,-768;Inherit;False;2687.375;1026.049;Normals;19;118;76;3;150;63;52;9;64;141;139;8;151;75;13;78;87;138;81;86;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;112;-3456,784;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-3328,480;Float;False;Property;_LayerPower;Layer Power;15;0;Create;True;0;0;0;False;1;Space(10);False;0.5;0.288;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-3968,-64;Inherit;False;48;WorldSpaceDeposit;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;73;-3040,432;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-3968,-192;Inherit;False;Property;_2ndNormalPower;Normal Power;23;0;Create;False;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;64;-4192,-288;Inherit;True;Property;_DetailNormalMap;Normal;22;0;Create;False;0;0;0;False;0;False;None;fcc0f7f30aa29f44e831dfb556f69df4;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ClampOpNode;105;-3200,784;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;72;-2848,592;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TriplanarNode;63;-3584,-256;Inherit;True;Spherical;World;True;Top Texture 1;_TopTexture1;white;-1;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;14;-2336,544;Inherit;True;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2592,752;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;144;-4223.918,-2173.562;Inherit;False;610;189;World-Space UVs - Second Maps;3;147;146;145;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ToggleSwitchNode;35;-1984,736;Inherit;False;Property;_UseVertexColor;Use Vertex Color;13;0;Create;True;0;0;0;False;3;Space(10);Header(Layer);Space(10);False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-4175.918,-2109.562;Inherit;False;Property;_Tiling2;Tiling;12;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;146;-4015.918,-2109.562;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1728,464;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1600,720;Float;False;Property;_LayerThreshold;Layer Threshold;16;0;Create;True;0;0;0;False;0;False;50;50;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-1504,464;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;147;-3871.918,-2109.562;Inherit;False;WorldSpaceSecondMaps;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;163;-1285.247,709.5432;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.001;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;129;-4224,-1920;Inherit;False;1990.061;1061.969;Diffuse / Colors;17;116;164;165;26;12;88;10;11;134;2;60;1;148;61;49;140;149;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;-4192,-1360;Inherit;False;147;WorldSpaceSecondMaps;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;24;-1312,464;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;149;-4160,-1552;Inherit;True;Property;_SecondAlbedo;Albedo;8;0;Create;False;0;0;0;False;3;Space(10);Header(Second Maps);Space(10);False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.CommentaryNode;127;-4224,1152;Inherit;False;2301.409;1022.124;Metallic / Smoothness / Occlusion;22;120;121;122;30;31;33;136;65;135;137;90;7;66;50;152;142;153;166;170;169;172;174;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-3968,-576;Inherit;False;Property;_SecondNormalPower;Normal Power;10;0;Create;False;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-3840,-1744;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;0;False;0;False;-1;None;dc25dfb1b19ccb34c9e5ec3797f30534;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;142;-4096,1632;Inherit;False;147;WorldSpaceSecondMaps;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;151;-4190,-510;Inherit;True;Property;_DetailNormalMap1;Normal;9;0;Create;False;0;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;8;-3968,-704;Inherit;False;Property;_NormalPower;Normal Power;3;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-1024,512;Inherit;False;BlendAlpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-3968,-976;Inherit;False;48;WorldSpaceDeposit;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TriplanarNode;148;-3904,-1504;Inherit;True;Spherical;World;False;Top Texture 5;_TopTexture5;white;-1;None;Mid Texture 5;_MidTexture5;white;-1;None;Bot Texture 5;_BotTexture5;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;141;-3968,-448;Inherit;False;147;WorldSpaceSecondMaps;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;61;-3968,-1232;Inherit;True;Property;_DetailAlbedoMap;Albedo;21;0;Create;False;0;0;0;False;0;False;None;1bbb6f363f884124aaa1cad8329f78cf;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;153;-4160,1408;Inherit;True;Property;_DetailNormalMap2;Metallic (R) Occlusion (G) Smoothness (A);11;0;Create;False;0;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;81;-2944,-320;Inherit;False;Constant;_Color0;Color 0;22;0;Create;True;0;0;0;False;0;False;0.01176471,0,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-3584,-704;Inherit;True;Property;_BumpMap;Normal;2;0;Create;False;0;0;0;False;0;False;-1;None;813ee639c17076a438ea1b8c35ba1df3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-3456,-1872;Inherit;False;Property;_Color;Main Color;0;0;Create;False;0;0;0;False;2;Header(Mesh Maps);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;86;-2944,-64;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-4096,2016;Inherit;False;48;WorldSpaceDeposit;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;66;-4160,1792;Inherit;True;Property;_DetailMetallicGlossMap;Metallic (R) Occlusion (G) Smoothness (A);24;0;Create;False;0;0;0;False;0;False;None;8d961c07e5665584094383581d95e88e;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;7;-3712,1200;Inherit;True;Property;_MetallicGlossMap;Metallic (R) Occlusion (G) Smoothness (A);4;0;Create;False;0;0;0;False;0;False;-1;None;d656b2dfe9d3f384a9845ea260f227c5;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-3456,-1360;Inherit;False;Property;_2ndColor;Color;20;0;Create;False;0;0;0;False;3;Space(10);Header(Layer Maps);Space(10);False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;152;-3792,1408;Inherit;True;Spherical;World;False;Top Texture 7;_TopTexture7;white;-1;None;Mid Texture 7;_MidTexture7;white;-1;None;Bot Texture 7;_BotTexture7;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;150;-3584,-512;Inherit;True;Spherical;World;True;Top Texture 6;_TopTexture6;white;-1;None;Mid Texture 6;_MidTexture6;white;-1;None;Bot Texture 6;_BotTexture6;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;60;-3584,-1104;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-3456,-1616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-3168,-1488;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-3328,1456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;78;-2688,-192;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-3072,1984;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-3328,1232;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;65;-3792,1888;Inherit;True;Spherical;World;False;Top Texture 2;_TopTexture2;white;-1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;88;-3168,-1360;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;-3328,1728;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;138;-3120,-592;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-3168,-1616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-2688,-576;Inherit;False;85;BlendAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-2560,1600;Inherit;False;Property;_SmoothnessPower;Smoothness Power;6;0;Create;True;0;0;0;False;0;False;0;0.657;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-2560,1344;Inherit;False;Property;_MetallicPower;Metallic Power;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;-2816,1472;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;-2560,1856;Inherit;False;Property;_OcclusionPower;Occlusion Power;7;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-2816,1728;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;30;-2816,1216;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;75;-2432,-224;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;13;-2432,-704;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;164;-2864,-1280;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;26;-2944,-1552;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;165;-2688,-1408;Inherit;False;Property;_SeeVertexColors;See Vertex Colors;26;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-2352,1216;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-2352,1472;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;174;-2319.798,1732.173;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;76;-2048,-480;Inherit;False;Property;_BlendNormals;Blend Normals;19;0;Create;True;0;0;0;False;1;Space(10);False;1;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;-2208,1472;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;-1792,-448;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-2432,-1408;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;-2208,1216;Inherit;False;Metallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;-2166,1730;Inherit;False;Occlusion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;123;-256,-64;Inherit;False;120;Metallic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-256,128;Inherit;False;122;Occlusion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-256,32;Inherit;False;121;Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;-256,-160;Inherit;False;118;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;117;-256,-256;Inherit;False;116;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,-128;Float;False;True;-1;7;;0;0;Standard;Custom/StandardLayered;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;162;1;34;1
WireConnection;162;0;34;2
WireConnection;162;2;34;3
WireConnection;162;3;34;4
WireConnection;109;0;162;0
WireConnection;109;1;110;0
WireConnection;71;0;54;0
WireConnection;71;1;54;0
WireConnection;48;0;71;0
WireConnection;112;1;109;0
WireConnection;112;0;113;0
WireConnection;73;0;22;0
WireConnection;105;0;112;0
WireConnection;72;0;105;0
WireConnection;72;1;73;0
WireConnection;63;0;64;0
WireConnection;63;8;9;0
WireConnection;63;3;52;0
WireConnection;14;0;63;0
WireConnection;93;0;72;0
WireConnection;93;1;105;0
WireConnection;35;0;14;2
WireConnection;35;1;93;0
WireConnection;146;0;145;0
WireConnection;146;1;145;0
WireConnection;16;0;35;0
WireConnection;16;1;22;0
WireConnection;17;0;16;0
WireConnection;147;0;146;0
WireConnection;163;0;25;0
WireConnection;24;0;17;0
WireConnection;24;1;163;0
WireConnection;85;0;24;0
WireConnection;148;0;149;0
WireConnection;148;3;140;0
WireConnection;3;5;8;0
WireConnection;152;0;153;0
WireConnection;152;3;142;0
WireConnection;150;0;151;0
WireConnection;150;8;139;0
WireConnection;150;3;141;0
WireConnection;60;0;61;0
WireConnection;60;3;49;0
WireConnection;134;0;1;0
WireConnection;134;1;148;0
WireConnection;12;0;11;0
WireConnection;12;1;60;0
WireConnection;137;0;7;4
WireConnection;137;1;152;4
WireConnection;78;0;81;0
WireConnection;78;1;63;0
WireConnection;78;2;86;0
WireConnection;136;0;7;1
WireConnection;136;1;152;1
WireConnection;65;0;66;0
WireConnection;65;3;50;0
WireConnection;135;0;7;2
WireConnection;135;1;152;2
WireConnection;138;0;3;0
WireConnection;138;1;150;0
WireConnection;10;0;2;0
WireConnection;10;1;134;0
WireConnection;31;0;137;0
WireConnection;31;1;65;4
WireConnection;31;2;90;0
WireConnection;33;0;135;0
WireConnection;33;1;65;2
WireConnection;33;2;90;0
WireConnection;30;0;136;0
WireConnection;30;1;65;1
WireConnection;30;2;90;0
WireConnection;75;0;138;0
WireConnection;75;1;78;0
WireConnection;13;0;138;0
WireConnection;13;1;63;0
WireConnection;13;2;87;0
WireConnection;26;0;10;0
WireConnection;26;1;12;0
WireConnection;26;2;88;0
WireConnection;165;1;26;0
WireConnection;165;0;164;0
WireConnection;171;0;30;0
WireConnection;171;1;170;0
WireConnection;172;0;31;0
WireConnection;172;1;169;0
WireConnection;174;0;33;0
WireConnection;174;1;166;0
WireConnection;76;0;13;0
WireConnection;76;1;75;0
WireConnection;121;0;172;0
WireConnection;118;0;76;0
WireConnection;116;0;165;0
WireConnection;120;0;171;0
WireConnection;122;0;174;0
WireConnection;0;0;117;0
WireConnection;0;1;119;0
WireConnection;0;3;123;0
WireConnection;0;4;124;0
WireConnection;0;5;125;0
ASEEND*/
//CHKSM=2695398C5B5765918C980D5C9AAACD793C903413