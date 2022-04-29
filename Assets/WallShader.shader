Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex("Base", 2D) = "white" {}
        _Ramp("Ramp", 2D) = "gray" {}
        _Alpha("Alpha", range(0.0, 1.0)) = 1.0
    }
        SubShader
        {
            Tags
            {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }
            LOD 200

            CGPROGRAM
                #pragma surface surf ToonRamp alpha:fade

            // Input Properties
            uniform float _Alpha;
            uniform float4 _Color;
            uniform sampler2D _Ramp;
            uniform sampler2D _MainTex;

            // Input Structs
            struct Input
            {
                float2 uv_MainTex : TEXCOORD0;
            };

            // Lighting function
            inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
            {
                #ifndef USING_DIRECTIONAL_LIGHT
                lightDir = normalize(lightDir);
                #endif

                half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
                half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

                half4 c;
                c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
                c.a = _Alpha;
                return c;
            }

            // Surface shader
            void surf(Input IN, inout SurfaceOutput o)
            {
                o.Albedo = (tex2D(_MainTex, IN.uv_MainTex) * _Color).rgb;
            }
        ENDCG
        }
            Fallback "Diffuse"
}
