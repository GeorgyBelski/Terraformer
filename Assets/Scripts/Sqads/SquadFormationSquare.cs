using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadFormationSquare : Sqad
{
    //private GameObject next;
    private Transform spawner;

    public SquadFormationSquare(GameObject leader, GameObject enemie, int columnsCount, int columnsSize, float columnsRange, int pos, float engl) : base(leader, enemie, columnsCount, columnsSize, columnsRange)
    {
        SquadMembers = new List<GameObject>();
        engl = DegreeToRadian(engl);
        next = Instantiate(leader, new Vector3(pos * Mathf.Sin(engl), 0, pos * Mathf.Cos(engl)), new Quaternion(0, 0, 0, 0));
        next.transform.LookAt(Vector3.zero);
        leader = next;
        spawner = next.transform;
        for (int i = 0; i < columnsCount; i++) {
            next = Instantiate(
                enemie, 
                new Vector3(spawner.transform.position.x + Mathf.Sin(engl) * columnsRange, 0, spawner.transform.position.z + Mathf.Cos(engl) * columnsRange), 
                spawner.rotation);
            SquadMembers.Add(next);

            next.GetComponent<Enemy_Logic>().setSquad(leader);

            next.transform.position = new Vector3(
                next.transform.position.x + Mathf.Sin(DegreeToRadian(next.transform.eulerAngles.y + 90)) * (columnsRange/2 * (columnsCount - 1)),
                0,
                next.transform.position.z + Mathf.Cos(DegreeToRadian(next.transform.eulerAngles.y + 90)) * (columnsRange/2 *(columnsCount - 1)));

            if (i > 0)
                next.transform.position = new Vector3(
                    next.transform.position.x + Mathf.Sin(DegreeToRadian(next.transform.eulerAngles.y - 90)) * i * columnsRange,
                    0,
                    next.transform.position.z + Mathf.Cos(DegreeToRadian(next.transform.eulerAngles.y - 90)) * i * columnsRange);

            for (int y = 0; y < columnsSize - 1; y++){
                next = Instantiate(enemie, new Vector3(
                    next.transform.position.x + Mathf.Sin(engl) * columnsRange,
                    0,
                    next.transform.position.z + Mathf.Cos(engl) * columnsRange), spawner.rotation);

                SquadMembers.Add(next);
                next.GetComponent<Enemy_Logic>().setSquad(leader);
            }
        }

        leader.GetComponent<Enemy_Logic>().setLeaderSquad(SquadMembers);
    }

    public void spawn(Vector3 position, Quaternion rotation)
    {
        
    }

}
