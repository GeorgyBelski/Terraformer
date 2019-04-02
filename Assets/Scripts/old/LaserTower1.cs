using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower1 : MonoBehaviour
{
    [Header("Main Attributes")]
    public int range = 8;


    [Header("Cooldowns")]
    public float cooldownAttack = 1f;
    float timerAttack = 0f;
    public float cooldownAbility1 = 10f;
    float timerAbility1 = 0f;
    public float cooldownAbility2 = 15f;
    float timerAbility2 = 0f;

    [Space]
    [Header("References")]
    public Transform cannon;
    public GameObject target;

    void Start()
    {

    }


    void Update()
    {
        target = ChooseTarget();
        LookAtTarger();
    }

    void ReduceTimers()
    {
        if (timerAttack > 0)
        {
            timerAttack -= Time.deltaTime;
        }
        if (timerAbility1 > 0)
        {
            timerAbility1 -= Time.deltaTime;
        }
        if (timerAbility2 > 0)
        {
            timerAbility2 -= Time.deltaTime;
        }
    }
    GameObject ChooseTarget()
    {
        float distanceToTarget = range;
        float distanceTmp = distanceToTarget;
        int targetIndex = -1;
        for (int i = 0; i < EnemyManager.enemies.Count; i++)
        {
            distanceTmp = (EnemyManager.enemies[i].transform.position - this.transform.position).magnitude;
            if (distanceTmp < distanceToTarget)
            {
                targetIndex = i;
                distanceToTarget = distanceTmp;
            }
        }
        if (targetIndex == -1)
        {
            return null;
        }
        else
        {
            return EnemyManager.enemies[targetIndex];
        }

    }
    void LookAtTarger()
    {

        if (target)
        {
            cannon.LookAt(target.transform);
        }
        
    }
    void Shooting()
    {
        if (timerAttack <= 0)
        {

        }
    }

}
