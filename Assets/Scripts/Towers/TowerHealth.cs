using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Damageable
{
    public Tower thisTower;
    public bool isRepair;
    private float maxRepairHealth = 0;
    public Color damagePoinysColor = Color.red;

    private float prevHealthRatio;
        
    void Start()
    {
        isRepair = false;
        for (int i = 0; i < damagePoints.Length; i++)
        { damagePoints[i].color = damagePoinysColor; }
    }

    void Update()
    {
        if (isRepair && healthRatio < maxRepairHealth)
        {
            if(healthRatio < prevHealthRatio)
            {
                maxRepairHealth -= prevHealthRatio - healthRatio;
                prevHealthRatio = healthRatio;
            }
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
        ResourceManager.isTowersSupplyChanged = true;
    }

    public override void ApplyDeath()
    {
        thisTower.BreakSymbiosis();
        RemoveFromList();
        //Destroy(thisTowet.gameObject);

        thisTower.cooldownAttack = float.PositiveInfinity;
        //    thisTower.timerAttack = float.PositiveInfinity;
        thisTower.enableAutoattacs = false;

        if (thisTower.type == TowerType.Electro)
        {
            ((ElectroTower)thisTower).DestroyCharge();
        }
       
    }

    public void Repair()
    {
        //print(healthRatio);
        if (healthRatio < 1 && !isRepair) { 
            float resource = ResourceManager.resource;
            float repaircost = ResourceManager.RepairCost;
            float costNeeded = (1 - healthRatio) * 100 * repaircost;

            prevHealthRatio = healthRatio;

            if(resource > costNeeded)
            {
                maxRepairHealth = 1;
                Debug.Log(costNeeded);
                ResourceManager.RemoveResource(costNeeded);
            }

            else
            {
                maxRepairHealth = healthRatio + resource / 100 / repaircost;
                ResourceManager.RemoveResource(resource);
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
