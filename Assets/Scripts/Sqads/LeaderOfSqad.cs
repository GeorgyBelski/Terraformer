﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderOfSqad : Enemy_Logic
{
    protected List<PartOfSquad> Squad;

    protected bool isLeader = false;
    protected Enemy_Logic leader;
    protected Vector3 tempDestPosition;

    private Enemy_Logic disabledEnemyLogic;

    public override void Attack()
    {
        disabledEnemyLogic.Attack();
    }
    void Start()
    {
        //print("+");
        //gameObject.GetComponent<Enemy_Logic>().Start();
        base.Start();
    }
    void Update()
    {
        //gameObject.GetComponent<Enemy_Logic>().Update();
        base.Update();
    }


    public void setEnemyLogicScript(Enemy_Logic script)
    {
        disabledEnemyLogic = script;
        //disabledEnemyLogic.Start();

    }



    public void setLeaderSquad(List<PartOfSquad> squad)
    {
        //print(squad.Count);
        this.Squad = squad;
        isLeader = true;
        for (int i = 0; i < Squad.Count; i++)
        {
            //print("+");
            Squad[i].setSquad(this);
        }
        //disabledEnemyLogic.Update();
    }

    public void removeFromSquad(PartOfSquad part)
    {
        
        if(Squad.Contains(part))
            this.Squad.Remove(part);
    }

    public void cancelSquad()
    {
        emk.agent.speed = tempNavAgentSpeed;
        check();
    }

    public override void check()
    {
        float heals = enem.GetHealthRatio();


        //print(disabledEnemyLogic.isGoingToDest);
        if(Vector3.Distance(transform.position, emk.GetDest()) < 2 && isLeader && disabledEnemyLogic.isGoingToDest)
        {
            isLeader = false;
            check();
        }

        if (heals <= 0.7 && isLeader)
        {
            isLeader = false;
            check();
        }

        


        if (!isLeader && Squad != null)
        {
            //print("+");
            disabledEnemyLogic.enabled = true;

            int count = Squad.Count;
            for (int i = 0; i < count; i++)
            {
                //print("+");
                Squad[0].cancelSquad();
            }
            isLeader = false;
            Squad = null;
            this.enabled = false;
            Destroy(this);
        }

        disabledEnemyLogic.check();
    }

    

}
