Shader "Unlit/Transparent Colored"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}
		_Intensity("Intensity", float) = 1.0
		_Alpha("Alpha", float) = 1.0
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag			
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				float _Intensity;
				float _Alpha;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = v.texcoord;
					o.color = fixed4(
						v.color.r * _Intensity,
						v.color.g * _Intensity,
						v.color.b * _Intensity,
						clamp(v.color.a * _Alpha, 0, 1));

					return o;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					return tex2D(_MainTex, IN.texcoord) * IN.color;
				}
				ENDCG
			}
		}
}
