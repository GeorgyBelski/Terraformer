Shader "Transparent Texture" {
   
Properties {
    _MainTex ("Texture (A = Transparency)", 2D) = "" {
        TexGen ObjectLinear
    }
}
 
SubShader {
    Tags {Queue = Transparent}
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Pass {
        SetTexture[_MainTex] {
            Matrix [_Projector]
        }
       
    }
}
 
}