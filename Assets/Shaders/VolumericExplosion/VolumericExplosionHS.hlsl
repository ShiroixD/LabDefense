#ifndef VOLUMERIC_EXPLOSION_HS_INCLUDED
#define VOLUMERIC_EXPLOSION_HS_INCLUDED

#include "VolumericExplosion.hlsli"

HS_CONSTANT_DATA_OUTPUT CalcHSPatchConstants()
{
	HS_CONSTANT_DATA_OUTPUT Output;

	Output.EdgeTessFactor[0] =
		Output.EdgeTessFactor[1] =
		Output.EdgeTessFactor[2] =
		Output.EdgeTessFactor[3] =
		Output.InsideTessFactor[0] =
		Output.InsideTessFactor[1] = g_TessellationFactor; // This could be made adaptive based on distance or even complexity. 

	return Output;
}

[domain("quad")]
[partitioning("integer")]
[outputtopology("triangle_ccw")]
[outputcontrolpoints(4)]
[patchconstantfunc("CalcHSPatchConstants")]
HS_OUTPUT HullProgram()
{
	HS_OUTPUT o = (HS_OUTPUT)0;
	return o;
}

#endif // VOLUMERIC_EXPLOSION_HS_INCLUDED