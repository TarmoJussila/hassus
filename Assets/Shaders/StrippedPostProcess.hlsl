
#include "UnityCG.cginc"

// Purpose of this file is to include some of the critical things defined in
// #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
// Without including the whole thing because it conflicts with some stuff in UnityCG.cginc

#define TEXTURE2D_SAMPLER2D(textureName, samplerName) Texture2D textureName; SamplerState samplerName

#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2) textureName.Sample(samplerName, coord2)

struct AttributesDefault
{
    float3 vertex : POSITION;
};


struct VaryingsDefault
{
    float4 vertex : SV_POSITION;
    float2 texcoord : TEXCOORD0;
    float2 texcoordStereo : TEXCOORD1;
    #if STEREO_INSTANCING_ENABLED
    uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
    #endif
};

// Vertex manipulation
float2 TransformTriangleVertexToUV(float2 vertex)
{
    float2 uv = (vertex + 1.0) * 0.5;
    return uv;
}

VaryingsDefault VertDefault(AttributesDefault v)
{
    VaryingsDefault o;
    o.vertex = float4(v.vertex.xy, 0.0, 1.0);
    o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

    #if UNITY_UV_STARTS_AT_TOP
    o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
    #endif

    o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);

    return o;
}