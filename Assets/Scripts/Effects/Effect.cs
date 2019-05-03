using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
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
            return new StanEffect();
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
