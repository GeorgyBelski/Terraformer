using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    Transform actor;
    Vector3 fromPlayerToBar;

    void Start()
    {
        actor = transform.root;
        fromPlayerToBar = this.transform.position - actor.transform.position;
    }

    void FixedUpdate()
    {
        this.transform.position = actor.transform.position + fromPlayerToBar;
        this.transform.rotation = Camera.main.transform.rotation;
    }
}
