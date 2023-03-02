Shader "Custom RP/Lit" {

	// Material properties
	Properties
	{
		_BaseMap("Texture", 2D) = "white" {}
		_BaseColor("Color", Color) = (0.5, 0.5, 0.5, 1.0)
		// Clips fragments below a set alpha value
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		[Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		// Blend modes for transparency
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
		// Turn depth off for transparency
		[Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
		// Fades the diffuse light, but keeps the specular reflections at full strength on transparency.
		[Toggle(_PREMULTIPLY_ALPHA)] _PremulAlpha ("Premultiply Alpha", Float) = 0
	}

	SubShader
	{
		Pass 
		{
			Tags  
			{
				"LightMode" = "CustomLit"
			}
			// Blend and depth (ZWrite) pass properties used for correct transparency rendering
			Blend [_SrcBlend][_DstBlend]
			ZWrite [_ZWrite]
			HLSLPROGRAM
			#pragma shader_feature _CLIPPING
			#pragma shader_feature _PREMULTIPLY_ALPHA
			// GPU Instancing to batch same mesh objects (putting all properties to arrays for the GPU)
			#pragma multi_compile _ _DIRECTIONAL_PCF3 _DIRECTIONAL_PCF5 _DIRECTIONAL_PCF7
			#pragma multi_compile _ _CASCADE_BLEND_SOFT _CASCADE_BLEND_DITHER
			#pragma multi_compile_instancing
			#pragma vertex LitPassVertex
			#pragma fragment LitPassFragment
			#include "LitPass.hlsl"
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
			#pragma shader_feature _CLIPPING
			#pragma multi_compile_instancing
			#pragma vertex ShadowCasterPassVertex
			#pragma fragment ShadowCasterPassFragment
			#include "ShadowCasterPass.hlsl"
			ENDHLSL
		}
	}
	CustomEditor"CustomShaderGUI"
}