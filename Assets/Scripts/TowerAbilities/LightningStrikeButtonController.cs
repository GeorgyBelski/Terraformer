using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeButtonController : AbilityButtonController
{
    protected override void ButtonAvailabilityControl()
    {
        if (TowerManager.availableElectroLaserTowers.Count == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        if (casterTower.type == TowerType.Electro)
        { ((ElectroTower)casterTower).CastLightningStrike(aimArea.position); }
        else
        { ((LaserTower)casterTower).CastLightningStrike(aimArea.position); }
    }
}
