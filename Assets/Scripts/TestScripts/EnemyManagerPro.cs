﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerPro : MonoBehaviour
{
    public static int enemyLayerMask = 1 << 12;
    public static int enemyLayer = 12;

    public static List<Enemy> enemies = new List<Enemy>();

    public static Dictionary<EnemyType, List<Enemy>> enemiesMap= new Dictionary<EnemyType, List<Enemy>>();
    public static Dictionary<Transform, Enemy> TransformEnemyMap = new Dictionary<Transform, Enemy>();
    public int enemyCount;// for debuging

    public static int killedEnemies = 0;
    public static void Restart()
    {
        enemies.Clear();
        enemiesMap.Clear();
        killedEnemies = 0;
    }
    private void Start()
    {
        //enemiesMap.Add("Damager", enemiesDamage);
        //enemiesMap.Add("Healer", enemiesHealers);
        //foreach (Enemy startEnemy in startEnemies)
        //{
        //enemies.Add(startEnemy);
        //enemiesMap.Add("Damage", startEnemy);
        //}
        //enemiesDamage.Add(startEnemies[1]);//Testing
        //enemiesHealers.Add(startEnemies[0]);
        //enemiesHealers.Add(startEnemies[2]);
    }
    private void Update()
    {
        enemyCount = enemies.Count;
    }
    //public static void 

    public static void AddEnemy(Enemy enemy)
    {
        if (enemiesMap.ContainsKey(enemy.type))
        {
            enemiesMap[enemy.type].Add(enemy);
            enemies.Add(enemy);
            
        }
        else
        {
            //List<Enemy> enemiesDamage = new List<Enemy>();
            enemiesMap.Add(enemy.type, new List<Enemy>());
            enemiesMap[enemy.type].Add(enemy);
            enemies.Add(enemy);
        }
        TransformEnemyMap.Add(enemy.transform, enemy);
        //enemiesMap.Add(name, enem);
        //print(enemiesMap[enemy.type]);
        
        
    }

    public static void RemoveEnemy(Enemy enemy)
    {
    //  if (enemies.Contains(enemy))
    //  {
        enemies.Remove(enemy);
        //enemiesMap[enem.getName].Remove(enem);
        enemiesMap[enemy.type].Remove(enemy);
        TransformEnemyMap.Remove(enemy.transform);

        if (enemy.type != EnemyType.Totem)
        {
            killedEnemies++;
            MenuController.RewriteKilledEnemiesCount();
            LevelManager.CheckFirstLevelCondition();
        }
    // }
    }

    public static bool checking(Enemy enem)
    {
        if (enem!= null && enemies.Exists(e => e.Equals(enem)))
        {
            return true;
        }
        return false;

    }


}
