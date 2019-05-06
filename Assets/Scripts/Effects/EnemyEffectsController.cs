using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyEffectsController : MonoBehaviour
{
    public Enemy thisEnemy;
    public ThirdPersonCharacter tpCharacter;
    public NavMeshAgent navAgent;
    public GameObject flamePrefab;
    public GameObject flame;
    public GameObject vertigoPrefab;
    public GameObject vertigo;

    ParticleSystem flameParticleSystem;

    public Dictionary<Effect.Type, Effect> effects = new Dictionary<Effect.Type, Effect>();
    List<Effect.Type> removeList = new List<Effect.Type>();


    void Start()
    {
        
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
            tpCharacter.m_Stan = false;
            navAgent.speed = 1;
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

    public void AddBurning(int damage) {
        BurningEffect burningEffect = (BurningEffect)AddEffect(Effect.Type.Burning);
        burningEffect.Set(2, 2, damage);
        EnableFlame();

    }

    public void AddStan(float duration) {
        StanEffect stanEffect = (StanEffect)AddEffect(Effect.Type.Stan);
        stanEffect.Set(2);
        EnableVertigo();
        //  tpCharacter.m_AnimSpeedMultiplier = 0f;
       // tpCharacter.m_MoveSpeedMultiplier = 0f;
        tpCharacter.m_Stan = true;
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
