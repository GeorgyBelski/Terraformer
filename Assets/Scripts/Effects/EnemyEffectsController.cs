using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectsController : MonoBehaviour
{
    public Enemy thisEnemy;
    public GameObject flamePrefab;
    public GameObject flame;
    ParticleSystem flameParticleSystem;
    public Dictionary<Effect.Type, Effect> effects = new Dictionary<Effect.Type, Effect>();
    List<Effect.Type> removeList = new List<Effect.Type>();


    void Start()
    {
        
    }

    void Update()
    {
        try
        {
            foreach (Effect effect in effects.Values)
            {
                if (effect != null)
                { 
                    effect.ReduceLifeTimer();
                    if (effect.timerLifetime <= 0)
                    {
                        if (effect.type == Effect.Type.Burning)
                        {
                            var emission = flameParticleSystem.emission;
                            emission.enabled = false;
                        }

                        removeList.Add(effect.type);
                    }
                    else
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

                    }
                }
            }
            foreach (Effect.Type type in removeList) {
                effects[type] = null;
            }
            removeList.Clear();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    

    public void AddBurning(int damage) {
        if (effects.ContainsKey(Effect.Type.Burning))
        {
            if (effects[Effect.Type.Burning] != null)
            {
                BurningEffect burningEffect = (BurningEffect)effects[Effect.Type.Burning];
                burningEffect.Set(burningEffect.lifetime, burningEffect.lifetime, damage);
            }
            else
            {
                effects[Effect.Type.Burning] = new BurningEffect(2, 2, damage);
            }
            
        }
        else
        {
            effects.Add(Effect.Type.Burning, new BurningEffect(2,2, damage));  
        }
        EnableFlame();

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
}
