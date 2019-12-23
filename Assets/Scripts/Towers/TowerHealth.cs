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
            if (((ElectroTower)thisTower).thanderBallAbility.thandetBall)
            { Destroy(((ElectroTower)thisTower).thanderBallAbility.thandetBall.gameObject); }
        }
        else if (thisTower.type == TowerType.Laser)
        {
            ((LaserTower)thisTower).lr.enabled = false;
            if (((LaserTower)thisTower).scorchingRayAbility.scorchingRay)
            { Destroy(((LaserTower)thisTower).scorchingRayAbility.scorchingRay.gameObject); }
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
            // ((Terraformer)thisTower).menu.SetActive(true);
            MenuController.ShowMenu(true);
            MenuController.ShowDefeat(true);
            //((Terraformer)thisTower).defeat.gameObject.SetActive(true);
            return;
        }
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
