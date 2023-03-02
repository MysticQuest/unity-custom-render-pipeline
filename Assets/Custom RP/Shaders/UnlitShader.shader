Shader "Custom RP/Unlit" {

	// Material properties
	Properties
	{
		_BaseMap("Texture", 2D) = "white" {}
		_BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		// Clips fragments below a set alpha value
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		[Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0
		[Toggle(_RECEIVE_SHADOWS)] _ReceiveShadows ("Receive Shadows", Float) = 1
		[KeywordEnum(On, Clip, Dither, Off)] _Shadows ("Shadows", Float) = 0
		// Blend modes for transparency
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
		// Option to not get depth for transparent objects
		[Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
	}

	SubShader
	{
		Pass 
		{
			// Blend and depth pass used for correct transparency rendering
			Blend [_SrcBlend][_DstBlend]
			ZWrite [_ZWrite]
			HLSLPROGRAM
			#pragma target 3.5
			#pragma shader_feature _CLIPPING
			#pragma shader_feature _RECEIVE_SHADOWS
			// GPU Instancing to batch same mesh objects (putting all properties to arrays for the GPU)
			#pragma multi_compile_instancing
			#pragma vertex UnlitPassVertex
			#pragma fragment UnlitPassFragment
			#include "UnlitPass.hlsl"
			ENDHLSL
		}
		Pass 
		{
			Tags 
			{
				"LightMode" = "ShadowCaster"
			}
			ColorMask 0
			HLSLPROGRAM
			// Doesn't compile OpenGL ES 2.0 shader variants by raising the target level of the shader pass
			#pragma target 3.5
			#pragma shader_feature _ _SHADOWS_CLIP _SHADOWS_DITHER
			#pragma multi_compile_instancing
			#pragma vertex ShadowCasterPassVertex
			#pragma fragment ShadowCasterPassFragment
			#include "ShadowCasterPass.hlsl"
			ENDHLSL
		}
	}
CustomEditor"CustomShaderGUI"
}