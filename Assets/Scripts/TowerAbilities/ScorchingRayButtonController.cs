using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchingRayButtonController : AbilityButtonController
{
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        ((LaserTower)casterTower).CastScorchingRay(aimArea.position);
    }

}
