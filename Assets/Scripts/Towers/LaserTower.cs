using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [Space]
    [Header("LaserTower")]
    public Transform gunpoint;
    public float beamDuration = 0.3f;
    public int damageAttack = 50;
    public int damageBurning = 5;
    float timerDuration;
    LineRenderer lr;
    Color startLaser, endLaser;
    

    private void Start()
    {
        base.Start();
        type = TowerType.Laser;

        lr = gunpoint.GetComponent<LineRenderer>();
        startLaser = lr.startColor;
        endLaser = lr.endColor;
    }
    public override void TowerAttack(Enemy target)
    {
        if (target) {
            timerDuration = beamDuration;
            lr.SetPosition(0, gunpoint.position);
            lr.SetPosition(1, target.GetPosition());
            lr.startColor = startLaser;
            lr.endColor = endLaser;

            target.ApplyDamage(damageAttack, target.GetPosition(), Vector3.zero);
            target.effectsController.AddBurning(damageBurning);
        }
    }

    internal override void TowerUpdate()
    {
        if (timerDuration > 0)
        {
            float ratioDuration = timerDuration / beamDuration;
            timerDuration -= Time.deltaTime;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g - 1 + ratioDuration, lr.startColor.b, ratioDuration);
            lr.endColor = new Color(lr.startColor.r, lr.startColor.g - 1 + ratioDuration, lr.startColor.b, ratioDuration);
            lr.widthMultiplier = ratioDuration;
        }
        else
        {
            lr.startColor = new Color(0, 0, 0, 0);
            lr.endColor = new Color(0, 0, 0, 0);
        }
    }
}
