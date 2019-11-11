using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaClusterBombAbilityButton : AbilityButtonController
{
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        ((PlasmaTower)casterTower).CastClusterBomb(aimArea.position);
    }
}
