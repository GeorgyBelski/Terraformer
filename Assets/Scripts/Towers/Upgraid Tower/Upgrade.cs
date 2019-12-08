using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour
{
    public Button thisButton;
    protected Tower thisTower;

    public int maxTowerHealthAddition = 1000;
    public float attackSpeedMultiplier = 1.2f;
    public float damageMultiplayer = 1.5f;
    public Material towerMaterial;

    public virtual void upgradeTower() {
        if(thisTower.towerMaterial.GetFloat("_Float_Upgrade") != 1)
        {
            thisTower.towerMaterial.SetFloat("_Float_Upgrade", 1);
            
            //thisTower.d
        }
        thisTower.towerHealth.maxHealth += maxTowerHealthAddition;
        thisTower.towerHealth.health += maxTowerHealthAddition;
        thisTower.cooldownAttack /= attackSpeedMultiplier;
        thisButton.enabled = false;
    }
}
