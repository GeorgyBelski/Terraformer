using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sqad : MonoBehaviour
{
    //public GameObject leader;
    public GameObject enemie;
    public int columnsCount;
    public int columnsSize;
    public float columnsRange;
    public GameObject next;
    public List<PartOfSquad> SquadMembers;
    public Enemy_Logic leaderLogic;

    public Sqad(GameObject leader, GameObject enemie, int columnsCount, int columnsSize, float columnsRange)
    {
        //this.leader = leader;
        this.enemie = enemie;
        this.columnsCount = columnsCount;
        this.columnsSize = columnsSize;
        this.columnsSize = columnsSize;
    }
    public enum Formation
    {
        Square,
        Straight,
        Circle
    }

    public void setEnemies(GameObject leader, GameObject enemie)
    {
        this.enemie = enemie;
        //this.leader = leader;
    }

    public static float DegreeToRadian(double angle)
    {
        return (float)(Mathf.PI * angle / 180);
    }

    protected LeaderOfSqad formateLeader(GameObject leader)
    {
        LeaderOfSqad sqadController = leader.AddComponent<LeaderOfSqad>();
        leaderLogic = leader.GetComponent<Enemy_Logic>();
        sqadController.setEnemyLogicScript(leaderLogic);

        activateSquadLogic(sqadController, leaderLogic);

        leader.transform.LookAt(Vector3.zero);
        return sqadController;
    }
    
    protected PartOfSquad formatePart(GameObject part)
    {
        PartOfSquad partController = part.AddComponent<PartOfSquad>();
        part.transform.LookAt(Vector3.zero);
        Enemy_Logic partlogic = part.GetComponent<Enemy_Logic>();

        ////////////////////////////////////////////////////
        partController.setEnemyLogicScript(partlogic);

        activateSquadLogic(partController, partlogic);
        return partController;
    }

    private void activateSquadLogic(Enemy_Logic controller, Enemy_Logic mainpart)
    {
        controller.animator = mainpart.animator;
        controller.emk = mainpart.emk;
        controller.enem = mainpart.enem;
        mainpart.enabled = false;
    }

   
}
