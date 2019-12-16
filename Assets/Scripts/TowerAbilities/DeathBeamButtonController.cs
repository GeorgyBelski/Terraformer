using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBeamButtonController : AbilityButtonController
{
    public override void TowerCastSingleTargetAbility(Tower casterTower, Enemy target)
    {
        ((LaserTower)casterTower).CastDeathBeam(target);
    }
}
