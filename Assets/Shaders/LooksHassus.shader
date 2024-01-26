//https://github.com/MirzaBeig/Post-Processing-Wireframe-Outlines/blob/main/Assets/Mirza%20Beig/Post-Processing%20Wireframe%20%2B%20Outlines/Shaders/Post-Processing%20Wireframe%20%2B%20Outlines.shader#L64
Shader "Hidden/Custom/LooksHassus"
{
    HLSLINCLUDE
    #include "StrippedPostProcess.hlsl"
    #include "Util.hlsl"


    //#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    Texture2D _MainTex;
    SamplerState sampler_MainTex;

    float _EdgeDepthMin;
    float _EdgeDepthMax;

    sampler2D _CameraDepthTexture;

    float _LineWidth;
    float _EdgeThreshold;
    float _Blend1;
    float _Blend2;
    float _Blend3;

    fixed4 _Colour;

    struct v2f
    {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
    };

    float4 AD(float2 uv)
    {
        float4 c = 0.0;
        const int Q = 9;

        float2 s = _LineWidth * ((1.0 / _ScreenParams.xy) / Q);

        for (int y = -Q + 1; y < Q; y++)
        {
            for (int x = -Q + 1; x < Q; x++)
            {
                c += tex2D(_CameraDepthTexture,
                           float4(uv + (float2(x, y) * s), 0.0, 0.0));
            }
        }

        return c / ((Q * 2 - 1) * (Q * 2 - 1));
    }

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float d = Linear01Depth(tex2D(_CameraDepthTexture, i.texcoord));
        float ad = Linear01Depth(AD(i.texcoord));

        float dt = d > ad - _EdgeThreshold;

        
        float3 c = 1.0;
        float4 t = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

        //return t;

        c = lerp(t, d, _Blend1);
        c = lerp(c, dt, _Blend2);

        c = lerp(c, lerp(_Colour.rgb, t, dt), _Blend3);

        return float4(c, 1.0);
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }
    }
}