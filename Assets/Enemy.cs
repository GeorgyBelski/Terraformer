using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Range(10, 1000)]
    public int maxHealth = 100;

    [Range(0, 1000)]
    public int health;
    int previousHealth = -1;

    [Space]
    [Header("References")]
    public Image healthBar;

    float healthRatio;

    void Start()
    {
        
    }

    void Update()
    {
        CalcHealthRatio();
    }

    public void ApplyDamage(int value, Vector3 shootPoint, Vector3 direction)
    {
        if (health > 0)
        {
            health -= value;
        }
        else {
            health = 0;
            EnemyManager.enemies.Remove(this);
        }
    }
    public float CalcHealthRatio() {
        if (health != previousHealth) {
            previousHealth = health;
            healthRatio = (float)health / maxHealth;
            healthBar.fillAmount = healthRatio;
        }
        return healthRatio;
    }
    public float GetHealthRatio()
    {
        return healthRatio;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
}
