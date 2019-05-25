using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Damageable
{
    public Tower thisTowet;


    void Start()
    {
        
    }

    void Update()
    {
        base.CalcHealthRatio();
    }


    public override void RemoveFromList()
    {
        TowerManager.towers.Remove(thisTowet);
    }
}
