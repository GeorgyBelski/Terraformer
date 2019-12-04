using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logic : MonoBehaviour
{


    public GameObject enemyDamager;
    public GameObject enemyTank;
    public GameObject enemyRusher;
    public GameObject enemyHeal;

    public float enemyDamagerCount = 30;
    public float enemyTankCount = 70;
    public float enemyRusherCount = 30;
    public float enemyHealCount = 30;

    public float towerCount = 100;

    public float startCount = 200;

    private float wave = 0;

    public GameObject portal;
    public PortalSettings thisPortalSettings;
    public float difficulties = 1;

    public bool betweenWavesActivity = false;

    public float winPoints = 2000f;

    public float timer = 30f;
    private float realTimer;
    public TextMeshProUGUI timerText;
    private float totalCount;
    private List<GameObject> list;

    private float randPos;

    void Start()
    {
        portal.active = false;
        realTimer = timer / 2;
    }

    void Update()
    {
        realTimer -= Time.deltaTime;
        timerText.text = realTimer.ToString("0.0");
        if (realTimer <= 0)
        {
            //print("+");
            portal.transform.position = countVector();
            portal.active = true;

            thisPortalSettings.setSettings(conutWave());
            wave++;
            realTimer = timer + wave / difficulties;
        }
    }

    private Vector3 countVector()
    {
        randPos = Random.Range(0f, 60f);
        return new Vector3(23 + wave * Mathf.Sin(randPos), 1, 23 + wave * Mathf.Cos(randPos));
    }

    private List<GameObject> conutWave()
    {
        list = new List<GameObject>();
        totalCount = startCount + (startCount / 2 * difficulties * wave);//TowerManager.towers.Count * difficulties * towerCount;

        if (wave > 1 && wave % 2 == 0)
        {
            list.Add(enemyHeal);
            totalCount -= enemyHealCount;
        }


        int tanksCount = (int)totalCount / (int)(300 / difficulties);

        if(wave > 4 && wave % 3 == 0)
        {
            listAdding(tanksCount, enemyTank);
            totalCount -= tanksCount * enemyTankCount;
        }



        int simpleDamagerCount = (int)(totalCount * 0.6f);
        totalCount -= simpleDamagerCount;

        int rushCount = (int)(totalCount / enemyRusherCount);
        totalCount = 0;

        simpleDamagerCount = simpleDamagerCount / (int)enemyDamagerCount;

        listAdding(simpleDamagerCount, enemyDamager);
        listAdding(rushCount, enemyRusher);

        return list;
    }

    public void listAdding(int count, GameObject enemy)
    {
        for(int i = 0; i < count; i++)
        {
            list.Add(enemy);
        }
    }

}
  
        

        //return new List<Enemy>;
    




    
    /*public static float basicTowerCount = 150f;
    public static float basicEnemyCount = 100f;
    public float basicSpawnPortalCooldown = 15f;
    //public List<Sqad> sqads;
    public GameObject enemie;
    public GameObject leader;

    public GameObject portal;
    //private static GameObject[] tower;
    //private static GameObject[] enemy;

    //private float totalCount;
    private float spawnPortalCooldown = 0;
    private float randPos;

    //private EnemyManagerPro enemyManager;
    //private TowerManager towerManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnPortalCooldown -= Time.deltaTime;
        if(spawnPortalCooldown <= 0)
        {
            //Instantiate(enemie, new Vector3(23 * Mathf.Sin(randPos), 0.5f, 23 * Mathf.Cos(randPos)), new Quaternion(0, 0, 0, 0));
            //randPos = Random.Range(0f, 360f);
            spawnPortalCooldown = basicSpawnPortalCooldown;
            spawnSqad(Sqad.Formation.Square);

        }   
    /* 

        if (spawnPortalCooldown <= 0 && countTotalCount() > 0)
        {

            randPos = Random.Range(0f, 360f);
            spawnPortalCooldown = basicSpawnPortalCooldown;
            Instantiate(portal, new Vector3(23 * Mathf.Sin(randPos), 0.5f, 23 * Mathf.Cos(randPos)), this.transform.rotation, null);
        }
    
    }

    public static float countTotalCount()
    {
        //float tower = TowerManager.towers.Count;//GameObject.FindGameObjectsWithTag("Tower");
        //float enemy = EnemyManagerPro.enemies.Count;//GameObject.FindGameObjectsWithTag("Enemy");
        return (TowerManager.towers.Count * basicTowerCount - basicEnemyCount * EnemyManagerPro.enemies.Count); // basicEnemyCount;
    }

    private void spawnSqad(Sqad.Formation en)
    {
        switch (en)
        {
            case Sqad.Formation.Square:
                randPos = Random.Range(0f, 360f);
                new SquadFormationSquare(leader, enemie, 2, 3, 1f, 25, randPos);
                break;
            case Sqad.Formation.Straight:
                randPos = Random.Range(0f, 360f);
                new SquadFormationSquare(leader, enemie, 1, 0, 0.75f, 25, randPos);
                break;
            case Sqad.Formation.Circle:
                randPos = Random.Range(0f, 360f);
                new SquadFormationCircle(leader, enemie, 1, 10, 2, 25, randPos);
                break;
        }
    }
    */

