using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerPro : MonoBehaviour
{
    public List<Enemy> startEnemies = new List<Enemy>();
    //public static List<Enemy> enemiesDamage = new List<Enemy>();
    //public static List<Enemy> enemiesHealers = new List<Enemy>();
    public static List<Enemy> enemies = new List<Enemy>();
    //public static List<HealBase> healBases = new List<HealBase>();

    public static Dictionary<string, List<Enemy>> enemiesMap= new Dictionary<string, List<Enemy>>();

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

    //public static void 

    public static void addEnemy(string name, Enemy enem)
    {
        if (enemiesMap.ContainsKey(name))
        {
            enemiesMap[name].Add(enem);
            enemies.Add(enem);
        }
        else
        {
            //List<Enemy> enemiesDamage = new List<Enemy>();
            enemiesMap.Add(name, new List<Enemy>());
            enemiesMap[name].Add(enem);
            enemies.Add(enem);
        }
        //enemiesMap.Add(name, enem);
        print(enemiesMap[name]);
        
        
    }

    public static void removeEnemie(string name, Enemy enem)
    {
        enemies.Remove(enem);
        //enemiesMap[enem.getName].Remove(enem);
        enemiesMap[name].Remove(enem);
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
