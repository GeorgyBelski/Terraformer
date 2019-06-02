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

    public override void ApplyDeath()
    {
        //RemoveFromList();
        //Destroy(thisTowet.gameObject);
        
        Material mt = thisTowet.gameObject.GetComponent<Material>();//.SetColor(Color.gray);
        //thisTowet.GetComponent<Material>
        //.color = Color.gray;
        thisTowet.cooldownAttack = float.PositiveInfinity;
        thisTowet.timerAttack = float.PositiveInfinity;
       
    }
}
