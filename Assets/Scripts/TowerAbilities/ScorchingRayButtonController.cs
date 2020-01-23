using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchingRayButtonController : AbilityButtonController
{
    protected override void ButtonAvailabilityControl() 
    {
        if (TowerManager.availableLaserPlasmaTowers.Count == 0)
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
        if (casterTower.type == TowerType.Laser)
        { ((LaserTower)casterTower).CastScorchingRay(aimArea.position); }
        else 
        { ((PlasmaTower)casterTower).CastScorchingRay(aimArea.position); }
    }

}
