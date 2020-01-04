using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy_Logic
{
    public TypeOfEnemy type;
    public int damageOnAttack;
    // Start is called before the first frame update
    void Start()
    {
        
        //print(1);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //print(1);
        base.Update();
    }

    public override void Attack()
    {
        if (targetTower)
        {
            //base.Attack();
            targetTower.towerHealth.ApplyDamage(damageOnAttack, Vector3.zero, Vector3.zero);
        }
    }
    public override void check()
    {


        //print(isAttack + " " + isStand + " " + isGoingToDest);
        //print(-1);
        float heals = enem.GetHealthRatio();

        if (isAttack || isGoingToDest)
        {

            if (base.destTower == null)
            {
                //print("+");
                isStand = false;
                isAttack = false;
                if(isPriority)
                    stateGoToDestanation(priorityTowerType);
                else
                    if (TowerManager.terraformer)
                        stateGoToDestanation(TowerType.Terraformer);
                    else
                        isStand = true;
            }
            else
            {
                //print("+");
                isAttack = true;
            }
        }
        else
        {
            //print("+");
            isStand = false;
            isAttack = false;
            if (TowerManager.terraformer)
                stateGoToDestanation(TowerType.Terraformer);
            else
                isStand = true;
            //isGoingToDist = true;
        }
            
        
    }


}
