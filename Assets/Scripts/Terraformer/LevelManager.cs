using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Range(1, 3)]
    public int levelNumber = 1;

    public static int level;
    public int firstLevelEnemyCount = 20;
    public static int killedEnemyCount =25;
    public int resourceVictoryCondition = 700;
    public static int resourceCondition = 1000;
    static bool isNewAtmosphere;
    public TextMeshProUGUI taskDescription;
    public string firstLevelDescription, secondLevelDescription, thirdLevelDescription;
    
    
    void Start()
    {
        level = levelNumber;
        killedEnemyCount = firstLevelEnemyCount;
        resourceCondition = resourceVictoryCondition;
        isNewAtmosphere = false;
        SetTaskDescription();
    }
    void Update()
    {
        NewAtmosphere();
    }

    void SetTaskDescription()
    {
        if(!taskDescription)
        { return; }

        if (level == 1)
        {
            taskDescription.text = firstLevelDescription;
        }
        else if (level == 2)
        {
            taskDescription.text = secondLevelDescription;
        }
        else 
        {
            taskDescription.text = thirdLevelDescription;
        }
    }

    public static void Defeat()
    {
        if (MenuController.victory.gameObject.activeSelf)
        { return; }

        MenuController.ShowMenu(true);
        MenuController.ShowDefeat(true);
        ResourceManager.ApplyDefeat();
    }

    public static void CheckFirstLevelCondition() 
    {
        if (level == 1 && EnemyManagerPro.killedEnemies >= killedEnemyCount)
        {
            Victory();
        }

    }
    public static void CheckSecondLevelCondition()
    {
        if (level == 2 && ResourceManager.resource >= resourceCondition)
        {
            Victory();
        }
    }

    public static void Victory()
    {   
        MenuController.ShowVictory(true);
        Terraformer.terraformer.IntermediateVictory();

        if (level == 3) { 
            Terraformer.isVictoryWave = true;
            isNewAtmosphere = true;
            /*
            foreach (var enemy in EnemyManagerPro.enemies) {
                if (enemy)
                { enemy.ApplyDamage(enemy.maxHealth, Vector3.zero, Vector3.zero); }
            }
            */
        }
    }

    void NewAtmosphere()
    {
        if (!isNewAtmosphere) 
        { return; }

        if (EnemyManagerPro.enemies.Count != 0)
        {
            var enemy = EnemyManagerPro.enemies.ToArray()[0];
            enemy.ApplyDamage(enemy.maxHealth, Vector3.zero, Vector3.zero);
        }
    }
}
