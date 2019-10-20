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
    public ParticleSystem lightningChargeParticleSys;
    public Transform currentLightningCharge;
    float lightningChargeSize;
    bool enableCharge;
    public ParticleSystemRenderer particleSystemRenderer;
    public Material ordinaryTrailMaterial, laserSymbiosisMaterial, plasmaSymbiosisMaterial; 


    float chargeLifeTime = 0.5f;
    float timerChargeLifeTime;
    float chargeLerpPosition;
    Vector3 fromChargeToTarget;
    int randomizer;
    [Range(1,100)]
    public int probabilityOfStan = 10;

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
    }

    public override void TowerAttack(Enemy target)
    {
        if (target)
        {
            currentLightningCharge.position = gunpoint.position;
            enableCharge = true;
            timerChargeLifeTime = chargeLifeTime;
            EnableChargeParticlesEmission(true);
            currentLightningCharge.localScale = new Vector3(lightningChargeSize, lightningChargeSize, lightningChargeSize);

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
                    currentLightningCharge.localScale *= 1.7f;
                    enableCharge = false;
                    EnableChargeParticlesEmission(false);
                    AddEffects();
                    target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
                    chargeLerpPosition = 0;
                }
            }

            timerChargeLifeTime -= Time.deltaTime;
            if (timerChargeLifeTime < 0.1 && currentLightningCharge.localScale.x > 0.1f)
            {
                currentLightningCharge.localScale /= 6;
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
        { target.effectsController.AddStun(1); }
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
        Debug.Log("DisableSymbiosisUpgrade");
        SetOrdinaryLightningCharge();
        symbiosisTowerType = null;
    }
    public void SetOrdinaryLightningCharge()
    {
        particleSystemRenderer.trailMaterial = ordinaryTrailMaterial;
        var trails = lightningChargeParticleSys.trails;
        trails.ribbonCount = 2;
        lightningLerpSpeed = 2;
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
