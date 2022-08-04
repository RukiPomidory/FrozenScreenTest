Shader "Unlit/CircleAlphaExpand"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _x("x", Range( 0 , 1)) = 0.5
		_y("y", Range( 0 , 1)) = 0.5
		_radius("radius", Range(0,1)) = 0.2
        _smooth("smooth", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha 
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _x;
            float _y;
            float _radius;
            float _smooth;
            float4 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed aspect = _MainTex_TexelSize.z / _MainTex_TexelSize.w;
                
                i.uv.x *= aspect;
                _x *= aspect;

                _radius *= 1 + _smooth;
                
                if (aspect > 1)
                    _radius *= aspect;
                else
                    _radius /= aspect;
                
                fixed x = i.uv.x;
                fixed y = i.uv.y;

                fixed distanceFromCenter = sqrt((_x - x) * (_x - x) + (_y - y) * (_y - y)); 

                fixed edgeOffset = distanceFromCenter - _radius;
                
                if (edgeOffset < 0)
                {
                    edgeOffset += _smooth;
                    edgeOffset /= _smooth / 2;

                    if (edgeOffset > 0)
                        edgeOffset = edgeOffset * edgeOffset;
                    
                    col.w = edgeOffset;
                }
                
                return col;
            }
            ENDCG
        }
    }
}
