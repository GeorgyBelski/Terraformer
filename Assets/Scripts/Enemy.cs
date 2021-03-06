﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public enum EnemyType { Solder, Healer, Tank, Jumper, Totem};

public class Enemy : Damageable
{
    public Material shaderMaterial;
    public ThirdPersonCharacter character;
    public EnemyEffectsController effectsController;
    public EnemyType type;
    public Animator animator;
    public SpurtFXController spurtFXController;
    public Transform chest;

    [Header("Sounds")]
    public AudioSource audioSource;
    public List<AudioClip> enemySounds;

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
        healthBar.material = Instantiate(shaderMaterial);
        shaderMaterial = healthBar.material;
        //shaderMaterial = healthBar.;
        shaderMaterial.SetFloat("_Steps", maxHealth/200);
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
        EnemyManagerPro.RemoveEnemy(this);
    }

    public override void ApplyDeath()
    {
        this.RemoveFromList();
        if (animator)
        {
            audioSource.pitch = Random.Range(0.9f, 1.2f);
            audioSource.PlayOneShot(enemySounds[0], 0.2f);
            animator.SetBool("DeathTrigger", true);
          //  effectsController.navAgent.speed = 0;
            effectsController.navAgent.enabled = false;
         //   effectsController.navAgent.angularSpeed = 0;
        }
        else {
            DestroyUnit();
        }
            
    }
    public void DestroyUnit() //C
    {
        //this.RemoveFromList();
        Destroy(this.gameObject);
        if (spurtFXController)
        { spurtFXController.Destroy(); }
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
    new public Vector3 GetPosition() 
    {
        return chest.position;
    }
}
