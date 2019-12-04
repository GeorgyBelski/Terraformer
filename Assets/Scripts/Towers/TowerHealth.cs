using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TowerHealth : Damageable
{
    public Tower thisTower;

    public Color damagePointsColor = Color.red;

    private float prevHealthRatio;
        
    void Start()
    {
        isHeal = false;
        for (int i = 0; i < damagePoints.Length; i++)
        { damagePoints[i].color = damagePointsColor; }
    }

    void Update()
    {
        base.CalcHealthRatio();

        if (isHeal && healthRatio < maxRepairHealth)
        {
            //base.CalcHealthRatio();
            print(maxRepairHealth);
            base.health += (int)((maxHealth/15) * Time.deltaTime);
            
            //base.CalcHealthRatio();
        }
        else
        {
            if(healHealth)
                healHealth.fillAmount = healthBar.fillAmount;
            //ealHealth.fillAmount = healthBar.fillAmount;
            isHeal = false;
        }
            
        
        
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
        if (thisTower.currentVisualLink) { Destroy(thisTower.currentVisualLink.gameObject); }
        RemoveFromList();
        //Destroy(thisTowet.gameObject);

        thisTower.cooldownAttack = float.PositiveInfinity;
        //    thisTower.timerAttack = float.PositiveInfinity;
        thisTower.enableAutoattacs = false;

        if (thisTower.type == TowerType.Electro)
        {
            ((ElectroTower)thisTower).DestroyCharge();
            Destroy(((ElectroTower)thisTower).thanderBallAbility.thandetBall.gameObject);
        }
        else if (thisTower.type == TowerType.Laser)
        {
            ((LaserTower)thisTower).lr.enabled = false;
            Destroy(((LaserTower)thisTower).scorchingRayAbility.scorchingRay.gameObject);
        }
        else if (thisTower.type == TowerType.Plasma) 
        {
            ((PlasmaTower)thisTower).DestroyBullets();
            ((PlasmaTower)thisTower).clusterBombAbility.DestroyBullets();
            PlasmaBlowUp blow = ((PlasmaTower)thisTower).blow;
            if (blow.gameObject.activeSelf)
            { blow.thisTower = null; }
        }
        else if (thisTower.type == TowerType.Terraformer)
        {
            ((Terraformer)thisTower).menu.SetActive(true);
            ((Terraformer)thisTower).defeat.gameObject.SetActive(true);
            return;
        }

        Destroy(thisTower.gameObject);


    }

    public void Repair()
    {
        //print(healthRatio);
        if (healthRatio < 1 && !isHeal) { 
            float resource = ResourceManager.resource;
            float repaircost = ResourceManager.RepairCost;
            float costNeeded = (1 - healthRatio) * 100 * repaircost;

            prevHealthRatio = healthRatio;

            if(resource > costNeeded)
            {
                maxRepairHealth = 1;
            //    Debug.Log(costNeeded);
                ResourceManager.RemoveResource(costNeeded);
            }

            else
            {
                maxRepairHealth = healthRatio + resource / 100 / repaircost;
                ResourceManager.RemoveResource(resource);
                //print(maxRepairHealth);
            }

            healHealth.fillAmount = maxRepairHealth;



            isHeal = true;
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
