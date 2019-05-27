
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Logic : MonoBehaviour
{
    public Animator animator;
    private EnemyMouseController emk;
    private Enemy enem;
    private Vector3? destTower = null;
    private Vector3? destHel = null;
    private Tower targetTower;
    private bool isGoingToDist = false;
    private bool isStand = true;
    private bool isGiveUp = false;
    private bool isAttack = false;

    private bool isRush = false;

    
    private float checkTime = 0.5f;//change!!!!!!!!!!!!!!!!!!!!!
    private float realcheckTime;//change!!!!!!!!!!!!!!!!!!!!!
    private float brawe = 0; //= Random.RandomRange(0.1f, 0.7f);
    private float min;

    public bool IsAttack { get => isAttack; set { isAttack = value; animator.SetBool("Attack", value); } }


    // Start is called before the first frame update
    void Start()
    {
        brawe = Random.Range(0.1f, 0.7f);
        realcheckTime = checkTime;
        emk = GetComponent<EnemyMouseController>();
        enem = GetComponent<Enemy>();
     //   EnemyManagerPro.AddEnemy(enem);
        //float min;

    }

    // Update is called once per frame
    void Update()
    {

        realcheckTime -= Time.deltaTime;
        if(realcheckTime <= 0)
        {
            //print(isAttack);
            realcheckTime = checkTime;
            
            check();
            
        }
        //print(emk.agent.remainingDistance < emk.agent.stoppingDistance);
        //print(Vector3.Distance(destTower.position, transform.position));
        if (isStand)
        {
            check();
            //isStand = false;
            //isGiveUp = false;
            //state_Attack();
 
        }

        if (isGoingToDist)
        {
            //print(Vector3.Distance(destTower.position, transform.position));
            //print(emk.agent.remainingDistance);
            if (Vector3.Distance((Vector3)destTower, transform.position) < 5 && emk.agent.remainingDistance < emk.agent.stoppingDistance)
            {
                isGoingToDist = false;
                IsAttack = true;
                stateAttack();
            }
        }
        
        //if (isGiveUp == true)
        //{
        //   state_Attack();

        //}
        //Debug.Log(enem.GetHealthRatio());
    }

    void stateAttack()
    {
        //Debug.Log(isAttack + " " + destTower + "ATAKING");
    }

    void stateGoToDestanation()
    {
        if (TowerManager.towers.Count > 0)
        {
            // min = Vector3.Distance(TowerManager.towers[0].transform.position, transform.position);
            min = float.PositiveInfinity;
            // destTower = TowerManager.towers[0].transform;
            Vector3 fromTargetTowerToEnemy;
            //Debug.Log(TowerManager.towers.Count);
            for (int i = 0; i < TowerManager.towers.Count; i++)
            {
                Vector3? current_dist = TowerManager.towers[i].transform.position;
                

                if (min > Vector3.Distance((Vector3)current_dist, transform.position))
                {
                    min = Vector3.Distance((Vector3)current_dist, transform.position);
                //    destTower = TowerManager.towers[i].transform;
                //    destTower.position = current_dist;
                    targetTower = TowerManager.towers[i];
                }
            }
            //Debug.Log(destTower.position);
            fromTargetTowerToEnemy = transform.position - targetTower.transform.position;
            destTower = targetTower.transform.position + fromTargetTowerToEnemy.normalized;
         //   destTower.position = targetTower.transform.position - fromTargetTowerToEnemy.normalized;
            emk.SetDest((Vector3)destTower);
            isGoingToDist = true;
            //isAttack = true;
        }
        else
        {
            isStand = true;
        }
    }
    void stateGiveUp()
    {
        if(EnemyManagerPro.enemiesMap[EnemyType.Healer].Count > 0)
        {
            //Debug.Log("checking Heal");
            targetTower = null;
            Vector3 fromHealerToEnemy;
            min = float.PositiveInfinity;
        //    destHel = EnemyManagerPro.enemiesMap[EnemyType.Healer][0].gameObject.transform.position;
            for (int i = 0; i < EnemyManagerPro.enemiesMap[EnemyType.Healer].Count; i++)
            {
                Vector3 current_dist = EnemyManagerPro.enemiesMap[EnemyType.Healer][i].gameObject.transform.position;

                if (min > Vector3.Distance(current_dist, transform.position))
                {
                    min = Vector3.Distance(current_dist, transform.position);
                    destHel = current_dist;
                }
            }
            //Debug.Log("going");
            fromHealerToEnemy = transform.position - (Vector3)destHel;
            destHel = (Vector3)destHel + fromHealerToEnemy.normalized;
            emk.SetDest((Vector3)destHel);
        }
        
    }
    /*
    public bool getAttackState()
    {
        return IsAttack;
    }
    */
    public void Attack() {
        targetTower.towerHealth.ApplyDamage(40, Vector3.zero, Vector3.zero);
    }

    public void check()
    {
        
        float heals = enem.GetHealthRatio();
        //Debug.Log(heals + " " + brawe);
        if((heals - brawe) <= 0 && heals != 0) //Логика состояний
        {
            //Debug.Log("checking");
            isStand = false;
            isGoingToDist = false;
            IsAttack = false;
            //state_Attack();
            stateGiveUp();
            isGiveUp = true;
        }
        else
        {
            if (isGiveUp)
            {
                if(heals > 0.5 + brawe/2)
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
                    if (destTower != null)
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
