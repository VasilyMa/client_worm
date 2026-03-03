Shader "Hidden/WaterShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Waves ("Waves", Float) = 5000
		_Offset ("Offset", Float) = 0
		_Wind ("Wind", Float) = 0
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
			float _Waves;
			float _Offset;
			float _Wind;

			fixed4 frag (v2f i) : SV_Target
			{
			    float2 uv = i.uv;
			    uv.x -= _Time * _Wind;
			    uv.x *= _Waves;
			    uv.x += _Offset;
			    uv.x -= floor(uv.x);
			    uv.x = abs(2 * uv.x - 1);
				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}
