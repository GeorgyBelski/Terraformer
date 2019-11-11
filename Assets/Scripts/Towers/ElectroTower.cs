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
    public Transform currentLightningCharge;
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

    [Header("ThandetBallAbility")]
    public ThanderBallAbility thanderBallAbility;

    private new void Start()
    {
        base.Start();
        type = TowerType.Electro;
        currentLightningCharge = Instantiate(lightningChargePrefab, gunpoint.position, gunpoint.rotation).transform;
        SaveOrdinaryLightningChargeSattings();
    }
    void SaveOrdinaryLightningChargeSattings()
    {
        lightningChargeParticleSys = currentLightningCharge.GetComponent<ParticleSystem>();
        particleSystemRenderer = currentLightningCharge.GetComponent<ParticleSystemRenderer>();
        lightningChargeSize = currentLightningCharge.localScale.x;
        lightningChargeParticleSys = currentLightningCharge.GetComponent<ParticleSystem>();
        autoAttackMaterial = Instantiate(particleSystemRenderer.trailMaterial);
        particleSystemRenderer.trailMaterial = autoAttackMaterial;
        //  ordinaryElectroColor1 = autoAttackMaterial.GetColor("_EnergyColor01");
        //  ordinaryElectroColor2 = autoAttackMaterial.GetColor("_EnergyColor02");
        autoAttackMaterial.SetColor("_EnergyColor01", ordinaryElectroColor1);
        autoAttackMaterial.SetColor("_EnergyColor02", ordinaryElectroColor2);
    }

    public override void TowerAttack(Enemy target)
    {    
        if (target && currentLightningCharge)
        {
            currentLightningCharge.position = gunpoint.position;
            enableCharge = true;
            timerChargeLifeTime = chargeLifeTime;
            EnableChargeParticlesEmission(true);
            currentLightningCharge.localScale = Vector3.one * lightningChargeSize;

        }
    }

    internal override void TowerUpdate()
    {
        ChargeControl();

     // ThanderBallControl();
        
    }

 // AutoAttack - Charge   

    void ChargeControl() {
        if (currentLightningCharge)
        {
            if (target && enableCharge)
            {
                fromChargeToTarget = target.GetPosition() - currentLightningCharge.position;
                float distanceFromChargeToTarget = fromChargeToTarget.magnitude;
                currentLightningCharge.position = Vector3.Lerp(gunpoint.position, target.GetPosition(), chargeLerpPosition);
                chargeLerpPosition += lightningLerpSpeed * Time.deltaTime * (range / distanceFromChargeToTarget);


                if (distanceFromChargeToTarget < 0.1)
                {
                    currentLightningCharge.position = target.GetPosition();
                    enableCharge = false;
                    EnableChargeParticlesEmission(false);
                    AddEffects();
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
                        currentLightningCharge.localScale = Vector3.one * lightningChargeSize * 2;
                    }
                    else if (currentLightningCharge.localScale.x > lightningChargeSize)
                    { currentLightningCharge.localScale = Vector3.one * lightningChargeSize; }
                    else
                    { currentLightningCharge.localScale = Vector3.one * lightningChargeSize * 1.6f; }
                    blinkingCounter--;
                }
            }
            else
            {
                currentLightningCharge.localScale /= 2;
            }
                if (timerChargeLifeTime <= 0)
            {
                //   DestroyCharge();
                EnableChargeParticlesEmission(false);
                timerChargeLifeTime = 0;
            }
        }
    }
    void EnableChargeParticlesEmission(bool isEmission)
    {
        var emission = lightningChargeParticleSys.emission;
        emission.enabled = isEmission;
    }
    public void DestroyCharge()
    {
        if (currentLightningCharge)
        {
            Destroy(currentLightningCharge.gameObject);
            currentLightningCharge = null;
            chargeLerpPosition = 0;
        }
    }

    void AddEffects()
    {
        // original Lightning Charge Effect
        randomizer = Random.Range(0, 100);
        if (randomizer <= probabilityOfStan)
        { target.effectsController.AddStun(stunDuration); }
        // symbiosis with LaserTower
        if (symbiosisTowerType == TowerType.Laser)
        { target.effectsController.AddBurning(BurningEffect.standardLifetime/2, 6); }
    }

    public override void ActivateSymbiosisUpgrade()
    {
        symbiosisTowerType = Symbiosis.ActivateElectroSymbiosisUpgrade(this);
    }
    public override void DisableSymbiosisUpgrade()
    {
        //Debug.Log("DisableSymbiosisUpgrade");
        SetOrdinaryLightningCharge();
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

    public override void EndCasting() {
        IsCastingAbility = false;
        TowerManager.availableElectroTowers.Add(this);
    }

}
