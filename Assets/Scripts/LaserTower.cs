using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    public Transform gunpoint;
    public float duration = 0.3f;
    float timerDuration;
    LineRenderer lr;
    Color startLaser, endLaser;
    

    private void Start()
    {
        lr = gunpoint.GetComponent<LineRenderer>();
        startLaser = lr.startColor;
        endLaser = lr.endColor;
    }
    public override void TowerAttack(GameObject target)
    {
        if (target) {
            timerDuration = duration;
            lr.SetPosition(0, gunpoint.position);
            lr.SetPosition(1, target.transform.position);
            lr.startColor = startLaser;
            lr.endColor = endLaser;
        }
    }

    internal override void TowerUpdate()
    {
        if (timerDuration > 0)
        {
            float ratioDuration = timerDuration / duration;
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
