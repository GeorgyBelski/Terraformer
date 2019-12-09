using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAbility_Heal : SupportAbility
{
    
    public Transform castPoint;
    public LineRenderer lr;
    float duration = 0.5f;
    float timerDuration;
    protected Enemy targetUnit = null;

    void Start()
    {
        base.Start();
        isAreaAbility = false;
        lr.enabled = false;
    }

    void Update()
    {
        base.Update();
        ShowHeal();
    }
    protected override void AbilityAiming() 
    {
        targetUnit = null;
        float minHealthRatio = 1;
        thisController.allies.ForEach(ally =>
        {
            if (ally) { 
                Vector3 toAlly = ally.transform.position - transform.position;
                if (toAlly.magnitude <= range)
                {
                    if (minHealthRatio > ally.GetHealthRatio())
                    {
                        minHealthRatio = ally.GetHealthRatio();
                        targetUnit = ally;
                    }
                }
            }
        });

        if (targetUnit)
        {
            Cast(); // -> AbilityCast()
        }
    }
    protected override void AbilityCast()
    {
        if (targetUnit)
        {
            thisController.lookAt = targetUnit.transform.position;
            thisController.animator.SetBool("CastingArm", true);
            // ApplyAbility(); // Call from animation
        }
    }
    override public void EndCast() //Call from animation
    {
        thisController.animator.SetBool("CastingArm", false);
    }
    void ShowHeal() 
    {
        if (timerDuration > 0) 
        { 
            timerDuration -= Time.deltaTime;
            lr.SetPosition(0, castPoint.position);
            if (targetUnit)
            { lr.SetPosition(1, targetUnit.GetPosition()); }
        }
        else 
        {
            lr.enabled = false;
        }

    }
    public void ApplyHealRay() // Call from animation
    {
        targetUnit.ApplyHeal(150);
        Debug.DrawLine(transform.position + Vector3.up * 1.8f, targetUnit.GetPosition(), Color.white, 0.5f);
        lr.enabled = true;
        timerDuration = duration;

    }

    
}
