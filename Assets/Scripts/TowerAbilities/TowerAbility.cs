using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAbility : MonoBehaviour
{

    [Header("References")]
    public Tower tower;
    protected Transform cannon, gunpoint;

    [Header("Ability Options")]
    public float castTime = 0.5f;
    protected float timerCast;
    public int damage = 100;

    protected void Start()
    {
        cannon = tower.cannon;
        gunpoint = tower.gunpoint;
    }


    public void Cast(Vector3 aimPosition)
    {
        if (tower.IsCastingAbility == true)
        {
            return;
        }

        tower.IsCastingAbility = true;
        timerCast = castTime;
    }

    protected void Control()
    {

        if (tower.IsCastingAbility)
        {
            CastingControl();
        }
        else
        {
            ShootingControl();
        }
    }


    public abstract void CastingControl();
    public abstract void ShootingControl();

    protected void ApplyDamageToTargets(List<Enemy> enemiesList, int damage)
    {
        foreach (Enemy enemy in enemiesList)
        {
            enemy.ApplyDamage(damage, Vector3.zero, Vector3.zero);
        }
        enemiesList.Clear();
    }
}
