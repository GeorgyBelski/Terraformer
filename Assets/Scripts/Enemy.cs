using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public enum EnemyType { Solder, Healer};

public class Enemy : Damageable
{
    
    public ThirdPersonCharacter character;
    public EnemyEffectsController effectsController;
    public EnemyType type;
    /*
   [Range(10, 1000)]
   public int maxHealth = 100;

   [Range(0, 1000)]
   public int health;
   int previousHealth = -1;

   [Space]
   [Header("References")]
   public Image healthBar;
   public Text[] damagePoints = new Text[3];
   public Animator[] damagePointAnimators = new Animator[3];
   public short damagePointIndex = 0;
   // Vector3 capsuleCenter;

   float healthRatio;
*/
    void Start()
    {
        // capsuleCenter = GetComponent<CapsuleCollider>().center;
        if (!EnemyManagerPro.enemies.Contains(this)) {
            //   EnemyManagerPro.enemies.Add(this);
            // EnemyManagerPro.enemies.Add(this);
            EnemyManagerPro.AddEnemy(this);
        }
    }

    void Update()
    {
        base.CalcHealthRatio();
    }

    public override void RemoveFromList()
    {
       EnemyManagerPro.enemies.Remove(this);
    }

/*
        public void ApplyDamage(int value, Vector3 shootPoint, Vector3 direction)
        {
            if (health > 0)
            {
                health -= value;
                PopUpDamagePoint(value);
            }
            if(health <= 0) {
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

            return this.transform.position + Vector3.up * 1.3f;
        }

        void PopUpDamagePoint(int value)
        {
            damagePoints[damagePointIndex].text = value.ToString();
            damagePointAnimators[damagePointIndex].SetBool("isDamaged", true);
            damagePointIndex++;
            if (damagePointIndex == damagePoints.Length) {
                damagePointIndex = 0;
            }
        }
    */
}
