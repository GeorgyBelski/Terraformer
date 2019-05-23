using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class HealController : MonoBehaviour
{
    [Header("Heling")]
    
    public Transform healpoint;
    public float healingRate = 2f;
    public float healingDist = 3f;
    public float healingPower = 100f;
    Enemy target;
    private float realHealingTime;

    [Header("Heal Points")]
    public GameObject healBase;
    public float castHealBaseRate = 15f;
    private float realCastHealBaseRate;

    LineRenderer lr;
    Color startHeal, endHeal;
    private EnemyMouseController emk;
    public float duration = 0.5f;
    private float realDuration;

    public float movementRate = 1f;
    private float realMovementRate;

    bool line = false;
    private float startwidth;
    private ThirdPersonCharacter tpch;

    void Start()
    {
        realMovementRate = movementRate;
        tpch = GetComponent<ThirdPersonCharacter>();
        realCastHealBaseRate = castHealBaseRate;
        realDuration = duration;
        lr = healpoint.GetComponent<LineRenderer>();
        startHeal = lr.startColor;
        endHeal = lr.endColor;
        realHealingTime = healingRate;
        startwidth = lr.widthMultiplier;
        //brawe = Random.Range(0.1f, 0.7f);
        //realcheckTime = checkTime;
        emk = GetComponent<EnemyMouseController>();
        //enem = GetComponent<Enemy>();
        //float min;
        lr.startColor = startHeal;
        EnemyManagerPro.AddEnemy(GetComponent<Enemy>());
        lr.endColor = endHeal;

    }

    void Update()
    {
        realCastHealBaseRate -= Time.deltaTime;
        if (line && target)
        {
            target.ApplyDamage(-(int)healingPower / 20, target.GetPosition(), Vector3.zero); // Testing!!!
            lr.widthMultiplier = startwidth;
            realDuration -= Time.deltaTime;
            lr.SetPosition(0, healpoint.position);
            lr.SetPosition(1, target.transform.position + new Vector3(0, 1, 0));
            lr.startColor = startHeal;
            lr.endColor = endHeal;
            if (realDuration <= 0)
            {
                line = false;
                lr.widthMultiplier = 0;
                realDuration = duration;
            }
        }

        if (realCastHealBaseRate <= 0)
        {
            Instantiate(healBase, transform.position, transform.rotation, null);
            //tpch.ScaleCapsuleForCrouching(true);
            realCastHealBaseRate = castHealBaseRate;
        }

        realHealingTime -= Time.deltaTime;
        state_heal();
        if(emk.agent.remainingDistance <= emk.agent.stoppingDistance)
        {
            realMovementRate -= Time.deltaTime;
        }
        if (realMovementRate <= 0)
        {
            realMovementRate = movementRate;
            state_Walking();
        }
    }

    private void state_Walking()
    {
       
        Vector3 target = new Vector3(this.transform.position.x + Random.Range(-3f, 3f), this.transform.position.y, this.transform.position.z + Random.Range(-3f, 3f));
        //Transform tr; // Fix!
        //tr.position = target;
        //emk.setDest(tr);
        emk.agent.SetDestination(target);
        
    }

    private void state_heal()
    {
        
        if (EnemyManagerPro.enemiesMap[EnemyType.Solder].Count > 0 && realHealingTime <= 0)
        {
            float min = -1;
            target = null;
            //Debug.Log("checking Heal");
            //min = Vector3.Distance(EnemyManagerPro.enemiesMap["Damager"][0].gameObject.transform.position, transform.position);
            //Transform dest = EnemyManagerPro.enemiesMap["Damager"][0].gameObject.transform;
            for (int i = 0; i < EnemyManagerPro.enemiesMap[EnemyType.Solder].Count; i++)
            {
                if(EnemyManagerPro.enemiesMap[EnemyType.Solder][i].GetHealthRatio() < 1)
                {
                    if (min == -1)
                    {
                        target = EnemyManagerPro.enemiesMap[EnemyType.Solder][i];
                        min = Vector3.Distance(target.gameObject.transform.position, transform.position);
                    }
                    else
                    {
                        if(min > Vector3.Distance(EnemyManagerPro.enemiesMap[EnemyType.Solder][i].gameObject.transform.position, transform.position))
                        {
                            target = EnemyManagerPro.enemiesMap[EnemyType.Solder][i];
                            min = Vector3.Distance(target.gameObject.transform.position, transform.position);

                        }
                    }
                    //min = Vector3.Distance(EnemyManagerPro.enemiesMap["Damager"][0].gameObject.transform.position, transform.position);
                    
                    //EnemyManagerPro.enemiesMap["Healer"][i].ApplyDamage(100, EnemyManagerPro.enemiesMap["Healer"][i].GetPosition(), Vector3.zero);
                }
                //Transform current_dist = EnemyManagerPro.enemiesMap["Healer"][i].gameObject.transform;
                /*
                if (min > Vector3.Distance(current_dist.position, transform.position))
                {
                    min = Vector3.Distance(current_dist.position, transform.position);
                    destHel = current_dist;
                }
                */
            }
            if(target && min <= healingDist)
            {
                line = true;
                //emk.setDest(target.transform);
                lr.SetPosition(0, healpoint.position);
                lr.SetPosition(1, target.transform.position + new Vector3(0, 1, 0));
                lr.startColor = startHeal;
                lr.endColor = endHeal;
                //target.ApplyDamage(-(int)healingPower, target.GetPosition(), Vector3.zero); // Change ApplyDamage
                realHealingTime = healingRate;
            }
        }
    }

    private void state_setHealPoint()
    {

    }
}
