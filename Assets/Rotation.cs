using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed = 10;
    Vector3 RingRotationY, startEulerAngles;
    private void Start()
    {
        startEulerAngles = transform.eulerAngles;
    }

    void Update()
    {
        RingRotationY += Vector3.up * speed * Time.deltaTime;
        transform.eulerAngles = RingRotationY + startEulerAngles;
    }
}
