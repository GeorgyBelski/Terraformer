using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [ColorUsageAttribute(true, true)]
    public Color ordinaryLaserColor1, ordinaryLaserColor2, laserSymbColor, electroSymbColor1, electroSymbColor2;
    [ColorUsageAttribute(true, true)]
    [HideInInspector]
    public Color currentColor1, currentColor2;

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
        currentColor1 = ordinaryLaserColor1;
        currentColor2 = ordinaryLaserColor2;
    }
    public override void TowerAttack(Enemy target)
    {
        if (target) {
            timerDuration = beamDuration;
            lr.SetPosition(0, gunpoint.position);
            lr.SetPosition(1, target.GetPosition());
            lr.material.SetColor("_EnergyColor01", currentColor1);
            lr.material.SetColor("_EnergyColor02", currentColor2);

            target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
            AddEffects();
        }
    }

    private void AddEffects()
    {
        target.effectsController.AddBurning(BurningEffect.standardLifetime, damageBurning);
        if (symbiosisTowerType == TowerType.Electro)
        {
            randomizer = Random.Range(0, 100);
            if(randomizer <= ((ElectroTower)symbiosisTower).probabilityOfStan/2)
            {
                target.effectsController.AddStun(1);
            }
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
            lrMaterial.SetColor("_EnergyColor01", new Color(currentColor1.r, currentColor1.g - 1 + ratioDuration, currentColor1.b, ratioDuration));
            lrMaterial.SetColor("_EnergyColor02", new Color(currentColor2.r, currentColor2.g, currentColor2.b, ratioDuration));
            lr.widthMultiplier = ratioDuration;
        }
        else
        {
            lr.enabled = false;
        }
    }

    public override void ActivateSymbiosisUpgrade()
    {
        symbiosisTowerType = Symbiosis.ActivateLaserSymbiosisUpgrade(this);
    }

    public override void DisableSymbiosisUpgrade()
    {
        SetOrdinaryLaserRay();
        symbiosisTowerType = null;
    }
    void SetOrdinaryLaserRay()
    {
        currentColor1 = ordinaryLaserColor1;
        currentColor2 = ordinaryLaserColor2;
        if (cooldownAttack != ordinaryCooldownAttack)
        { cooldownAttack = ordinaryCooldownAttack; }
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
