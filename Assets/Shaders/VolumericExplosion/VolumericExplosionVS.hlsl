#ifndef VOLUMERIC_EXPLOSION_VS_INCLUDED
#define VOLUMERIC_EXPLOSION_VS_INCLUDED

#include "VolumericExplosion.hlsli"

FragmentInput VertexProgram(VertexInput input) {
	FragmentInput output;
	float4 worldPos = mul(unity_ObjectToWorld, float4(input.position.xyz, 1.0));
	output.position = mul(unity_MatrixVP, worldPos);
	output.uv = input.uv;
	return output;
}

#endif // VOLUMERIC_EXPLOSION_VS_INCLUDED