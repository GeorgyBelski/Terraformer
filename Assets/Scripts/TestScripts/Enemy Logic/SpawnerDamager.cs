using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerDamager : Enemy_Logic
{
    public int damageOnAttack;
    public GameObject powerCristall;
    public Image chargeBar;

    public float jumpDistance;
    public float chargingTime;

    private float realChargingTime;
    private bool isChargingReady = false;
    private Material cristalMaterial;

    public override void Attack()
    {
        targetTower.towerHealth.ApplyDamage(damageOnAttack, Vector3.zero, Vector3.zero);
    }

    // Start is called before the first frame update
    void Start()
    {
        cristalMaterial = powerCristall.GetComponent<Renderer>().material;
        //readyToJump();
        realChargingTime = 0;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        realChargingTime += Time.deltaTime;
        if (!isChargingReady)
        {
            if (realChargingTime < chargingTime)
            {
                charging();             
            }
            else
            {
                readyToJump();
                print("=");
            } 
        }

        base.Update();
    }

    void charging()
    {
        print("+");
        chargeBar.fillAmount = realChargingTime / chargingTime;
        
        //cristalMaterial.color = new Color(255 - (100 / chargingTime * realChargingTime), 0 + (100 / chargingTime * realChargingTime), 0);
    }

    void readyToJump()
    {
        isChargingReady = true;
    }

    void jump()
    {
        isChargingReady = false;
        realChargingTime = 0;
        charging();
    }

    public override void check()
    {
        float heals = enem.GetHealthRatio();
        if ((heals - brawe) <= 0 && heals != 0) //Логика состояний
        {
            if (EnemyManagerPro.enemiesMap.ContainsKey(EnemyType.Healer) && EnemyManagerPro.enemiesMap[EnemyType.Healer].Count > 0)
            {
                //Debug.Log("checking");
                isStand = false;
                isGoingToDest = false;
                IsAttack = false;
                //state_Attack();
                stateGiveUp();
                isGiveUp = true;
            }
            else
            {
                isStand = false;
                isGiveUp = false;
                stateGoToDestanation();
            }

        }
        else
        {
            if (isGiveUp)
            {
                if (heals > 0.5 + brawe / 2 || EnemyManagerPro.enemiesMap[EnemyType.Healer].Count == 0)
                {
                    isStand = false;
                    isGiveUp = false;
                    stateGoToDestanation();
                    //
                }
                else
                {
                    stateGiveUp();
                }
            }
            else
            {
                if (IsAttack)
                {
                    if (destTower == null)
                    {
                        isStand = false;
                        IsAttack = false;
                        isGiveUp = false;
                        stateGoToDestanation();
                    }
                    else
                    {
                        IsAttack = true;
                    }
                }
                else
                {
                    isStand = false;
                    IsAttack = false;
                    isGiveUp = false;
                    stateGoToDestanation();
                    //isGoingToDist = true;
                }
            }
        }
    }
}
