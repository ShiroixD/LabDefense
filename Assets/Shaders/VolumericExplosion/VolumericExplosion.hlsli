#ifndef VOLUMERIC_EXPLOSION_INCLUDED
#define VOLUMERIC_EXPLOSION_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

struct VertexInput {
	float4 position : POSITION;
	float2 uv : TEXCOORD0;
};

struct FragmentInput {
	float4 position : SV_POSITION;
	float2 uv : TEXCOORD0;
};

CBUFFER_START(UnityPerFrame)
	float4x4 unity_MatrixVP;
CBUFFER_END

CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
CBUFFER_END

#endif // VOLUMERIC_EXPLOSION_INCLUDED