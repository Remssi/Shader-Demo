Shader "ShieldEffects/ForceShieldShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LineTex ("Line Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Distort("Distortion", float) = 1
		_LineDistort("Line Distortion", float) = 1
		_Speed("Speed", float) = 1
		_LineSpeed("Line Speed", float) = 1
		_RimPower("Rim Power", float) = 1
		_RimMag ("Rim Magnitude", float) = 1
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_ShieldSize("Shield Size", float) = 0.15
	}
	SubShader
	{
		Tags { "Queue"="Overlay" "RenderType"="Transparent" }
		LOD 100

		GrabPass { "_BackgroundTexture" }

		Zwrite Off
		//Blend SrcAlpha OneMinusSrcAlpha
		//Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 grabPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 wPos : TEXCOORD2;
				float2 linePos : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _LineTex;
			float4 _Color;
			float _Distort;
			float _LineDistort;
			float _Speed;
			float _LineSpeed;
			float _RimPower;
			float _RimMag;
			float4 _RimColor;
			float _ShieldSize;

			sampler2D _BackgroundTexture;
			fixed4 _BackgroundTexture_TexelSize;
			
			v2f vert (appdata v)
			{
				v.vertex.xyz += v.normal * _ShieldSize;

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.linePos = TRANSFORM_TEX(v.uv, _MainTex);

				o.uv.x += _Time * _Speed;
				o.uv.y += _Time * _Speed;

				o.linePos.x += _Time * _LineSpeed;
				o.linePos.y += _Time * _LineSpeed;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				// distortion uvs
				fixed2 col = tex2D(_MainTex, i.uv).xy;
				col += tex2D(_LineTex, i.linePos).xy * _LineDistort;

				i.grabPos.xy += col * _Distort;

				// grabpass
				half4 grab = tex2Dproj(_BackgroundTexture, i.grabPos) * _Color;

				// rim
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.wPos.xyz);
				float d = dot(i.normal, - _WorldSpaceLightPos0);
				float r = pow(1.0 - saturate(dot(i.normal, viewDir)), _RimPower);

				return grab + (float4(0.1, 0.1, 0.1, 1.0) * d + r * _RimColor * _RimMag);
			}
			ENDCG
		}
	}
}
