using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDamager : Enemy_Logic
{
    public TypeOfEnemy type;
    public int damageOnAttack;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        //print(1);
        base.Update();
    }

    public override void Attack()
    {
        if (targetTower)
        { targetTower.towerHealth.ApplyDamage(damageOnAttack, Vector3.zero, Vector3.zero); }
    }

}
