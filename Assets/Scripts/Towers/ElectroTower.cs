using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElectroTower : Tower
{
    [Header("ElectroTower")]
    public Transform gunpoint;
    

    [Header("AutoAttack")]
    public float lightningLerpSpeed = 100f;
    public int damageAttack = 50;
    public GameObject lightningCharge;
    public Transform currentLightningCharge;

    float chargeLifeTime = 0.5f;
    float timerChargeLifeTime;
    float chargeLerpPosition;
    Vector3 fromChargeToTarget;

    [Header("ThandetBall")]
    public GameObject thanderBallPrefab;
    public float thanderBallSpeed = 20f;
    public float thanderBallEffectRadius = 2.19f;
    GameObject thandetBall;
    Animator thandetBallAnimator;
    SphereCollider thandetBallCollider;
    Vector3 thandetBallAim;
    Collider[] hitThanderBallColliders;


    float previousDistanceToAim;
    public float explosionTime = 0.4f;
    float timerExplosionTime;


    private void Start()
    {

        type = TowerType.Electro;
        thandetBallAim = Vector3.down;
        hitThanderBallColliders = new Collider[10];
    }
    public override void TowerAttack(Enemy target)
    {
        if (target)
        {
            if (currentLightningCharge) {
                DestroyCharge();
            }
            currentLightningCharge = Instantiate(lightningCharge, gunpoint.position, gunpoint.rotation).transform;
            timerChargeLifeTime = chargeLifeTime;


        }
    }

    internal override void TowerUpdate()
    {
            ChargeControl();
        ThanderBallControl();
    }

 // AutoAttack - Charge   

    void ChargeControl() {
        if (currentLightningCharge)
        {
            if (target)
            {
                fromChargeToTarget = target.GetPosition() - currentLightningCharge.position;
                float distanceFromChargeToTarget = fromChargeToTarget.magnitude;
                currentLightningCharge.position = Vector3.Lerp(gunpoint.position, target.GetPosition(), chargeLerpPosition);
                chargeLerpPosition += lightningLerpSpeed * Time.deltaTime * (range / distanceFromChargeToTarget);
                if (distanceFromChargeToTarget < 0.2)
                {
                    currentLightningCharge.position = target.GetPosition();
                    DestroyCharge();
                    target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
                    chargeLerpPosition = 0;
                }
            }

            timerChargeLifeTime -= Time.deltaTime;
            if (timerChargeLifeTime <= 0)
            {
                DestroyCharge();
                timerChargeLifeTime = 0;
            }
        }
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

    public void CastThanderBall( Vector3 aimPosition) {
        if (IsCastingAbility == true) {
            return;
        }
        
        TowerManager.availableElectroTowers.Remove(this);
        thandetBallAim = aimPosition;
        cannon.LookAt(thandetBallAim);
        IsCastingAbility = true;
        Vector3 offsetFromCannon = gunpoint.position - cannon.position;
        if (!thandetBall)
        {
            thandetBall = Instantiate(thanderBallPrefab, gunpoint.position + offsetFromCannon/2, cannon.rotation);
            thandetBallAnimator = thandetBall.GetComponentInChildren<Animator>();
            thandetBallCollider = thandetBall.GetComponentInChildren<SphereCollider>();
            thandetBallCollider.enabled = false;
        }
        else {
            thandetBall.transform.position = gunpoint.position + offsetFromCannon/2;
            thandetBall.transform.rotation = cannon.rotation;
            thandetBallAnimator.SetBool("isReachAim", false);
        }
        


    }
    void ThanderBallControl() {
        if (thandetBallAim != Vector3.down) { // analog to  'thandetBallAim != null';
            if (IsCastingAbility) {
                Vector3 ofsetFromCannon = gunpoint.position - cannon.position;
                thandetBall.transform.position += ofsetFromCannon * thanderBallSpeed * Time.deltaTime / 32;
                previousDistanceToAim = float.PositiveInfinity;
            }
            else{
                Vector3 toAim = thandetBallAim - thandetBall.transform.position;
                float distanceTOAim = toAim.magnitude;
                if (distanceTOAim > 0.15f && previousDistanceToAim > distanceTOAim)
                {
                    previousDistanceToAim = distanceTOAim;
                    thandetBall.transform.position += toAim.normalized * thanderBallSpeed * Time.deltaTime;
                }
                else {
                    thandetBall.transform.position = thandetBallAim;
                    thandetBallAnimator.SetBool("isReachAim", true);
                    ApplyThanderBallEffects(thandetBallAim, thanderBallEffectRadius );
                    thandetBallAim = Vector3.down; // analog to  'thandetBallAim = null';
                }
                
            }

        }
    }

    bool debug_draw;
    Vector3 center;
    float size;

    private void ApplyThanderBallEffects(Vector3 center, float radius)
    {      
        int hittedEnemysNumber = Physics.OverlapSphereNonAlloc(center, radius, hitThanderBallColliders, EnemyManagerPro.enemyLayerMask);

        {
            debug_draw = true;
            this.center = center;
            this.size = radius;

        }
        Debug.Log("hittedEnemysNumber: " + hittedEnemysNumber);
        for (int i=0; i < hittedEnemysNumber; i++)
        {
            //  hitColliders[i].SendMessage("AddDamage");
            //    Debug.Log(hitThanderBallColliders[i]);
            hitThanderBallColliders[i].GetComponent<EnemyEffectsController>().AddStan(2);
            hitThanderBallColliders[i] = null;
        }
    }

    public void EndCasting() {
        IsCastingAbility = false;
        TowerManager.availableElectroTowers.Add(this);
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(center, size);
    }
    */
}
