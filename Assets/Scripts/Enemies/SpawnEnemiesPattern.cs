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

    public PortalSettings portal;

    public float spawnRate;
    public float portalPosition;
    public int portalRange = 22;
    private int realPortalRange;

    private int wave;

    public Sqad.Formation formation;
    public int colCount;
    public int colSize;
    public float range;
    private WaveType patternType;



    private int enemieCountInList = 0;
    private bool spawning;

    private List<GameObject> list;

    void Start()
    {
        realPortalRange = portalRange;
    }

    public void setPattern(int wave)
    {
        this.wave = wave;
        realPortalRange = portalRange + wave;
        list = new List<GameObject>();
        switch (wave)
        {
            case 1:
                patternType = WaveType.Simple;
                /*
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Circle;
                colCount = 1;
                colSize = 9;
                range = 3;
                list.Add(enemyTank);
                list.Add(enemyDamager);
                */
                addEnemy(4, enemyDamager);
                spawnRate = 1f;
                portalPosition = 0;
                break;
            case 2:
                
                addEnemy(5, enemyDamager);
                addEnemy(1, enemyHeal);
                break;
            case 3:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Square;
                colCount = 2;
                colSize = 4;
                range = 1;
                list.Add(enemyRusher);
                list.Add(enemyRusher);
                //addEnemy(7, enemyRusher);
                //addEnemy(1, enemyHeal);
                //spawnRate = 0.5f;
                break;
            case 4:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Square;
                colCount = 3;
                colSize = 3;
                range = 0.7f;
                list.Add(enemyTank);
                list.Add(enemyDamager);
                portalPosition = 90;

                break;
            case 5:
                patternType = WaveType.Simple;
                spawnRate = 1f;
                //enemyRusher.setPriority(TowerType.Terraformer);
                addEnemy(15, enemyDamager);
                addEnemy(2, enemyHeal);
                break;
            case 6:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Square;
                colCount = 3;
                colSize = 4;
                range = 1;
                list.Add(enemyTank);
                list.Add(enemyRusher);
                spawnRate = 0.5f;
                break;
            case 7:
                patternType = WaveType.Simple;
                //addEnemy(3, enemyTank);
                addEnemy(15, enemyDamager);
                addEnemy(10, enemyRusher);
                addEnemy(2, enemyHeal);
                spawnRate = 0.3f;
                break;
            case 8:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Circle;
                colCount = 1;
                colSize = 5;
                range = 4;
                list.Add(enemyTank);
                list.Add(enemyTank);
                //addEnemy(6, enemyTank);
                //addEnemy(3, enemyRusher);
                //addEnemy(1, enemyHeal);
                spawnRate = 0.5f;
                portalPosition = 270;
                break;
            case 9:
                patternType = WaveType.Simple;
                addEnemy(22, enemyRusher);
                //addEnemy(3, enemyRusher);
                addEnemy(1, enemyHeal);
                spawnRate = 0.2f;
                portalPosition = 120;
                break;

            case 10:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Square;
                colCount = 4;
                colSize = 6;
                range = 1;
                list.Add(enemyTank);
                list.Add(enemyRusher);
                portalPosition = 120;
                break;

            case 11:
                patternType = WaveType.Simple;
                addEnemy(12, enemyRusher);
                addEnemy(2, enemyHeal);
                addEnemy(12, enemyDamager);
                addEnemy(12, enemyRusher);
                //addEnemy(22, enemyRusher);
                //addEnemy(3, enemyRusher);
                addEnemy(2, enemyHeal);
                spawnRate = 0.5f;
                portalPosition = 180;
                break;

            case 12:
                patternType = WaveType.Squad;
                formation = Sqad.Formation.Square;
                colCount = 3;
                colSize = 4;
                range = 2;
                list.Add(enemyTank);
                list.Add(enemyTank);
                portalPosition = 250;
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

    //////
    ///-------------------------------------------
    ///Enemy Spawning
    ///-------------------------------------------
    /////
    public void spawnEnemyes()
    {
        switch (patternType)
        {
            case WaveType.Simple:
                simpleSpawn();
                break;
            case WaveType.Squad:
                squadSpawn();
                break;
        }

    }

    private void simpleSpawn()
    {
        Enemy enem = Instantiate(list[enemieCountInList], new Vector3(
            portal.transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / list.Count - 1))) * 2),
            0,
            portal.transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / list.Count - 1))) * 2)), portal.transform.rotation)
            .GetComponent<Enemy>();

        enem.maxHealth = enem.maxHealth + 100 * wave;
        enem.health = enem.maxHealth;

        enemieCountInList++;


        if (list.Count <= enemieCountInList)
        {
            enemieCountInList = 0;
            portal.deactivate();
            //print("+");

        }
    }

    private void squadSpawn()
    {
        switch (formation)
        {
            case Sqad.Formation.Square:
                new SquadFormationSquare(list[0], list[1], colCount, colSize, range, realPortalRange, portalPosition);
                break;
            case Sqad.Formation.Circle:
                new SquadFormationCircle(list[0], list[1], colCount, colSize, range, realPortalRange, portalPosition);
                break;
        }
        portal.deactivate();
    }



}
