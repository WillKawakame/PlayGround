Shader "Unlit/Lighting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            //Cull Off
            //ZWrite Off
            //ZTest // LEqual GEqual Always
            //Blend One One // additive
            //Blend DestColor Zero// multiply

            CGPROGRAM
            //function that hold ver
            #pragma vertex vert 
            //function that hol frag
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            //Unity functions
            #include "UnityCG.cginc"

            #define name defineSomeValueToThatName


            //Variables
            sampler2D _MainTex;
            float4 _MainTex_ST;


            struct appdata {
                float4 vertex : POSITION; // Vertex Position 
                float3 normals : NORMAL; // Direction Vertex is pointing
                float2 uv : TEXCOORD0; // uv coordinates on texture
            };

            struct v2f {
                float4 vertex : SV_POSITION; // Clip Space postion
                float2 uv : TEXCOORD0; // Texture Coord
                UNITY_FOG_COORDS(1)
            };


            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // local space to clip space
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
