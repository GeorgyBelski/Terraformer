using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroAreaDamageController : MonoBehaviour
{
    public int damage;
    public float stunDuration = 0;
    public Tower thisTower;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
        stunDuration = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == EnemyManagerPro.enemyLayer) 
        {
            Enemy enemy;
            other.TryGetComponent<Enemy>(out enemy);
            if (enemy && enemy != thisTower.target) 
            { 
                enemy.ApplyDamage(damage, Vector3.zero, Vector3.zero) ;
                if (stunDuration > 0)
                { enemy.effectsController.AddStun(stunDuration); }
            }

        }
    }
}
