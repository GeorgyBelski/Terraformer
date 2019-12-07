using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAbility_HealingTotem : SupportAbility
{
    
    public Transform castPoint;
    public GameObject healBase;
    protected Vector3 totemPoint;

    void Start()
    {
        base.Start();
        isAreaAbility = true;
    }

    void Update()
    {
        base.Update();

    }
    protected override void AbilityAiming() 
    {
        Vector3 toSupportDestination = thisController.supportDestination - transform.position;
        if (range >= toSupportDestination.magnitude)
        {
            totemPoint = thisController.supportDestination;
        }
        else
        {
            totemPoint = transform.position + toSupportDestination.normalized * range;
        }
        Cast();
    }
    protected override void AbilityCast()
    {
        thisController.lookAt = totemPoint;
        thisController.animator.SetBool("CastToAir", true);
        // ApplyTotem(); // Call from animation

    }
    override public void EndCast() //Call from animation
    {
        thisController.animator.SetBool("CastToAir", false);
    }
    public void ApplyTotem() // Call from animation
    {
        Instantiate(healBase, totemPoint, transform.rotation);

    }
}
