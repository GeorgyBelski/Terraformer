using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadFormationCircle : Sqad
{

    public SquadFormationCircle(GameObject leader, GameObject enemie, int columnsCount, int columnsSize, float columnsRange, int pos, float engl) : base(leader, enemie, columnsCount, columnsSize, columnsRange)
    {
        SquadMembers = new List<GameObject>();
        //columnsSize--;
        engl = SquadFormationSquare.DegreeToRadian(engl);
        leader = Instantiate(leader, new Vector3(pos * Mathf.Sin(engl), 0, pos * Mathf.Cos(engl)), new Quaternion(0, 0, 0, 0));
        //this.leader = leader;
        leader.transform.LookAt(Vector3.zero);

        for(int i = 0; i < columnsSize; i++)
        {
            next = Instantiate(enemie, new Vector3(
                leader.transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(i * (360 / columnsSize))) * columnsRange),
                0,
                leader.transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(i * (360 / columnsSize))) * columnsRange)), leader.transform.rotation);

            SquadMembers.Add(next);

            next.GetComponent<Enemy_Logic>().setSquad(leader);

            //print((Mathf.Sin(0 + i * (360 / (columnsSize - 1))) * columnsRange + " " + Mathf.Cos(0 + i * (360 / (columnsSize - 1))) * columnsRange) + " " + i * (360 / (columnsSize - 1)));

        }

        leader.GetComponent<Enemy_Logic>().setLeaderSquad(SquadMembers);
    } 
}
