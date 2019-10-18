using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationShaderMinusTime : MonoBehaviour
{
    Material scrochingExplosion;
    public GameObject scrochingExplosionQuad;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void MinusTime()
    {
        if (!scrochingExplosion)
        {
            scrochingExplosion = scrochingExplosionQuad.GetComponent<MeshRenderer>().material;
        }
        Debug.Log("MinusTime " + scrochingExplosion.GetColor("Color_E32B0E86"));
        scrochingExplosion.SetFloat("Vector1_ACE52F54", -Time.time); // Vector1 MinisTime
        scrochingExplosion.SetColor("Color_E32B0E86", Color.red);
    }
}
