using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerPro : MonoBehaviour
{
    public static int enemyLayerMask;
    // public List<Enemy> startEnemies = new List<Enemy>();
    //public static List<Enemy> enemiesDamage = new List<Enemy>();
    //public static List<Enemy> enemiesHealers = new List<Enemy>();
    public static List<Enemy> enemies = new List<Enemy>();
    //public static List<HealBase> healBases = new List<HealBase>();

    public static Dictionary<EnemyType, List<Enemy>> enemiesMap= new Dictionary<EnemyType, List<Enemy>>();

    private void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
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
        //enemiesMap.Add(name, enem);
        print(enemiesMap[enemy.type]);
        
        
    }

    public static void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        //enemiesMap[enem.getName].Remove(enem);
        enemiesMap[enemy.type].Remove(enemy);
    }

    public static bool checking(Enemy enem)
    {
        if (enemies.Exists(e => e.Equals(enem)))
        {
            return true;
        }
        return false;

    }


}
