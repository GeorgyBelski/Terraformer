using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElectroTower : Tower
{
    [Header("ElectroTower")]
   // public Transform gunpoint;
    

    [Header("AutoAttack")]
    public float lightningLerpSpeed = 100f;
    public int damageAttack = 50;
    public GameObject lightningChargePrefab;
    ParticleSystem lightningChargeParticleSys;
    public Transform currentLightningCharge;
    float lightningChargeSize;
    bool enableCharge;
    Material chargeTrailMaterial;

    float chargeLifeTime = 0.5f;
    float timerChargeLifeTime;
    float chargeLerpPosition;
    Vector3 fromChargeToTarget;

    [Header("ThandetBallAbility")]
    public ThanderBallAbility thanderBallAbility;
/*
    public GameObject thanderBallPrefab;
    public int thanderBallDamage = 100;
    public float thanderBallSpeed = 20f;
    public float thanderBallEffectRadius = 2.19f;
    GameObject thandetBall;
    Material thandetBallTrailMaterial;
    Animator thandetBallAnimator;
    SphereCollider thandetBallCollider;
    Vector3? thandetBallAim;
    //Collider[] hitThanderBallColliders;
    List<Enemy> thanderBallTargets;
*/



    private void Start()
    {
        base.Start();
        type = TowerType.Electro;

     //   thandetBallAim = null;
     //   hitThanderBallColliders = new Collider[10];
    //    thanderBallTargets = new List<Enemy>();
    }
    public override void TowerAttack(Enemy target)
    {
        if (target)
        {
            if (!currentLightningCharge)
            {
                //  DestroyCharge();
                currentLightningCharge = Instantiate(lightningChargePrefab, gunpoint.position, gunpoint.rotation).transform;
                lightningChargeParticleSys = currentLightningCharge.GetComponent<ParticleSystem>();
               chargeTrailMaterial = currentLightningCharge.GetComponent<ParticleSystemRenderer>().trailMaterial;
                chargeTrailMaterial.SetColor("_BaseColor", new Color(5, 5, 5, 1));
                lightningChargeSize = currentLightningCharge.localScale.x;
            }
            else {
                currentLightningCharge.position = gunpoint.position;
            }
            enableCharge = true;
            timerChargeLifeTime = chargeLifeTime;
            EnableChargeParticlesEmission(true);
            currentLightningCharge.localScale = new Vector3(lightningChargeSize, lightningChargeSize, lightningChargeSize);

        }
    }

    internal override void TowerUpdate()
    {
            ChargeControl();
     //   ThanderBallControl();
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
                    //   DestroyCharge();
                    enableCharge = false;
                    EnableChargeParticlesEmission(false);
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
    void DestroyCharge()
    {
        if (currentLightningCharge)
        {
            Destroy(currentLightningCharge.gameObject);
            currentLightningCharge = null;
            chargeLerpPosition = 0;
        }
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
