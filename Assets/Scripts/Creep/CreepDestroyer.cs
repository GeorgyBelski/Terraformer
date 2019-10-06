using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepDestroyer : MonoBehaviour
{
    public bool destroyHexagon = false;
    int creepLayerMask = 1 << CreepHexagonGenerator.creepLayer; //
    void Start()
    {
        
    }

    void Update()
    {
        RunDestroyRay();
    }

    void RunDestroyRay()
    {
        if (!destroyHexagon) { return; }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2f, creepLayerMask))
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

        destroyHexagon = false;
    }
}
