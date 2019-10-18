using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotationAngle = 180; 

    void Update()
    {
        float frameAngle = -rotationAngle * Time.deltaTime;
        this.transform.Rotate(0,0, frameAngle);

    }
}
