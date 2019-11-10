using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaClusterBombAbilityButton : AbilityButtonController
{
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        ((PlazmaTower)casterTower).CastClusterBomb(aimArea.position);
    }
}
