﻿using System;
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



    public LineRenderer lr;
    [HideInInspector]
    public Material lrMaterial;
    [ColorUsageAttribute(true, true)]
    public Color ordinaryLaserColor1, ordinaryLaserColor2, laserSymbColor, electroSymbColor1, electroSymbColor2, plasmaSymbColor1, plasmaSymbColor2;
    [ColorUsageAttribute(true, true)]
    [HideInInspector]
    public Color currentColor1, currentColor2;
    public float[] lrWidthKeys;

    public GameObject areaDamagePrefab;
    GameObject areaDamager;
    public int symbiosisAreaDamage = 30;
    LaserAreaDamageController areaDamageController;

    [Header("DeathBeamAbility")]
    public DeathBeamAbility deathBeamAbility;
    [Header("ScorchingRayAbility")]
    public ScorchingRayAbility scorchingRayAbility;
    [Header("LightningStrikeAbility")]
    public LightningStrikeAbility lightningStrikeAbility;


    //  [Space]



    [Header("UpgradeAbilityCast")]
    private bool isUpgraided;
    public LaserBlowUpCast cast;
    public float castSize = 5f;
    public float castDamage = 50f;
    public float castBlowUpSize = 2f;
    public float castBlowUpDamage = 25f;

    public float castCooldown = 20f;
    private float realCastTime;



    private new void Start()
    {
        cast.gameObject.active = false;
        realCastTime = castCooldown;
        base.Start();
        type = TowerType.Laser;

        lr = gunpoint.GetComponent<LineRenderer>();
        lrMaterial = lr.material;
        if (!lr)
        {
            lr = gunpoint.gameObject.AddComponent<LineRenderer>();
        }

        lrWidthKeys = new float[lr.widthCurve.keys.Length];
        for (int i = 0; i < lrWidthKeys.Length; i++)
        {
            lrWidthKeys[i] = lr.widthCurve.keys[i].value;
        }
        currentColor1 = ordinaryLaserColor1;
        currentColor2 = ordinaryLaserColor2;

        areaDamager = Instantiate(areaDamagePrefab);
        areaDamageController = areaDamager.GetComponent<LaserAreaDamageController>();
        areaDamageController.thisTower = this;
        areaDamager.SetActive(false);


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
        target.effectsController.AddBurning(Effect.burningDuration, Effect.burningDamage);
        if (symbiosisTowerType == TowerType.Electro)
        {
            randomizer = Random.Range(0, 100);
            if (randomizer <= ((ElectroTower)symbiosisTower).probabilityOfStan)
            {
                target.effectsController.AddStun(((ElectroTower)symbiosisTower).stunDuration / 2);
            }
        }
        else if (symbiosisTowerType == TowerType.Plasma)
        {
            ApplyAreaDamage();
        }
    }
    void ApplyAreaDamage() 
    {
        areaDamageController.BurningDuration = Effect.burningDuration;
        areaDamageController.damageBurning = damageBurning;
        areaDamageController.damageHit = symbiosisAreaDamage;
        areaDamager.transform.position = target.GetPosition();
        areaDamager.SetActive(true);
    }
    internal override void TowerUpdate()
    {
        if (isUpgraided)
        {
            realCastTime -= Time.deltaTime;

            if(realCastTime <= 0)
            {
                cast.gameObject.active = true;
                realCastTime = castCooldown;
            }
        }
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
        symbiosisTowerType = Symbiosis.ActivateSymbiosisUpgrade(this);
    }

    public override void DisableSymbiosisUpgrade()
    {
        SetOrdinaryLaserRay();
        if (symbiosisTowerType == TowerType.Electro)
        {
            TowerManager.availableElectroLaserTowers.Remove(this);
        }
        else if (symbiosisTowerType == TowerType.Plasma) 
        {
            TowerManager.availableLaserPlasmaTowers.Remove(this);
        }
        symbiosisTowerType = null;
    }
    void SetOrdinaryLaserRay()
    {
        currentColor1 = ordinaryLaserColor1;
        currentColor2 = ordinaryLaserColor2;
        if (cooldownAttack != ordinaryCooldownAttack)
        { cooldownAttack = ordinaryCooldownAttack; }
    }

    
    // Ability 1  - DeathBeam
    public void CastDeathBeam(Enemy target)
    {
        deathBeamAbility.Cast(target);
    }

    // Ability Laser-Plasma - ScorchingRay
    public void CastScorchingRay(Vector3 aimPosition)
    {
        scorchingRayAbility.Cast(aimPosition);
    }
    // Ability Electro-Laser - LightningStrike
    public void CastLightningStrike(Vector3 aimPosition)
    {
        lightningStrikeAbility.Cast(aimPosition);
    }

    public override void EndCasting()
    {
        base.EndCasting();
        IsCastingAbility = false;
        //TowerManager.availableLaserTowers.Add(this);
    }
    override public void DestroyBulletsAndAbilities() 
    {
        lr.enabled = false;
        if (lightningStrikeAbility.lightningStrike) { Destroy(lightningStrikeAbility.lightningStrike.gameObject); }
        if (scorchingRayAbility.scorchingRay) { Destroy(scorchingRayAbility.scorchingRay.gameObject); }
        Destroy(areaDamager);
    }
    public void upgrade()
    {
        
        isUpgraided = true;
        cast.Set(castSize, castDamage, castBlowUpSize, castBlowUpDamage, damageBurning);


    }
    
}
