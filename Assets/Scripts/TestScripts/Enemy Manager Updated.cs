using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerUpdated : MonoBehaviour
{
    public List<Enemy> startEnemies = new List<Enemy>();
    public static List<Enemy> enemies = new List<Enemy>();



    private void Start()
    {
        foreach (Enemy startEnemy in startEnemies)
        {
            enemies.Add(startEnemy);
        }
    }
}
