#ifndef COMMON_H
#define COMMON_H

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

// =======================================================================
// Global register indices
// =======================================================================
#define S_BILINEAR_CLAMPED_SAMPLER      0
#define S_BILINEAR_WRAPPED_SAMPLER      1

#define B_EXPLOSION_PARAMS              0
#define T_NOISE_VOLUME                  0
#define T_GRADIENT_TEX                  1

#define PI      (3.14159265359f)

// =======================================================================
// HLSL ONLY
// =======================================================================

#define S_REG(oo)		s##oo
#define T_REG(oo)		t##oo
#define U_REG(oo)		u##oo
#define B_REG(oo)		b##oo

SamplerState My_Trilinear_Clamp_Sampler : register(S_REG(S_BILINEAR_CLAMPED_SAMPLER));
SamplerState My_Trilinear_Repeat_Sampler : register(S_REG(S_BILINEAR_WRAPPED_SAMPLER));

#define CONSTANT_BUFFER( name, reg ) cbuffer name : register( b##reg )

// =======================================================================
// Shared ( C++ & HLSL )
// =======================================================================
CONSTANT_BUFFER(ExplosionParams, B_EXPLOSION_PARAMS)
{
    float4x4 unity_ObjectToWorld;
    float4x4 unity_WorldToObject;
    float4 _ProjectionParams;
    float4 _ScreenParams;

    float4x4 g_WorldToViewMatrix;
    float4x4 g_ViewToProjectionMatrix;
    float4x4 g_ProjectionToViewMatrix;
    float4x4 g_WorldToProjectionMatrix;
    float4x4 g_ProjectionToWorldMatrix;
    float4x4 g_ViewToWorldMatrix;

    float3 g_EyePositionWS;
    float g_NoiseAmplitudeFactor;

    float3 g_EyeForwardWS;
    float g_NoiseScale;

    float3 g_ExplosionPositionWS;
    float g_ExplosionRadiusWS;

    float3 g_NoiseAnimationSpeed;
    float4 _SinTime;

    float g_EdgeSoftness;
    float g_NoiseFrequencyFactor;
    uint g_PrimitiveIdx;
    float g_Opacity;

    float g_DisplacementWS;
    float g_StepSizeWS;
    uint g_MaxNumSteps;
    float g_NoiseInitialAmplitude;

    float2 g_UvScaleBias;
    float g_InvMaxNoiseDisplacement;
    uint g_NumOctaves;

    float g_SkinThickness;
    uint g_NumHullOctaves;
    uint g_NumHullSteps;
    float g_TessellationFactor;
};

#endif // COMMON_H