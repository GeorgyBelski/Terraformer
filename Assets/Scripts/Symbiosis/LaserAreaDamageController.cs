using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAreaDamageController : MonoBehaviour
{
    public int damageHit;
    public int damageBurning;
    public float BurningDuration;
    public Tower thisTower;

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == EnemyManagerPro.enemyLayer)
        {
            other.TryGetComponent<Enemy>(out Enemy enemy);
            if (enemy && enemy != thisTower.target)
            {
                enemy.ApplyDamage(damageHit, Vector3.zero, Vector3.zero);
                enemy.effectsController.AddBurning(BurningDuration, damageBurning);
            }

        }
    }
}
