using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDamager : Enemy_Logic
{
    public TypeOfEnemy type;
    public int damageOnAttack;
    public EnemyEffectsController effectController;
    public float timerBreakingCreep;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        //print(1);
        base.Update();
      //  ReduceTimerBreakingCreep();
    }

    public override void Attack()
    {
        if (targetTower)
        {
            audioSource.pitch = Random.Range(0.7f, 0.9f);
            base.Attack();
            targetTower.towerHealth.ApplyDamage(damageOnAttack, Vector3.zero, Vector3.zero);
        }
    }

    public override void check() 
    {
        if (!isCasting)
        { base.check(); }
    }

}
