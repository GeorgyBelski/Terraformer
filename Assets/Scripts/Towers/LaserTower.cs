using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [Space]
    [Header("LaserTower")]
    //   public Transform gunpoint;
    [Header("AutoAttack")]
    public float beamDuration = 0.3f;
    public int damageAttack = 50;
    public int damageBurning = 5;
    float timerDuration;
    LineRenderer lr;
    Material lrMaterial;
    Color startLaser, endLaser;

    [Header("ThandetBallAbility")]
    public ScorchingRayAbility scorchingRayAbility;

    private void Start()
    {
        base.Start();
        type = TowerType.Laser;

        lr = gunpoint.GetComponent<LineRenderer>();
        lrMaterial = lr.material;
        if (!lr)
        {
            lr = gunpoint.gameObject.AddComponent<LineRenderer>();
        }

        startLaser = lr.startColor;
        endLaser = lr.endColor;
    }
    public override void TowerAttack(Enemy target)
    {
        if (target) {
            timerDuration = beamDuration;
            lr.SetPosition(0, gunpoint.position);
            lr.SetPosition(1, target.GetPosition());
            lr.startColor = startLaser;
            lr.endColor = endLaser;

            target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
            target.effectsController.AddBurning(BurningEffect.standardLifetime, damageBurning);
        }
    }

    internal override void TowerUpdate()
    {
        if (timerDuration > 0)
        {
            float ratioDuration = timerDuration / beamDuration;
            timerDuration -= Time.deltaTime;
            lr.enabled = true;

          //  lr.startColor = new Color(lr.startColor.r, lr.startColor.g - 1 + ratioDuration, lr.startColor.b, ratioDuration);
            //     lr.endColor = new Color(lr.startColor.r, lr.startColor.g - 1 + ratioDuration, lr.startColor.b, ratioDuration);
            lrMaterial.SetColor("_BaseColor", new Color(lr.startColor.r, lr.startColor.g - 1 + ratioDuration, lr.startColor.b, ratioDuration));
            lrMaterial.SetColor("_EmissionColor", new Color(1 + ratioDuration, 0, 0));
            lr.widthMultiplier = ratioDuration;
        }
        else
        {
            lr.enabled = false;
        }
    }

    public override void ActivateSymbiosisUpgrade()
    {
        
    }

    public override void DisableSymbiosisUpgrade()
    {

    }

    // Ability 1  - ScorchingRay
    public void CastScorchingRay(Vector3 aimPosition)
    {
        scorchingRayAbility.Cast(aimPosition);
    }

    public override void EndCasting()
    {
        IsCastingAbility = false;
        TowerManager.availableLaserTowers.Add(this);
    }

    
}
