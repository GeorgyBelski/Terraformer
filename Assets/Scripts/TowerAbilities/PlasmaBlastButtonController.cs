using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBlastButtonController : AbilityButtonController
{
    protected override void ButtonAvailabilityControl()
    {
        if (TowerManager.availableElectroPlasmaTowers.Count == 0)
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
        { ((ElectroTower)casterTower).CastPlasmaBlast(aimArea.position); }
        else
        { ((PlasmaTower)casterTower).CastPlasmaBlast(aimArea.position); }
    }
}
