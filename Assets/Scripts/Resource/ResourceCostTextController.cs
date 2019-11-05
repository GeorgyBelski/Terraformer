using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCostTextController : MonoBehaviour
{
    public void ReturnToNormal()
    {
        ResourceManager.ExitCostIsTooHighSignal();
        ResourceManager.DisplayCost(false);
    }
}
