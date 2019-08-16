using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    public static float basicTowerCount = 150f;
    public static float basicEnemyCount = 100f;
    public float basicSpawnPortalCooldown = 15f;
    //public List<Sqad> sqads;
    public GameObject enemie;

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
            spawnSqad(Sqad.Formation.Circle);

        }   
    /* 

        if (spawnPortalCooldown <= 0 && countTotalCount() > 0)
        {

            randPos = Random.Range(0f, 360f);
            spawnPortalCooldown = basicSpawnPortalCooldown;
            Instantiate(portal, new Vector3(23 * Mathf.Sin(randPos), 0.5f, 23 * Mathf.Cos(randPos)), this.transform.rotation, null);
        }
    */
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
                new SquadFormationSquare(enemie, enemie, 4, 3, 1f, 25, randPos);
                break;
            case Sqad.Formation.Straight:
                randPos = Random.Range(0f, 360f);
                new SquadFormationSquare(enemie, enemie, 1, 5, 0.75f, 25, randPos);
                break;
            case Sqad.Formation.Circle:
                randPos = Random.Range(0f, 360f);
                new SquadFormationCircle(enemie, enemie, 1, 10, 2, 25, randPos);
                break;
        }
    }
}
