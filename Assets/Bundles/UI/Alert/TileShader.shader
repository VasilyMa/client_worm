Shader "Hidden/TileShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_XSpeed ("X Speed", Float) = 0
		_Alpha ("Alpha", Float) = 1
		_Coeff ("Coeff", Float) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _XSpeed;
			float _Alpha;
			float _Coeff;

			fixed4 frag (v2f i) : SV_Target
			{
			    float2 uv = i.uv;
			    uv.x -= _Time * _XSpeed;
			    uv.x = _Coeff * (uv.x - floor(uv.x));
			    float4 c = tex2D(_MainTex, uv);
			    c.a *= _Alpha;
				return c;
			}
			ENDCG
		}
	}
}
