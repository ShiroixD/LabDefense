Shader "Custom/SimpleExplosionEdgeTesselation"
{
    Properties
    {
        _EdgeLength("Edge length", Range(2,50)) = 5
        _RampTex("Color Ramp", 2D) = "white" {}
        _DispTex("Displacement Texture", 2D) = "gray" {}
        _Displacement("Displacement", Range(0, 1.0)) = 0.1
        _ChannelFactor("ChannelFactor (r,g,b)", Vector) = (1,0,0)
        _Range("Range (min,max)", Vector) = (0,0.5,0)
        _ClipRange("ClipRange [0,1]", float) = 0.8
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessEdge nolightmap
            #pragma target 4.6
            #include "Tessellation.cginc"

            float _EdgeLength;
            sampler2D _DispTex;
            float _Displacement;
            float3 _ChannelFactor;
            float2 _Range;
            float _ClipRange;

            struct Input
            {
                float2 uv_DispTex;
            };

            float4 tessEdge(appdata_full v0, appdata_full v1, appdata_full v2)
            {
                return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
            }

            void disp(inout appdata_full v)
            {
                float3 dcolor = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0));
                float d = (dcolor.r * _ChannelFactor.r + dcolor.g * _ChannelFactor.g + dcolor.b * _ChannelFactor.b);
                v.vertex.xyz += v.normal * d * _Displacement;
            }

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            sampler2D _RampTex;

            void surf(Input IN, inout SurfaceOutput o)
            {
                float3 dcolor = tex2D(_DispTex, IN.uv_DispTex);
                float d = (dcolor.r * _ChannelFactor.r + dcolor.g * _ChannelFactor.g + dcolor.b * _ChannelFactor.b) * (_Range.y - _Range.x) + _Range.x;
                clip(_ClipRange - d);
                half4 c = tex2D(_RampTex, float2(d, 0.5));
                o.Albedo = c.rgb;
                o.Specular = 0.2;
                o.Gloss = 1.0;
                o.Emission = c.rgb * c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
