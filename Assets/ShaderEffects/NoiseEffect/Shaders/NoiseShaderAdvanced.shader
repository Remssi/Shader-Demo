Shader "Hidden/NoiseShaderAdvanced"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_NoiseMag ("Noise Magnitude", Range(0, 2)) = 1
		_OffsetValue ("Offset Value", Range(0, 2)) = 0

		_NoiseLineTex ("Noise Line Texture", 2D) = "white" {}
		_LineMag ("Line Magnitude", Range(0, 100)) = 0.25
		_LineDropSpeed ("Line Drop Speed", Range(0, 100)) = 0
		_DistortionSpeed ("Distortion Speed", Range(0, 100)) = 0
		_LineStartPos ("Line Start Position", Range(0, 1)) = 0

		_NoiseLineTex2 ("Noise Line Texture 2", 2D) = "white" {}
		_Line2Mag ("Line 2 Magnitude", Range(0, 100)) = 0.25
		_Line2DropSpeed ("Line 2 Drop Speed", Range(0, 100)) = 0
		_Distortion2Speed ("Distortion2 Speed", Range(0, 100)) = 0
		_Line2StartPos ("Line 2 Start Position", Range(0, 1)) = 0

		_VioletGreenTex ("Violet Green Texture", 2D) = "white" {}
		_VioletGreenMag ("Violet Green Magnitude", Range(0, 1)) = 1
		_VioletGreenOffset ("Violet Green Offset", Range(0, 2)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
			sampler2D _NoiseTex;
			sampler2D _NoiseLineTex;
			sampler2D _NoiseLineTex2;

			sampler2D _VioletGreenTex;

			float _NoiseMag;
			float _OffsetValue;

			float _LineDropSpeed;
			float _DistortionSpeed;
			float _LineMag;
			float _LineStartPos;

			float _Line2DropSpeed;
			float _Distortion2Speed;
			float _Line2Mag;
			float _Line2StartPos;

			float _VioletGreenMag;
			float _VioletGreenOffset;


			fixed4 frag (v2f i) : SV_Target
			{

				float2 distScroll = float2(_Time.x * _DistortionSpeed, _LineDropSpeed);

				fixed2 dist = (tex2D(_NoiseLineTex, i.uv + distScroll + _LineStartPos).xy) * _LineMag;

				float2 distScroll2 = float2(_Time.x * _Distortion2Speed, _Line2DropSpeed);

				fixed2 dist2 = (tex2D(_NoiseLineTex2, i.uv + distScroll2 + _Line2StartPos).xy) * _Line2Mag;

				fixed4 col = tex2D(_MainTex, i.uv + (dist + dist2) * 0.025);
				col += tex2D(_NoiseTex, i.uv + _OffsetValue + (dist + dist2) * 0.025) * _NoiseMag;

				col += tex2D(_VioletGreenTex, i.uv + _VioletGreenOffset) * _VioletGreenMag;

				return col;
			}
			ENDCG
		}
	}
}
