using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static int enemyLayerMask;
    public List<Enemy> startEnemies = new List<Enemy>();
    public static List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");

        foreach (Enemy startEnemy in startEnemies){
            enemies.Add(startEnemy);
        }
    }
}
