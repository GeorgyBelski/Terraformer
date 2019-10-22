using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : Effect
{
    public int damage; // in half a second
    public float timerDamage = 0.3f;
    public static float standardLifetime = 2f;

    public BurningEffect() {
        this.type = Effect.Type.Burning;
    }

    public void Set(float lifetime, int damage) {
        
        this.lifetime = lifetime;
        this.timerLifetime = lifetime;
        this.damage = damage;
    }


    public void ReduceDamageTimer()
    {
        if (timerDamage <= 0)
        {
            timerDamage = 0;
        }
        else
        {
            timerDamage -= Time.deltaTime;
        }
    }

    
}
