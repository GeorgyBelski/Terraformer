using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertigoEffect : MonoBehaviour
{
    Material mt;
    // Start is called before the first frame update
    void Start()
    {
        mt = GetComponent<Renderer>().material;
        mt.color = new Color(mt.color.r, mt.color.g, mt.color.b, mt.color.a - 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
