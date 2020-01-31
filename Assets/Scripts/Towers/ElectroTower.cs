using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElectroTower : Tower
{
    [Header("ElectroTower")]
   // public Transform gunpoint;
    

    [Header("AutoAttack")]
    public float lightningLerpSpeed = 2f;
    public int damageAttack = 50;
    public GameObject lightningChargePrefab;
    [HideInInspector]
    public ParticleSystem lightningChargeParticleSys;
    int blinkingCounter;
    [HideInInspector]
    public Transform lightningCharge;
    float lightningChargeSize;
    bool enableCharge;
    [HideInInspector]
    public ParticleSystemRenderer particleSystemRenderer;
    [HideInInspector]
    public Material autoAttackMaterial;
    [ColorUsageAttribute(true, true)]
    public Color ordinaryElectroColor1, ordinaryElectroColor2, electroSymbColor, laserSymbColor1, laserSymbColor2, plasmaSymbColor1, plasmaSymbColor2;
    [ColorUsageAttribute(true, true)]
    [HideInInspector]
    public Color currentColor1, currentColor2;

    float chargeLifeTime = 0.5f;
    float timerChargeLifeTime;
    float chargeLerpPosition;
    Vector3 fromChargeToTarget;

    [Range(1,100)]
    public int probabilityOfStan = 10;
    public float stunDuration = 2f;
    public GameObject areaDamagePrefab;
    GameObject areaDamager;
    public int symbiosisAreaDamage = 30;
    ElectroAreaDamageController areaDamageController;

    [Header("ThandetBallAbility")]
    public ThanderBallAbility thanderBallAbility;
    [Header("LightningStrikeAbility")]
    public LightningStrikeAbility lightningStrikeAbility;
    [Header("PlasmaBlastAbility")]
    public PlasmaBlastAbility plasmaBlastAbility;
    private new void Start()
    {
        base.Start();
        type = TowerType.Electro;
        lightningCharge = Instantiate(lightningChargePrefab, gunpoint.position, gunpoint.rotation).transform;
        SaveOrdinaryLightningChargeSattings();
        areaDamager = Instantiate(areaDamagePrefab);
        areaDamageController = areaDamager.GetComponent<ElectroAreaDamageController>();
        areaDamageController.thisTower = this;
        areaDamager.SetActive(false);
    }
    void SaveOrdinaryLightningChargeSattings()
    {
        lightningChargeParticleSys = lightningCharge.GetComponent<ParticleSystem>();
        particleSystemRenderer = lightningCharge.GetComponent<ParticleSystemRenderer>();
        lightningChargeSize = lightningCharge.localScale.x;
        lightningChargeParticleSys = lightningCharge.GetComponent<ParticleSystem>();
        autoAttackMaterial = Instantiate(particleSystemRenderer.trailMaterial);
        particleSystemRenderer.trailMaterial = autoAttackMaterial;
        //  ordinaryElectroColor1 = autoAttackMaterial.GetColor("_EnergyColor01");
        //  ordinaryElectroColor2 = autoAttackMaterial.GetColor("_EnergyColor02");
        autoAttackMaterial.SetColor("_EnergyColor01", ordinaryElectroColor1);
        autoAttackMaterial.SetColor("_EnergyColor02", ordinaryElectroColor2);
    }

    public override void TowerAttack(Enemy target)
    {    
        if (target && lightningCharge)
        {
            lightningCharge.position = gunpoint.position;
            enableCharge = true;
            timerChargeLifeTime = chargeLifeTime;
            EnableChargeParticlesEmission(true);
            lightningCharge.localScale = Vector3.one * lightningChargeSize;

        }
    }

    internal override void TowerUpdate()
    {
        ChargeControl();

     // ThanderBallControl();
        
    }

 // AutoAttack - Charge   

    void ChargeControl() {
        if (target && enableCharge)
        {
            fromChargeToTarget = target.GetPosition() - lightningCharge.position;
            float distanceFromChargeToTarget = fromChargeToTarget.magnitude;
            lightningCharge.position = Vector3.Lerp(gunpoint.position, target.GetPosition(), chargeLerpPosition);
            chargeLerpPosition += lightningLerpSpeed * Time.deltaTime * (range / distanceFromChargeToTarget);


            if (distanceFromChargeToTarget < 0.1)
            {
                lightningCharge.position = target.GetPosition();
                enableCharge = false;
                EnableChargeParticlesEmission(false);
                AddEffects(); //<- Symbiosi Area Damage 
                target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
                chargeLerpPosition = 0;
                blinkingCounter = 3;
            }
        }

        timerChargeLifeTime -= Time.deltaTime;
        if (timerChargeLifeTime > 0.1)
        {
            if(chargeLerpPosition == 0 && blinkingCounter > 0)
            {
                if(blinkingCounter == 3)
                {
                    lightningCharge.localScale = Vector3.one * lightningChargeSize * 2;
                }
                else if (lightningCharge.localScale.x > lightningChargeSize)
                { lightningCharge.localScale = Vector3.one * lightningChargeSize; }
                else
                { lightningCharge.localScale = Vector3.one * lightningChargeSize * 1.6f; }
                blinkingCounter--;
            }
        }
        else
        {
            lightningCharge.localScale /= 2;
        }
            if (timerChargeLifeTime <= 0)
        {
            //   DestroyCharge();
            EnableChargeParticlesEmission(false);
            chargeLerpPosition = 0;
            timerChargeLifeTime = 0;
        }
    }
    void EnableChargeParticlesEmission(bool isEmission)
    {
        var emission = lightningChargeParticleSys.emission;
        emission.enabled = isEmission;
    }
    public void DestroyCharge()
    {
        if (lightningCharge)
        {
            Destroy(lightningCharge.gameObject);
            lightningCharge = null;
            chargeLerpPosition = 0;
        }
    }

    void AddEffects()
    {
        // original Lightning Charge Effect
        randomizer = Random.Range(0, 100);
        bool isStun = randomizer <= probabilityOfStan;
        if (isStun)
        { target.effectsController.AddStun(stunDuration); }
        // symbiosis with LaserTower
        if (symbiosisTowerType == TowerType.Laser)
        { target.effectsController.AddBurning(Effect.burningDuration / 2, Effect.burningDamage); }
        else if(symbiosisTowerType == TowerType.Plasma)
        { ApplyAreaDamage(symbiosisAreaDamage, isStun, stunDuration); }
    }
    void ApplyAreaDamage(int damage, bool isStun, float stunDuration = 0) 
    {   
        if (isStun)
        {  
            areaDamageController.stunDuration = stunDuration; 
        }
        areaDamageController.damage = damage;
        areaDamager.transform.position = lightningCharge.position;
        areaDamager.SetActive(true);
    }
    public override void ActivateSymbiosisUpgrade()
    {
        symbiosisTowerType = Symbiosis.ActivateSymbiosisUpgrade(this);
    }
    public override void DisableSymbiosisUpgrade()
    {
        //Debug.Log("DisableSymbiosisUpgrade");
        SetOrdinaryLightningCharge();
        if (symbiosisTowerType == TowerType.Laser)
        {
            TowerManager.availableElectroLaserTowers.Remove(this);
        }
        else if (symbiosisTowerType == TowerType.Plasma)
        {
            TowerManager.availableElectroPlasmaTowers.Remove(this);
        }
        symbiosisTowerType = null;
    }
    public void SetOrdinaryLightningCharge()
    {
        //   particleSystemRenderer.trailMaterial = ordinaryTrailMaterial;
        autoAttackMaterial.SetColor("_EnergyColor01", ordinaryElectroColor1);
        autoAttackMaterial.SetColor("_EnergyColor02", ordinaryElectroColor2);
        var trails = lightningChargeParticleSys.trails;
        trails.ribbonCount = 2;
        lightningLerpSpeed = 2;
        if (cooldownAttack != ordinaryCooldownAttack)
        { cooldownAttack = ordinaryCooldownAttack; }
    }

    // Ability 1  - ThanderBall

    public void CastThanderBall(Vector3 aimPosition)
    {
        thanderBallAbility.Cast(aimPosition);
    }

    // Ability Electro-Laser - LightningStrike
    public void CastLightningStrike(Vector3 aimPosition)
    {
        lightningStrikeAbility.Cast(aimPosition);
    }
    // Ability Electro-Plasma - PlasmaBlast
    public void CastPlasmaBlast(Vector3 aimPosition)
    {
        plasmaBlastAbility.Cast(aimPosition);
    }

    public override void EndCasting() {
        base.EndCasting();
        IsCastingAbility = false;
       // TowerManager.availableElectroTowers.Add(this);
    }
    override public void DestroyBulletsAndAbilities()
    {
        DestroyCharge();
        if (lightningStrikeAbility.lightningStrike) { Destroy(lightningStrikeAbility.lightningStrike.gameObject); }
        if (thanderBallAbility.thanderBall) { Destroy(thanderBallAbility.thanderBall.gameObject); }
        Destroy(areaDamager);
    }
}
