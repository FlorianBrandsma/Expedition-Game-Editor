﻿Shader "ZWrite/Diffuse/DoubleSided" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}


	SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 200

		Cull Off

		Pass
		{
			ZWrite On
			ColorMask 0
		}

		UsePass "Transparent/Diffuse/FORWARD"

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		fixed4 _Color;
		
		struct Input 
		{
			float2 uv_MainTex;
			float4 color : COLOR;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Albedo *= IN.color.rgb;
			o.Alpha = c.a;
		}
		
	ENDCG
	
	}

Fallback "Mobile/Diffuse"

}