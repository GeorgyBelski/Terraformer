using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBreaker : MonoBehaviour
{
    public bool breakHexagon = false;
    void Start()
    {
        
    }

    void Update()
    {
        LaunchDestroyRay();
    }

    void LaunchDestroyRay()
    {
        if (!breakHexagon) { return; }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2f, CreepHexagonGenerator.creepLayerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            GameObject hexagonGameObject = hit.collider.gameObject;
            if (CreepHexagonGenerator.meshHexagonMap.TryGetValue(hexagonGameObject, out CreepHexagonGenerator.Hexagon hexagon))
            {
                hexagon.DamageHexagon();
            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 2, Color.white);
        }

        breakHexagon = false;
    }
}
