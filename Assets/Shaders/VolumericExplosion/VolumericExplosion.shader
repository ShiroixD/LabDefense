Shader "Custom/VolumericExplosion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            HLSLPROGRAM
            #pragma target 4.6
            #pragma vertex VertexProgram
            #pragma hull HullProgram
            #pragma domain DomainProgram
            #pragma fragment PixelProgram

            sampler2D _MainTex;

            #include "VolumericExplosion.hlsli"
            #include "VolumericExplosionVS.hlsl"
            #include "VolumericExplosionHS.hlsl"
            #include "VolumericExplosionDS.hlsl"
            #include "VolumericExplosionPS.hlsl"

            ENDHLSL
        }
    }
}
