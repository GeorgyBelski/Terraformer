using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTAbility1 : AbilityButtonController
{
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        ((ElectroTower)casterTower).CastThanderBall(aimArea.position);
    }

}
