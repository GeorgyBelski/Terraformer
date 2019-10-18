using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Damageable
{
    public Tower thisTower;
    private bool isRepair;
    private float maxRepairHealth = 0;


    void Start()
    {
        isRepair = false;
    }

    void Update()
    {
        if (isRepair && healthRatio < maxRepairHealth)
        {
            //print(Time.deltaTime);
            base.health += (int)((maxHealth/10) * Time.deltaTime);
            
            //base.CalcHealthRatio();
        }
        else
            isRepair = false;

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
        //RemoveFromList();
        //Destroy(thisTowet.gameObject);
        
        Material mt = thisTower.gameObject.GetComponent<Renderer>().material;//.SetColor(Color.gray);
        //thisTowet.GetComponent<Material>
        mt.color = Color.gray;
        thisTower.gizmoMaterial.color = Color.gray;
        thisTower.cooldownAttack = float.PositiveInfinity;
        thisTower.timerAttack = float.PositiveInfinity;
       
    }

    public void Repair()
    {
        //print(healthRatio);
        if (healthRatio < 1) { 
            float resource = ResourceManager.resourceST;
            float repaircost = ResourceManager.RepairCost;

            float costNeeded = (1 - healthRatio) * 100 * repaircost;
            //print(costNeeded);
            if(resource > costNeeded)
            {
                maxRepairHealth = 1;
                ResourceManager.removeResource(costNeeded);
            }

            else
            {
                maxRepairHealth = healthRatio + resource / 100 / repaircost;
                ResourceManager.removeResource(resource);
                //print(maxRepairHealth);
            }

        

            isRepair = true;
        }
    }
    /*
    public void Repair()
    {
        float repaircost = ResourceManager.RepairCost;
        float resourses = ResourceManager.resourceST;
        float value = 1 - healthRatio;
        if (resourses > 100 * value * repaircost)
        {
            health = maxHealth;
            ResourceManager.removeResource(100 * value * repaircost);
            //CalcHealthRatio();
        }
        else
        {
            healthRatio = resourses * repaircost / 100;
            CalcHealthRatio();
            ResourceManager.removeResource(resourses);
        }
        //(int)((healthRatio + value) * maxHealth);
    }
    */
}
