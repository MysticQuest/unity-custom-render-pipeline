Shader "Custom RP/Unlit" {

	// Material properties
	Properties
	{
		_BaseMap("Texture", 2D) = "white" {}
		_BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		// Clips fragments below a set alpha value
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		[Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0
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
			#pragma shader_feature _CLIPPING
			// GPU Instancing to batch same mesh objects (putting all properties to arrays for the GPU)
			#pragma multi_compile_instancing
			#pragma vertex UnlitPassVertex
			#pragma fragment UnlitPassFragment
			#include "UnlitPass.hlsl"
			ENDHLSL
		}
	}
CustomEditor"CustomShaderGUI"
}