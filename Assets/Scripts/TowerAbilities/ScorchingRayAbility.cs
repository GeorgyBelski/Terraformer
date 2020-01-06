using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchingRayAbility : TowerAbility
{
    [Header("ScorchingRay")]
    public GameObject scorchingRayPrefab;

    public float speed = 20f;
    public float effectRadius = 2.19f;
    [HideInInspector]
    public GameObject scorchingRay;
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

        tower.audioSource.pitch = 2f;
        tower.audioSource.PlayOneShot(tower.abilitiesSounds[2], 0.6f);

        Vector3 offsetFromCannon = gunpoint.position - cannon.position;
        if (!scorchingRay)
        {
            scorchingRay = Instantiate(scorchingRayPrefab, gunpoint.position, Quaternion.identity);
            animator = scorchingRay.GetComponent<Animator>();
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
            tower.audioSource.Stop();
            tower.EndCasting();
            tower.audioSource.pitch = 1.5f;
            tower.audioSource.PlayOneShot(tower.abilitiesSounds[3], 0.8f);
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
            AudioSource blowUp = scorchingRay.GetComponent<AudioSource>();
            blowUp.pitch = 2f;
            blowUp.PlayOneShot(tower.abilitiesSounds[4], 0.4f);
            animator.SetBool("isReachAim", true);
            ApplyScorchingRayEffects((Vector3)aim, effectRadius);
            aim = null;
        }
    }

    private void ApplyScorchingRayEffects(Vector3 aim, float effectRadius)
    {

        EnemyManagerPro.enemies.ForEach(enemy =>
        {
            if (enemy)
                { Vector3 distanceToEnemy = enemy.transform.position - aim;
                if (distanceToEnemy.magnitude <= effectRadius)
                {
                    enemy.effectsController.AddBurning(BurningEffect.standardLifetime, ((LaserTower)tower).damageBurning);
                    targets.Add(enemy);
                }
            }    
        });
        ApplyDamageToTargets(targets, damage);
    }
}
