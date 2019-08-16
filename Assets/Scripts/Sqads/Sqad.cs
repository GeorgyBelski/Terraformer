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
    public List<GameObject> SquadMembers;

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

    /*public void spawn(Vector3 position, Quaternion rotation)
   {
       print(position);
       next = Instantiate(leader, position, rotation);
       print(next.transform.position);
       next.transform.LookAt(Vector3.zero);
       Instantiate(enemie, new Vector3(Mathf.Sin(next.transform.eulerAngles.y) + 0.5f, position.y, Mathf.Cos(next.transform.eulerAngles.y) + 0.5f), rotation);
       Instantiate(enemie, new Vector3(Mathf.Sin(next.transform.eulerAngles.y) + 1f, position.y, Mathf.Cos(next.transform.eulerAngles.y) + 1f), rotation);
       Instantiate(enemie, new Vector3(Mathf.Sin(next.transform.eulerAngles.y) + 1.5f, position.y, Mathf.Cos(next.transform.eulerAngles.y) + 1.5f), rotation);
       //position = new Vector3(position.x, position.y, position.z);


       for(int i = 0; i < columnsCount; i++)
       {
           for(int y = 0; y < columnsSize; y++)
           {

           }
       }

   }*/
}
