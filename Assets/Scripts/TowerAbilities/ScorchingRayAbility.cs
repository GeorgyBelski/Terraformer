﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchingRayAbility : TowerAbility
{
    [Header("ScorchingRay")]
    public GameObject scorchingRayPrefab;

    public float speed = 20f;
    public float effectRadius = 2.19f;

    GameObject scorchingRay;
    Material trailMaterial;
    Animator animator;
    Vector3? aim;
    Vector3 toAimNormalized;
    List<Enemy> targets;

    float previousDistanceToAim;


    new void Start()
    {
        base.Start();
        aim = null;
        targets = new List<Enemy>();
    }

    public new void Cast(Vector3 aimPosition)
    {
        base.Cast(aimPosition);

        TowerManager.availableLaserTowers.Remove((LaserTower)tower);
        aim = aimPosition;
        //   cannon.LookAt((Vector3)thandetBallAim);
        tower.RotateCannon((Vector3)aim);

        Vector3 offsetFromCannon = gunpoint.position - cannon.position;
        if (!scorchingRay)
        {
            scorchingRay = Instantiate(scorchingRayPrefab, gunpoint.position, Quaternion.identity);
            animator = scorchingRay.GetComponent<Animator>();
            //    thandetBallCollider = thandetBall.GetComponentInChildren<SphereCollider>();
            //    thandetBallCollider.enabled = false;

         //   trailMaterial = scorchingRay.GetComponentInChildren<ParticleSystemRenderer>().trailMaterial;
            //   Debug.Log("thandetBallTrailMaterial: " + thandetBallTrailMaterial);
          //  trailMaterial.SetColor("_BaseColor", new Color(5, 5, 5, 1));
        }
        else
        {
            scorchingRay.transform.position = gunpoint.position;
        //    scorchingRay.transform.rotation = cannon.rotation;
            animator.SetBool("isReachAim", false);
        }
        toAimNormalized = ((Vector3)aim - scorchingRay.transform.position).normalized;

    }

    void Update()
    {
        ScorchingRayControl();
    }

    void ScorchingRayControl()
    {
        if (aim != null)
        {
            base.Control();//<- CastingControl(), <- ShootingControl()
        }
    }
    public override void CastingControl()
    {
        previousDistanceToAim = float.PositiveInfinity;
        timerCast -= Time.deltaTime;
        if (timerCast <= 0)
        {
            tower.EndCasting();
        }
    }

    public override void ShootingControl()
    {
        Vector3 toAim = (Vector3)aim - scorchingRay.transform.position;
        float distanceToAim = toAim.magnitude;
        if (distanceToAim > 0.15f && previousDistanceToAim > distanceToAim)
        {
            previousDistanceToAim = distanceToAim;
            scorchingRay.transform.position += toAimNormalized * speed * Time.deltaTime;
        }
        else
        {
            scorchingRay.transform.position = (Vector3)aim;
            animator.SetBool("isReachAim", true);
            ApplyScorchingRayEffects((Vector3)aim, effectRadius);
            aim = null;
        }
    }

    private void ApplyScorchingRayEffects(Vector3 aim, float effectRadius)
    {
        EnemyManagerPro.enemies.ForEach(enemy =>
        {
            Vector3 distanceToEnemy = enemy.transform.position - aim;
            if (distanceToEnemy.magnitude <= effectRadius)
            {
                enemy.effectsController.AddBurning(((LaserTower)tower).damageBurning);
                targets.Add(enemy);
            }
        });
        ApplyDamageToTargets(targets, damage);
    }
}