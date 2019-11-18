using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{

    public GameObject portal;
    public PortalSettings thisPortalSettings;
    public float difficulties = 1;

    public bool betweenWavesActivity = false;

    public float winPoints = 2000f;

    private float timer = 30f;
    private float realTimer;

    private float randPos;

    void Start()
    {
        realTimer = timer;
        if(realTimer <= 0)
        {
            portal.transform.position = countVector();
        }
    }

    void Update()
    {

    }

    private Vector3 countVector()
    {
        randPos = Random.Range(0f, 360f);
        return new Vector3(23 * Mathf.Sin(randPos), 1, 23 * Mathf.Cos(randPos));
    }

    
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
}
