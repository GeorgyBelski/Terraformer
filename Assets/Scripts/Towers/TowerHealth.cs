using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Damageable
{
    public Tower thisTower;


    void Start()
    {
        
    }

    void Update()
    {
        base.CalcHealthRatio();
       /* if (health == 0 && TowerManager.towers.Contains(thisTower)){
            RemoveFromList();
        }*/
    }


    public override void RemoveFromList()
    {
        TowerManager.RemoveTower(thisTower);
    }

    public override void ApplyDeath()
    {
        thisTower.BreakSymbiosis();
        RemoveFromList();
        //Destroy(thisTowet.gameObject);

        thisTower.cooldownAttack = float.PositiveInfinity;
        thisTower.timerAttack = float.PositiveInfinity;
       
    }
}
