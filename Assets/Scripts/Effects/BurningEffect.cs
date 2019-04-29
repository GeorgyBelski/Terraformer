using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : Effect
{
    public GameObject flame;
    public int damage; // in half a second
    public float timerDamage = 0.3f;

    public BurningEffect(float lifetime, float timerLifetime, int damage)
    {
        this.type = Type.Burning;
        Set(lifetime, timerLifetime, damage);
    }


    public void Set(float lifetime, float timerLifetime, int damage) {
        this.lifetime = lifetime;
        this.timerLifetime = timerLifetime;
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
