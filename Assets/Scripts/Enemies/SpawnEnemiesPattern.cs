using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesPattern : MonoBehaviour
{
    public enum WaveType
    {
        Simple, Squad 
    }

    public GameObject enemyDamager;
    public GameObject enemyTank;
    public GameObject enemyRusher;
    public GameObject enemyHeal;

    public float spawnRate;
    public float portalPosition;

    private int wave;

    public Sqad.Formation formation;
    public int colCount;
    public int colSize;
    public float range;
    private WaveType patternType;

    private List<GameObject> list;
    public void setPattern(int wave)
    {
        this.wave = wave;
        list = new List<GameObject>();
        switch (wave)
        {
            case 1:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Square;
                colCount = 3;
                colSize = 2;
                range = 1;
                //addEnemy(3, enemyTank);
                list.Add(enemyTank);
                list.Add(enemyDamager);
                spawnRate = 0.5f;
                portalPosition = 0;
                break;
            case 2:
                patternType = WaveType.Simple;
                addEnemy(5, enemyDamager);
                addEnemy(1, enemyHeal);
                break;
            case 3:
                addEnemy(7, enemyRusher);
                spawnRate = 0.1f;
                break;
            case 4:
                spawnRate = 1f;
                portalPosition = 90;
                patternType = WaveType.Simple;
                //enemyRusher.setPriority(TowerType.Terraformer);
                addEnemy(10, enemyDamager);

                break;
            case 5:

                addEnemy(2, enemyTank);
                addEnemy(3, enemyRusher);
                addEnemy(1, enemyHeal);
                spawnRate = 0.5f;
                break;
            case 6:
                addEnemy(1, enemyTank);
                addEnemy(5, enemyDamager);
                addEnemy(5, enemyRusher);
                addEnemy(1, enemyHeal);
                spawnRate = 0.5f;
                break;
            case 7:
                //addEnemy(3, enemyTank);
                addEnemy(15, enemyDamager);
                addEnemy(5, enemyRusher);
                spawnRate = 0.3f;
                break;
            case 8:
                addEnemy(6, enemyTank);
                //addEnemy(3, enemyRusher);
                addEnemy(1, enemyHeal);
                spawnRate = 0.5f;
                break;
            case 9:
                addEnemy(20, enemyRusher);
                //addEnemy(3, enemyRusher);
                addEnemy(1, enemyHeal);
                spawnRate = 0.2f;
                portalPosition = 120;
                break;
                default:
                addEnemy(5 * wave, enemyDamager.gameObject);
                break;

        }


        //List<Enemy> list = new List<Enemy>();

        //return list;
    }

    public List<GameObject> getPattern()
    {
        return list; 
    }

    public WaveType getPatType()
    {
        return patternType;
    }

    private void addEnemy(int count, GameObject enem)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(enem);
        }
            
    }
}
