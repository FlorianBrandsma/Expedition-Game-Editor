Shader "UI/MultipleTextures"
{
	Properties
	{
		_MainTex("", 2D) = "" {}
		_Tex0("Primary", 2D) = "white" {}
		_Tex1("Secondary", 2D) = "white" {}
		_Tex2("Tertiary", 2D) = "white" {}

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
	{
		Name "Default"
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		//UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float4 worldPosition : TEXCOORD1;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	fixed4 _TextureSampleAdd;
	float4 _ClipRect;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		OUT.worldPosition = IN.vertex;
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

		OUT.texcoord = IN.texcoord;

		OUT.color = IN.color;
		return OUT;
	}

	sampler2D _Tex0;
	sampler2D _Tex1;
	sampler2D _Tex2;
	
	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 tex1 = (tex2D(_Tex0, IN.texcoord) + _TextureSampleAdd);
		fixed4 tex2 = (tex2D(_Tex1, IN.texcoord) + _TextureSampleAdd);
		fixed4 tex3 = (tex2D(_Tex2, IN.texcoord) + _TextureSampleAdd);

		fixed4 first	= tex1.rgba * (1.0 - tex2.a) * (1.0 - tex3.a);
		fixed4 second	= tex2.rgba * tex2.a;
		fixed4 third	= tex3.rgba * tex3.a;

		fixed4 output	= first + second + third;

		output.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

#ifdef UNITY_UI_ALPHACLIP
		clip(output.a - 0.001);
#endif

		return output;
	}
	
		ENDCG
	}
	}
}
