Shader "Custom/VolumericExplosion"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            HLSLPROGRAM
            #pragma target 4.6
            #pragma enable_d3d11_debug_symbols
            #pragma vertex VertexProgram
            #pragma hull HullProgram
            #pragma domain DomainProgram
            #pragma fragment PixelProgram

            #include "VolumericExplosion.hlsli"
            #include "VolumericExplosionVS.hlsl"
            #include "VolumericExplosionHS.hlsl"
            #include "VolumericExplosionDS.hlsl"
            #include "VolumericExplosionPS.hlsl"

            ENDHLSL
        }
    }
}
