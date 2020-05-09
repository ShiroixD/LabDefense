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
            #pragma target 3.5
            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram

            sampler2D _MainTex;

            #include "VolumericExplosion.hlsli"
            #include "VolumericExplosionVS.hlsl"
            #include "VolumericExplosionFS.hlsl"

            ENDHLSL
        }
    }
}
