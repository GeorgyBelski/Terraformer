using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    Transform actor;
    Vector3 fromPlayerToBar;
    Quaternion StableRotation;

    void Start()
    {
        actor = transform.root;
        fromPlayerToBar = this.transform.position - actor.transform.position;
        StableRotation = Camera.main.transform.rotation;
        this.transform.rotation = StableRotation;
    }

    void Update()
    {
        this.transform.position = actor.transform.position + fromPlayerToBar;
        this.transform.rotation = StableRotation;
    }
}
