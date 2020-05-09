#ifndef VOLUMERIC_EXPLOSION_FS_INCLUDED
#define VOLUMERIC_EXPLOSION_FS_INCLUDED

#include "VolumericExplosion.hlsli"

float4 FragmentProgram(FragmentInput input) : SV_TARGET{
	return tex2D(_MainTex, input.uv);
}

#endif // VOLUMERIC_EXPLOSION_FS_INCLUDED