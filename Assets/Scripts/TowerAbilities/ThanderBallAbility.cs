﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThanderBallAbility : TowerAbility
{
    [Header("ThandetBall")]
    public GameObject thanderBallPrefab;

    public float speed = 20f;
    public float effectRadius = 2.19f;
    public float abilityStunDuration = 3f;
    [HideInInspector]
    public GameObject thanderBall;
    Material thandetBallTrailMaterial;
    Animator thandetBallAnimator;
    SphereCollider thandetBallCollider;
    Vector3? aim;
    Vector3 toAimNormalized;
    //Collider[] hitThanderBallColliders;
    List<Enemy> thanderBallTargets;

    float previousDistanceToAim;

    new void Start()
    {
        base.Start();
        aim = null;
        //   hitThanderBallColliders = new Collider[10];
        thanderBallTargets = new List<Enemy>();

        
    }

    void Update()
    {
        ThanderBallControl();
    }


    public new void Cast(Vector3 aimPosition)
    {
        base.Cast(aimPosition);

    //    TowerManager.availableElectroTowers.Remove((ElectroTower)tower);
        aim = aimPosition;
        tower.RotateCannon((Vector3)aim);

        tower.audioSource.pitch = 4f;
        tower.audioSource.PlayOneShot(tower.abilitiesSounds[0], 0.7f);

        Vector3 offsetFromCannon = gunpoint.position - cannon.position;
        if (!thanderBall)
        {
            thanderBall = Instantiate(thanderBallPrefab, gunpoint.position + offsetFromCannon / 2, cannon.rotation);
            thandetBallAnimator = thanderBall.GetComponentInChildren<Animator>();
        //    thandetBallCollider = thandetBall.GetComponentInChildren<SphereCollider>();
        //    thandetBallCollider.enabled = false;

            thandetBallTrailMaterial = thanderBall.GetComponentInChildren<ParticleSystemRenderer>().trailMaterial;
            thandetBallTrailMaterial.SetColor("_BaseColor", new Color(5, 5, 5, 1));
        }
        else
        {
            thanderBall.transform.position = gunpoint.position + offsetFromCannon / 2;
            thanderBall.transform.rotation = cannon.rotation;
            thandetBallAnimator.SetBool("isReachAim", false);
        }
        toAimNormalized = ((Vector3)aim - thanderBall.transform.position).normalized;

    }

    void ThanderBallControl()
    {
        if (aim != null)
        {
            base.Control();//<- CastingControl(), <- ShootingControl()
        }
    }

    public override void CastingControl()
    {
        Vector3 ofsetFromCannon = gunpoint.position - cannon.position;
        thanderBall.transform.position += ofsetFromCannon * speed * Time.deltaTime / 32;
        previousDistanceToAim = float.PositiveInfinity;
        timerCast -= Time.deltaTime;
        if (timerCast <= 0)
        {
            tower.EndCasting();
        }
    }

    public override void ShootingControl()
    {
        Vector3 toAim = (Vector3)aim - thanderBall.transform.position;
        float distanceToAim = toAim.magnitude;
        if (distanceToAim > 0.15f && previousDistanceToAim > distanceToAim)
        {
            previousDistanceToAim = distanceToAim;
            thanderBall.transform.position += toAimNormalized * speed * Time.deltaTime;
        }
        else
        {
            thanderBall.transform.position = (Vector3)aim;
            thanderBall.transform.rotation = Quaternion.identity;
            AudioSource blowUp = thanderBall.GetComponent<AudioSource>();
            blowUp.pitch = 2f;
            blowUp.PlayOneShot(tower.abilitiesSounds[1], 0.5f);
            thandetBallAnimator.SetBool("isReachAim", true);
            ApplyThanderBallEffects((Vector3)aim, effectRadius);
            aim = null;
        }
    }

    private void ApplyThanderBallEffects(Vector3 center, float radius)
    {    /*  
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
        
*/
        EnemyManagerPro.enemies.ForEach(enemy =>
        {
            if (enemy)
            {
                Vector3 distanceToEnemy = enemy.transform.position - center;
                if (distanceToEnemy.magnitude <= radius)
                {
                    enemy.effectsController.AddStun(abilityStunDuration);
                    thanderBallTargets.Add(enemy);
                }
            }
        });

        ApplyDamageToTargets(thanderBallTargets, damage);
    }

    

}
