using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanEffect : Effect
{
    public StanEffect() {
        this.type = Effect.Type.Stan;
    }

    public void Set(float lifetime, float timerLifetime)
    {
        this.lifetime = lifetime;
        this.timerLifetime = timerLifetime;
    }
    public void Set(float lifetime)
    {
        Set(lifetime, lifetime);
    }
}
