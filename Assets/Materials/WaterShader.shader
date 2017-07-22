﻿Shader "Custom/WaterShader"
{
	Properties
	{
		_DisplacementTex("Displacement texture", 2D) = "white" {}
		_WaterSpeed("Water Speed", Float) = 0.5
		_Strength("Displacement strength", Float) = 1
		_Tint("Color", Color) = (0,0.1,0,0.1)
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
		ZWrite On Lighting Off Cull Off Fog{ Mode Off } Blend One Zero

		GrabPass{ "_GrabTexture" }

		Pass
		{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _GrabTexture;

		//sampler2D _MainTex;
		sampler2D _DisplacementTex;
		fixed4 _Tint;
		fixed _Strength;
		fixed _WaterSpeed;

		struct vin_vct
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f_vct
		{
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;

			float4 uvgrab : TEXCOORD1;
		};

		v2f_vct vert(vin_vct v)
		{
			v2f_vct o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.color = v.color;

			o.texcoord = v.texcoord;

			o.uvgrab = ComputeGrabScreenPos(o.vertex);
			return o;
		}

		fixed4 frag(v2f_vct i) : COLOR
		{
			fixed time = _Time[1];

			half4 n = tex2D(_DisplacementTex, i.uvgrab + half2(0, time * _WaterSpeed));
			half4 d = n * 2 - 1;
			i.uvgrab += d * _Strength;
			i.uvgrab = saturate(i.uvgrab);

			float4 c = tex2D(_GrabTexture, i.uvgrab);
			return c * _Tint;
		}

		ENDCG
	}
	}
}