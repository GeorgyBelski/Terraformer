using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> startEnemies = new List<GameObject>();
    public static List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject startEnemy in startEnemies){
            enemies.Add(startEnemy);
        }
    }
}
