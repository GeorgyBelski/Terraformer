using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Damageable : MonoBehaviour
{

    [Range(10, 4000)]
    public int maxHealth = 100;

    [Range(0, 4000)]
    public int health;
    int previousHealth = -1;

    [Space]
    [Header("References")]
    public Image healthBar;
    public Image healBar;
    public bool isHeal;
    protected float maxRepairHealthRatio = 0;
    public Text[] damagePoints = new Text[3];
    public Animator[] damagePointAnimators = new Animator[3];
    public short damagePointIndex = 0;
    // Vector3 capsuleCenter;

    public float healthRatio = 1f;

    bool isDead = false;

    void Start()
    {
        
    }

    void Update()
    {
        CalcHealthRatio();
    }

    public virtual void ApplyDamage(int value, Vector3 shootPoint, Vector3 direction)
    {
        if (health > 0)
        {
            health -= value;
            if (isHeal)
            {
                //print((float)value / maxHealth);
                maxRepairHealthRatio -= (float)value / maxHealth;
                healBar.fillAmount = maxRepairHealthRatio;
            }
              
            PopUpDamagePoint(value);
        }
        if (health <= 0 && !isDead)
        {
            health = 0;
            ApplyDeath();
            isDead = true;
        }
    }

    public void ApplyHeal(int value) 
    {
        if (health > 0) 
        {
            health += value;
        }

        PopUpHealPoint(value);

        if (health > maxHealth) 
        {
            health = maxHealth;
        }
    }
    public abstract void RemoveFromList();
    public abstract void ApplyDeath();

    public float CalcHealthRatio()
    {
        if (health != previousHealth)
        {
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

        return this.transform.position + Vector3.up * 1.3f;
    }

    void PopUpDamagePoint(int value)
    {
        PopUpPoint(value, false);
    }
    void PopUpHealPoint(int value)
    {
        PopUpPoint(value, true);
    }
    void PopUpPoint(int value, bool isHeal) 
    {
        if (!damagePoints[damagePointIndex]) 
        { return; }

        damagePoints[damagePointIndex].text = value.ToString();

        if (isHeal)
        { damagePointAnimators[damagePointIndex].SetBool("isHealed", true); }
        else
        { damagePointAnimators[damagePointIndex].SetBool("isDamaged", true); }

        damagePointIndex++;
        if (damagePointIndex == damagePoints.Length)
        {
            damagePointIndex = 0;
        }
    }
}
