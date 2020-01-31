using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TowerHealth : Damageable
{
    public Tower thisTower;

    public Color damagePointsColor = Color.red;

    private float prevHealthRatio;

    private float emergencySoundCounter = 10f;
    private float realEmergencySoundCounter;

    void Start()
    {
        realEmergencySoundCounter = 0;
        isHeal = false;
        for (int i = 0; i < damagePoints.Length; i++)
        { damagePoints[i].color = damagePointsColor; }
    }

    void Update()
    {
        base.CalcHealthRatio();

        if (isHeal && healthRatio < maxRepairHealthRatio)
        {
            //base.CalcHealthRatio();
           // print(maxRepairHealth);
            base.health += (int)((maxHealth/15) * Time.deltaTime);
            
            //base.CalcHealthRatio();
        }
        else
        {
            //print("+");
            isHeal = false;
            if(healBar)
                healBar.fillAmount = healthBar.fillAmount;
            //ealHealth.fillAmount = healthBar.fillAmount;
            
        }


        realEmergencySoundCounter -= Time.deltaTime;
       /* if (health == 0 && TowerManager.towers.Contains(thisTower)){
            RemoveFromList();
        }*/
    }


    public override void RemoveFromList()
    {
        TowerManager.RemoveTower(thisTower);
        ResourceManager.isTowersSupplyChanged = true;
    }

    public override void ApplyDamage(int value, Vector3 shootPoint, Vector3 direction)
    {
        if (thisTower.type == TowerType.Terraformer)
        {
            if(realEmergencySoundCounter <= 0)
                ((Terraformer)thisTower).playEmergency();
            realEmergencySoundCounter = emergencySoundCounter;
        }
        base.ApplyDamage(value, shootPoint, direction);
    }

    public override void ApplyDeath()
    { 
        thisTower.BreakSymbiosis();
        if (thisTower.currentVisualLink) { Destroy(thisTower.currentVisualLink.gameObject); }
        RemoveFromList();

        thisTower.cooldownAttack = float.PositiveInfinity;
        thisTower.enableAutoattacs = false;

        if (thisTower.type == TowerType.Terraformer)
        {
            MenuController.ShowMenu(true);
            MenuController.ShowDefeat(true);
            return;
        }
        thisTower.DestroyBulletsAndAbilities();
        thisTower.hexagon.SetStatus(HexCoordinatStatus.Attend);
        Destroy(thisTower.gameObject); 
    }

    public void Repair()
    {
        //print(healthRatio);
        if (healthRatio < 1 && !isHeal) { 
            float resource = ResourceManager.resource;
            float towerRepairFactor = ResourceManager.TowerRepairFactor;
            float costNeeded = (1 - healthRatio) * towerRepairFactor;

            prevHealthRatio = healthRatio;

            if(resource > costNeeded)
            {
                maxRepairHealthRatio = 1;
            //    Debug.Log(costNeeded);
               // ResourceManager.RemoveResource(costNeeded);
            }

            else
            {
                maxRepairHealthRatio = healthRatio + resource /towerRepairFactor;
                ResourceManager.RemoveResource(resource);
                //print(maxRepairHealth);
            }

            healBar.fillAmount = maxRepairHealthRatio;



            isHeal = true;
        }
    }

    public float CalculateRepairCost() 
    {
        float resource = ResourceManager.resource;
        float towerRepairFactor = ResourceManager.TowerRepairFactor;
        float costNeeded = (1 - healthRatio) * towerRepairFactor;

        if (resource > costNeeded)
        { return costNeeded; }
        else 
        { return resource; }
    }

}
