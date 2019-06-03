using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacing : MonoBehaviour
{
    public bool isOnTower = false;
    
    private void OnTriggerEnter(/*LayerMask lay*/ Collider other)
    {
        /*
        if(lay.value == LayerMask.GetMask("Tower")){

        }
        */
        Debug.Log("+");
        if (other.CompareTag("Tower")){
            isOnTower = true;
            Debug.Log("+");
        }
    }

    void OnTriggerExit(/*LayerMask lay*/ Collider other)
    {
        /*
        if(lay.value == LayerMask.GetMask("Tower")){

        }
        */
        if (other.CompareTag("Tower"))
        {
            isOnTower = false;
        }
    }
}
