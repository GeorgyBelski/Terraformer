using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLazerTower : Upgrade
{
    public LaserTower thisLazerTower;
    public float burningDamageMultiplyer = 1.5f;

    //private float damageMultiplyer;
    void Start()
    {
        base.thisTower = thisLazerTower;
       // damageMultiplyer = base.damageMultiplayer;
    }

    public override void upgradeTower()
    {
        base.upgradeTower();
        //float newFloat = thisLazerTower.damageAttack;
        thisLazerTower.damageAttack = (int)(thisLazerTower.damageAttack * damageMultiplayer);
        thisLazerTower.damageBurning = (int)(thisLazerTower.damageBurning * burningDamageMultiplyer);

        thisLazerTower.upgrade();
    }
}
