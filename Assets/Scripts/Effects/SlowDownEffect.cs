using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownEffect : Effect
{
    public float slowdownMultiplayer = 2f;
   public SlowDownEffect()
    {
        this.type = Effect.Type.Slowdown;
    }

    public void Set(float lifetime, float timerLifetime, float slowdownMultiplayer)
    {
        this.lifetime = lifetime;
        this.timerLifetime = timerLifetime;
        this.slowdownMultiplayer = slowdownMultiplayer;
    }
    public void Set(float lifetime, float slowdownMultiplayer)
    {
        Set(lifetime, lifetime, slowdownMultiplayer);
    }
}
