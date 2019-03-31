using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroTower : Tower
{
    public Transform gunpoint;
    public float lightningSpeed = 100f;
    public int damageAttack = 50;
    public GameObject lightningCharge;
    public Transform currentLightningCharge;
    public float explosionTime = 0.4f;

    float chargeLifeTime = 0.5f;
    float timerChargeLifeTime;
    float chargeLerpPosition;
    Vector3 fromChargeToTarget;

    private void Start()
    {

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
        if (currentLightningCharge)
        { 
            timerChargeLifeTime -= Time.deltaTime;
            if (timerChargeLifeTime <= 0)
            {
                DestroyCharge();
            }
            if (target)
            {
                try {
                    fromChargeToTarget = target.GetPosition() - currentLightningCharge.position;
                    float distanceFromChargeToTarget = fromChargeToTarget.magnitude;
                    currentLightningCharge.position = Vector3.Lerp(gunpoint.position, target.GetPosition(), chargeLerpPosition);
                    chargeLerpPosition += lightningSpeed * Time.deltaTime * (range / distanceFromChargeToTarget);
                    if (distanceFromChargeToTarget < 0.2) {
                        currentLightningCharge.position = target.GetPosition();
                        DestroyCharge();
                        target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
                        chargeLerpPosition = 0;
                    }
                }
                catch (Exception e) {
                    Debug.Log("Exception in TowerUpdate: " + e);
                }
            }
        }
    }

    void DestroyCharge() {
        Destroy(currentLightningCharge.gameObject);
        currentLightningCharge = null;
        chargeLerpPosition = 0;
    }
}
