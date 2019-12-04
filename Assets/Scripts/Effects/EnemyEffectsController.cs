using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyEffectsController : MonoBehaviour
{
    public bool enableBurning = true;
    public bool enableStun = true;
    public bool enableSlowdown = true;

    public Enemy thisEnemy;
    public ThirdPersonCharacter tpCharacter;
    public NavMeshAgent navAgent;
    public GameObject flamePrefab;
    public GameObject flame;
    public GameObject vertigoPrefab;
    public GameObject vertigo;
    public float originalNavAgentSpeed;
    //private float basicSpeed;

    ParticleSystem flameParticleSystem;

    public Dictionary<Effect.Type, Effect> effects = new Dictionary<Effect.Type, Effect>();
    List<Effect.Type> removeList = new List<Effect.Type>();


    void Start()
    {
        if (enableStun && navAgent)
        { originalNavAgentSpeed = navAgent.speed; }
    }

    void Update()
    {
        foreach (Effect effect in effects.Values)
        {
            if (effect != null)
            { 
                effect.ReduceLifeTimer();
                if (effect.timerLifetime <= 0)
                {
                    StopEffect(effect);
                    removeList.Add(effect.type);
                }
                else
                {
                    ContinueEffect(effect);
                }
            }
        }
        foreach (Effect.Type type in removeList) {
            effects[type] = null;
        }
        removeList.Clear();
    }

    private void ContinueEffect(Effect effect)
    {
        if (effect.type == Effect.Type.Burning)
        {
            BurningEffect burningEffect = (BurningEffect)effect;
            burningEffect.ReduceDamageTimer();
            if (burningEffect.timerDamage == 0)
            {
                burningEffect.timerDamage = 0.5f;
                thisEnemy.ApplyDamage(burningEffect.damage, Vector3.zero, Vector3.zero);
            }
        }
        else if (effect.type == Effect.Type.Stan)
        {
            navAgent.speed = 0;
        }
        else if (effect.type == Effect.Type.Slowdown)
        {

        }
    }

    private void StopEffect(Effect effect)
    {
        if (effect.type == Effect.Type.Burning)
        {
            var emission = flameParticleSystem.emission;
            emission.enabled = false;
        }
        else if (effect.type == Effect.Type.Stan)
        {
            vertigo.SetActive(false);
          //  tpCharacter.m_AnimSpeedMultiplier = 1f;
          //  tpCharacter.m_StationaryTurnSpeed = 180f;
            tpCharacter.m_Stun = false;
            navAgent.speed = originalNavAgentSpeed;
        }
        else if (effect.type == Effect.Type.Slowdown)
        {
            enableSlowdown = true;
            navAgent.speed = originalNavAgentSpeed;
        }
    }
    private Effect AddEffect(Effect.Type type) {
        if (effects.ContainsKey(type))
        {
            if (effects[type] != null)
            {
                Effect effect = effects[type];
                //   burningEffect.Set(burningEffect.lifetime, burningEffect.lifetime, damage);
                return effect;
            }
            else
            {
                Effect effect = Effect.Create(type);
                effects[type] = effect;
                return effect;
            }

        }
        else
        {
            Effect effect = Effect.Create(type);
            effects.Add(type, effect);
            return effect;
        }
    }

    public void AddSlowdown(float duration, float multiplayer)
    {
        
        if (!enableSlowdown)
            return;
        SlowDownEffect slowdown = (SlowDownEffect)AddEffect(Effect.Type.Slowdown);
        slowdown.Set(duration, multiplayer);
        // print(navAgent.speed);
        navAgent.speed = Mathf.Min(originalNavAgentSpeed * multiplayer, navAgent.speed);
        // print(navAgent.speed);
        enableSlowdown = false;
    }

    public void AddBurning(float time, int damage) {
        if (!enableBurning) {
            return;
        }
        BurningEffect burningEffect = (BurningEffect)AddEffect(Effect.Type.Burning);
        burningEffect.Set(time, damage);
        EnableFlame();

    }

    public void AddStun(float duration) {
        if (!enableStun)
        {
            return;
        }
        StunEffect stunEffect = (StunEffect)AddEffect(Effect.Type.Stan);
        stunEffect.Set(2);
        EnableVertigo();
        //  tpCharacter.m_AnimSpeedMultiplier = 0f;
       // tpCharacter.m_MoveSpeedMultiplier = 0f;
        tpCharacter.m_Stun = true;
        navAgent.speed = 0;
    }

    

    void EnableFlame() {
        if (!flame)
        {
            flame = Instantiate(flamePrefab, thisEnemy.GetPosition(), thisEnemy.transform.rotation);
            flame.transform.parent = thisEnemy.transform;
            flameParticleSystem = flame.GetComponent<ParticleSystem>();
        }
        else
        {
            var emission = flameParticleSystem.emission;
            emission.enabled = true;
        }
    }

    private void EnableVertigo()
    {
        if (!vertigo)
        {
            vertigo = Instantiate(vertigoPrefab, thisEnemy.GetPosition() + Vector3.up * 0.6f, vertigoPrefab.transform.rotation);
            vertigo.transform.parent = thisEnemy.transform;
        }
        else {
            vertigo.SetActive(true);
        }
    }

}
