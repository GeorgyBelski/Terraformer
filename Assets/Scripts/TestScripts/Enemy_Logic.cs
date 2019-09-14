
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfEnemy { Damager, Tank, Saboteur };

public abstract class Enemy_Logic : MonoBehaviour
{
    [Header("Enemy Controllers")]
    public Animator animator;
    public EnemyMouseController emk;
    public Enemy enem;
    //public NavMeshAgent navAgent;

    [Header("Priority Check Timer")]
    public float checkTime = 0.5f;


    protected Vector3? destTower = null;
    protected Vector3? destHel = null;
    protected Tower targetTower;

    public bool isGoingToDest = false;
    protected bool isStand = true;
    protected bool isGiveUp = false;
    protected bool isAttack = false;

    protected bool isRush = false;



    protected float realcheckTime;//change!!!!!!!!!!!!!!!!!!!!!
    protected float brawe = 0; //= Random.RandomRange(0.1f, 0.7f);
    protected float min;
    protected float tempNavAgentSpeed;

    public bool IsAttack { get => isAttack; set { isAttack = value; animator.SetBool("Attack", value); } }


    // Start is called before the first frame update
    protected void Start()
    {
       
        brawe = Random.Range(0.1f, 0.7f);
        realcheckTime = checkTime;
        tempNavAgentSpeed = emk.agent.speed;
    }

    // Update is called once per frame
    protected void Update()
    {
        //print("+");

        realcheckTime -= Time.deltaTime;
        if(realcheckTime <= 0)
        {
            realcheckTime = checkTime;
            check();
        }
        if (isStand)
        {
            check();
        }

        if (isGoingToDest)
        {
            if (Vector3.Distance((Vector3)destTower, transform.position) < 5 && emk.agent.remainingDistance < emk.agent.stoppingDistance)
            {
                isGoingToDest = false;
                IsAttack = true;
                stateAttack();
            }
        }
    }

    protected void stateAttack()
    {
        //Debug.Log(isAttack + " " + destTower + "ATAKING");
    }

    protected void stateGoToDestanation()
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
            isGoingToDest = true;
            //isAttack = true;
            
        }
        else
        {
            isStand = true;
        }
    }

    protected void stateGoToDestanation(TowerType destTowerType) 
    {
        
        Vector3 fromTargetTowerToEnemy;
        targetTower = TowerManager.GetNearestTower(this.transform, destTowerType);
        fromTargetTowerToEnemy = transform.position - targetTower.transform.position;
        destTower = targetTower.transform.position + fromTargetTowerToEnemy.normalized;
        emk.SetDest((Vector3)destTower);
        isGoingToDest = true;
        print(destTower);

    }

    protected void stateGiveUp()
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
                if (EnemyManagerPro.enemiesMap[EnemyType.Healer][i] == null) {
                    return;
                }
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



    public abstract void Attack();
    //{
    //targetTower.towerHealth.ApplyDamage(damage, Vector3.zero, Vector3.zero);
    //}

    /// <summary>
    /// /////////////////////////////////SQUADS
    /// </summary>

   

/// <summary>
/// /////////////////////////////////SQUADS
/// </summary>

    public virtual void check()
    {
        //print("+");
        //print("+");
        //print(-1);
        float heals = enem.GetHealthRatio();
        //Debug.Log(heals + " " + brawe);
        if ((heals - brawe) <= 0 && heals != 0) //Логика состояний
        {
            if(EnemyManagerPro.enemiesMap.ContainsKey(EnemyType.Healer) && EnemyManagerPro.enemiesMap[EnemyType.Healer].Count > 0) { 
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
                if(heals > 0.5 + brawe/2 || EnemyManagerPro.enemiesMap[EnemyType.Healer].Count == 0)
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
