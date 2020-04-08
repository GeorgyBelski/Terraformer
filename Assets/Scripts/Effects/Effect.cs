using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public static int burningDamage = 25;
    public static float burningDuration = 2f;
    public static float slowdownMultiplier = 0.5f;
    public static float slowdownDuration = 2f;
    public enum Type {Burning, Stan, Slowdown};
    public Type type;

    public float lifetime = 2f;
    public float timerLifetime;

    public static Effect Create(Type type) {
        if (type == Type.Burning)
        {
            return new BurningEffect();
        }
        else if(type == Type.Stan)
        {
            return new StunEffect();
        }
        else if(type == Type.Slowdown)
        {
            return new SlowDownEffect();
        }
        return null;
    }

    public void ReduceLifeTimer() {
        if (timerLifetime <= 0)
        {
            timerLifetime = 0;
        }
        else
        {
            timerLifetime -= Time.deltaTime;
         //   Debug.Log("timer: " + timer);
        }
    }

}
