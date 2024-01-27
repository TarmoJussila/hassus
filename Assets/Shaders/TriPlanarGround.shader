Shader "Custom/TriPlanarGround"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main texture (RGB)", 2D) = "white" {}
        _TopTex ("Top texture (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BlendOffset ("Blend Offset", Range(0, 0.5)) = 0.25
        _BlendPow ("Blend power", Range(1,32)) = 1
        _TopPow ("Top power", Range(1,32)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Hassus fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _TopTex;

        float4 _MainText_ST;
        float4 _TopText_ST;

        struct Input
        {
            float2 uv_MainTex;
            fixed3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed _BlendPow;
        fixed _BlendOffset;
        fixed _TopPow;

        #include <AutoLight.cginc>
        #include <Lighting.cginc>

        half4 LightingHassus(SurfaceOutput s, half3 lightDir, half atten)
        {
            float NdotL = max(0.0, dot(s.Normal, lightDir));

            float lightBandsMultiplier = 2 / 256;
            float lightBandsAdditive = 2 / 2;
            fixed bandedNdotL = (floor((NdotL * 256 + lightBandsAdditive) / 2))
                * lightBandsMultiplier;

            float3 lightingModel = bandedNdotL * fixed4(1, 1, 1, 1);
            float attenuation = 1; //LIGHT_ATTENUATION(0.5);
            float3 attenColor = attenuation * _LightColor0.rgb;
            float4 finalDiffuse = float4(lightingModel * attenColor, 1);
            return finalDiffuse;
        }

        void vert(inout appdata_full a, out Input o)
        {
            o.uv_MainTex = a.texcoord;
            o.worldPos = mul(unity_ObjectToWorld, a.vertex);
        }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed2 uvX = IN.worldPos.zy;
            fixed2 uvY = IN.worldPos.xz;
            fixed2 uvZ = IN.worldPos.xy;

            uvX.x *= (o.Normal.x < 0 ? -1 : 1);
            uvY.x *= (o.Normal.y < 0 ? -1 : 1);
            uvZ.x *= (o.Normal.z < 0 ? 1 : -1);

            // fix repetitions
            uvX.y += 0.5;
            uvZ.x += 0.5;

            fixed4 xy = tex2D(_MainTex, uvZ + _MainText_ST.xy) * _Color;
            fixed4 xzTop = tex2D(_TopTex, uvY + _TopText_ST.xy) * _Color;
            fixed4 xzMain = tex2D(_MainTex, uvY + _MainText_ST.xy) * _Color;
            fixed4 yz = tex2D(_MainTex, uvX + _MainText_ST.xy) * _Color;

            //fixed4 xz = lerp(xzMain, xzTop, 0);
            fixed up = clamp(dot(fixed3(0, 1, 0), o.Normal), 0, 1);
            fixed4 xz = lerp(xzMain, xzTop, pow(up, _TopPow));

            fixed3 absNormal = abs(o.Normal);
            absNormal = saturate(absNormal - _BlendOffset);
            absNormal = pow(absNormal, _BlendPow);
            absNormal = absNormal / (absNormal.x + absNormal.y + absNormal.z);

            fixed4 c = (xy * absNormal.z + xz * absNormal.y + yz * absNormal.x);

            o.Albedo = c.rgb;

            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}