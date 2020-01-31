using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeAbility : TowerAbility
{
    [Header("LightningStrike")]
    public GameObject lightningStrikePrefab;
    public float abilityStunDuration = 3f;
    // public float speed = 20f;
    public float effectRadius = 2.19f;
    [HideInInspector]
    public GameObject lightningStrike;

    public List<AudioClip> abilitiesSounds;

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

        aim = aimPosition;
        tower.RotateCannon((Vector3)aim);

        tower.audioSource.pitch = 2f;
        tower.audioSource.PlayOneShot(abilitiesSounds[0], 0.6f);

        if (!lightningStrike)
        {
            lightningStrike = Instantiate(lightningStrikePrefab, gunpoint.position, Quaternion.identity);
            animator = lightningStrike.GetComponent<Animator>();
        }
        else
        {
            lightningStrike.transform.position = gunpoint.position;
            animator.SetBool("isReachAim", false);
        }
        toAimNormalized = ((Vector3)aim - lightningStrike.transform.position).normalized;

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
            tower.audioSource.PlayOneShot(abilitiesSounds[1], 0.8f);
        }
    }

    public override void ShootingControl()
    {

       /* Vector3 toAim = (Vector3)aim - lightningStrike.transform.position;
        float distanceToAim = toAim.magnitude;
        if (distanceToAim > 0.15f && previousDistanceToAim > distanceToAim)
        {
            previousDistanceToAim = distanceToAim;
            lightningStrike.transform.position += toAimNormalized * speed * Time.deltaTime;
        }
        else
        {*/
        lightningStrike.transform.position = (Vector3)aim;
        AudioSource blowUp = lightningStrike.GetComponent<AudioSource>();
        blowUp.pitch = 2f;
        blowUp.PlayOneShot(abilitiesSounds[2], 0.4f);
        animator.SetBool("isReachAim", true);
        ApplyLightningStrikeEffects((Vector3)aim, effectRadius);
        aim = null;
      //  }
    }

    private void ApplyLightningStrikeEffects(Vector3 aim, float effectRadius)
    {

        EnemyManagerPro.enemies.ForEach(enemy =>
        {
            if (enemy)
            {
                Vector3 distanceToEnemy = enemy.transform.position - aim;
                if (distanceToEnemy.magnitude <= effectRadius)
                {
                    enemy.effectsController.AddBurning(Effect.burningDuration, Effect.burningDamage);
                    enemy.effectsController.AddStun(abilityStunDuration);
                    targets.Add(enemy);
                }
            }
        });
        ApplyDamageToTargets(targets, damage);
    }
}
