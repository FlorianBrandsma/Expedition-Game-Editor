Shader "Custom/MultipleTextures" 
{
	Properties
	{
		_Tex1("Layer 1 Texture", 2D) = "white" {}
		_Tex2("Layer 2 Texture", 2D) = "white" {}
		_Tex3("Layer 3 Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		SetTexture[_Tex1]{ constantColor(1,1,1,1) combine texture * constant }
		SetTexture[_Tex2]{ constantColor(1,1,1,1) combine texture lerp(texture) previous }
		SetTexture[_Tex3]{ constantColor(1,1,1,1) combine texture lerp(texture) previous }
		}
	}
}