using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    public static float basicTowerCount = 200f;
    public static float basicEnemyCount = 10f;
    public float basicSpawnPortalCooldown = 15f;

    public GameObject portal;
    private static GameObject[] tower;
    private static GameObject[] enemy;

    //private float totalCount;
    private float spawnPortalCooldown = 0;
    private float randPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnPortalCooldown -= Time.deltaTime;


        if (spawnPortalCooldown <= 0 && countTotalCount() > 0)
        {

            randPos = Random.Range(0f, 360f);
            spawnPortalCooldown = basicSpawnPortalCooldown;
            Instantiate(portal, new Vector3(30 * Mathf.Sin(randPos), 0.5f, 30 * Mathf.Cos(randPos)), this.transform.rotation, null);
        }
    }

    public static float countTotalCount()
    {
        tower = GameObject.FindGameObjectsWithTag("Tower");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        return (tower.Length * basicTowerCount - basicEnemyCount * enemy.Length) / basicEnemyCount;
    }
}
