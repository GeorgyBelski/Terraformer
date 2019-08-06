using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTAbility1 : AbilityButtonController
{
    public override void TowerCastAreaAbility(Tower casterTower, Vector3 aimAreaPosition)
    {
        ((ElectroTower)casterTower).CastThanderBall(aimArea.position);
    }

}
