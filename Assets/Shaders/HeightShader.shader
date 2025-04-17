Shader "Unlit/HeightShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MountainTex ("Mountain Texture", 2D) = "white" {}
        _SnowTex ("Snow Texture", 2D) = "white" {}
        _WaterTex ("Water Texture", 2D) = "white" {}

        _SnowHeight ("Snow Height", Range(0.0, 50.0)) = .0
        _MountainHeight ("Mountain Height", Range(0.0, 10.0)) = 10.0
        _WaterHeight ("Water Height", Range(0.0, 10.0)) = 10.0

        _BlendDistance ("Blend Distance", Range(0.0, 10.0)) = 10.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            //passed parameters
            sampler2D _MountainTex;
            sampler2D _SnowTex;
            sampler2D _WaterTex;

            float _SnowHeight;
            float _WaterHeight;
            float _MountainHeight;

            float _BlendDistance;

            float heightblend(float height, float mid)
            {
                return (height - mid + _BlendDistance) / (_BlendDistance*2);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 height : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.height = v.vertex.y; //store height data into "outgoing" v2f
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = 0.0;
                // sample the texture
                fixed4 col_water = tex2D(_WaterTex, i.uv);
                fixed4 col_rock = tex2D(_MountainTex, i.uv);
                fixed4 col_snow = tex2D(_SnowTex, i.uv);

                fixed4 col;

               
                if (i.height.y > _SnowHeight + _BlendDistance)
                {
                    col = col_snow;
                    col.a = 1;
                    //col = fixed4(1,1,1,1);
                }
                else if (i.height.y > _SnowHeight - _BlendDistance)
                {
                    t = heightblend(i.height, _SnowHeight);
                    col = (1-t)*col_rock + t*col_snow;
                }
                else if (i.height.y > _MountainHeight) {

                    col = col_rock;
                    //col = fixed4(.3,.2,.0,1);
                }
                else {
                    col = col_water;
                    //col = fixed4(0, 0, 0.8, 1);
                }

                // if (i.height.y > _SnowHeight) {

                //     col = col_snow;
                //     //col = fixed4(1, 1, 1, 1);
                // }
                // else if (i.height.y > _MountainHeight ) {

                //     col = col_rock;
                //     //col = fixed4(.3,.2,.0,1);
                // }
                // else {
                //     col = col_water;
                //     //col = fixed4(0, 0, 0.8, 1);
                // }



                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
