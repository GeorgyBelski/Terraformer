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
        //RemoveFromList();
        //Destroy(thisTowet.gameObject);

        Material mt = thisTower.gameObject.GetComponent<Renderer>().material;//.SetColor(Color.gray);
        //thisTowet.GetComponent<Material>
        mt.color = Color.gray;
        thisTower.gizmoMaterial.color = Color.gray;
        thisTower.cooldownAttack = float.PositiveInfinity;
        thisTower.timerAttack = float.PositiveInfinity;
       
    }
}
