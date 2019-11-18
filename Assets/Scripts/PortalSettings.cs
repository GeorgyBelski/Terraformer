using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSettings : MonoBehaviour
{
    private List<GameObject> enemyList = new List<GameObject>();
    public float sizeInSecond = 1;

    private float size = 5;
    private GameObject leader;
    private GameObject next;

    private bool spawn = false;

    void Update()
    {
        //print("+"); 
        if (this.enabled)
        {
            transform.localScale = new Vector3(transform.localScale.x + sizeInSecond * Time.deltaTime, transform.localScale.y + sizeInSecond * Time.deltaTime, transform.localScale.z + sizeInSecond * Time.deltaTime);
        
        //print(transform.localScale.x + " " + size);
            if(transform.localScale.x >= size && spawn)
            {
                spawn = false;
                //gameObject.active = false;
                spawnEnemyes();
                transform.localScale = Vector3.zero;
                transform.position = Vector3.zero;
                this.enabled = false;
                //gameObject. = false

            }
        }
    }

    public void spawnEnemyes()
    {
        //print(enemyList.Count);
        //engl = SquadFormationSquare.DegreeToRadian(engl);
        leader = enemyList[0];
        Instantiate(leader, transform.position, this.transform.rotation);
        for (int i = 1; i < enemyList.Count - 1; i++)
        {
            Instantiate(enemyList[i], new Vector3(
                this.transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(i * (360 / enemyList.Count - 1))) * 1 * enemyList.Count/5),
                0,
                this.transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(i * (360 / enemyList.Count - 1))) * 1 * enemyList.Count / 5)), leader.transform.rotation);
        }
        gameObject.active = false;
    }

    public void setSetings(List<GameObject> list)
    {
        enemyList = list;
        //size = list.Count;
        //print(size);
        spawn = true;
        this.enabled = true;
    }
}
