using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfSquad : Enemy_Logic
{
    private LeaderOfSqad leader;
    protected bool isSquad = false;
    protected Vector3 tempDestPosition;

    private Enemy_Logic disabledEnemyLogic;

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        base.Start();
    }
    void Update()
    {
        base.Update();
        /*
        realcheckTime -= Time.deltaTime;
        if (realcheckTime <= 0)
        {
            realcheckTime = checkTime;
            check();
        }
        if (isStand)
        {
            check();
        }
        */
    }

    public void setEnemyLogicScript(Enemy_Logic script)
    {
        disabledEnemyLogic = script;
    }

    public void setSquad(LeaderOfSqad leader)
    {
        //print("+");
        this.leader = leader;
        tempDestPosition = leader.transform.position - this.transform.position;
        isSquad = true;
    }

    protected void stateFoloving()
    {
        EnemyMouseController em = leader.emk;
        Vector3 posTemp = em.transform.position - tempDestPosition; ;
        emk.SetDestination(posTemp);
        emk.agent.speed = em.agent.speed * 1.5f;
    }

    public void cancelSquad()
    {
        //print("=");
        emk.agent.speed = tempNavAgentSpeed;
        isSquad = false;
        leader.removeFromSquad(this);
        check();
    }

    public override void check()
    {
        //print("+");
        float heals = enem.GetHealthRatio();

        if (heals <= 0.8 && isSquad)
        {
            disabledEnemyLogic.enabled = true;
            this.enabled = false;
            Destroy(this);
            cancelSquad();
        }

        if (isSquad && leader)
        {
            stateFoloving();
            return;
        }

        if (!isSquad)
        {
            //cancelSquad();
            disabledEnemyLogic.enabled = true;
            Destroy(this);
            //cancelSquad();
        }

        //disabledEnemyLogic.check();
    }
}
