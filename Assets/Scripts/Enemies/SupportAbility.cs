using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SupportAbilityState {Ready, Aiming, Recharge };
public abstract class SupportAbility : MonoBehaviour
{
    public SupportController thisController;
    public SupportAbilityState state = SupportAbilityState.Ready;
    public bool isAreaAbility;
    public float range = 5f;
    public float cooldawn = 5f;
    public float timerCooldown;



    protected void Start()
    {
       // totemPoint = transform.position;
    }

    protected void Update()
    {
        ReduceTimer();
        Aiming();
    }

    void ReduceTimer() 
    {
        timerCooldown -= Time.deltaTime;
        if (timerCooldown <= 0)
        { state = SupportAbilityState.Ready; }

    }

    void Aiming() 
    {
        if(state != SupportAbilityState.Ready)
        { return; }
        state = SupportAbilityState.Aiming;

        AbilityAiming();
    }
    protected abstract void AbilityAiming();

    protected void Cast()
    {
        if (thisController.emk.character.m_Stun || thisController.state == SupportState.Cast) 
        { return; }

        thisController.state = SupportState.Cast;
        thisController.activeAbility = this;
        AbilityCast();

        timerCooldown = cooldawn;
        state = SupportAbilityState.Recharge;
    }
    protected abstract void AbilityCast();
    public abstract void EndCast();
}
