using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public GameObject enemy; 
    public float basicLifeTime = 5f;
    public float spawnRate = 1f;

    private float realSpawnRate = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject tower = GameObject.Find("Tower");
        transform.LookAt(new Vector3(0, 0, 0));
        transform.Rotate(new Vector3(0, 90, 0));// = Quaternion.Euler(0f, 90f, 0);
        //transform.rotation = new Vector3(transform.rota.x, transform.rotation.y, transform.rotation.z);


    }

    // Update is called once per frame
    void Update()
    {
        realSpawnRate -= Time.deltaTime;
        print(Logic.countTotalCount());
        if((int)(Logic.countTotalCount()) >= 1 && realSpawnRate <=0)
        {
            realSpawnRate = spawnRate;
            Instantiate(enemy, transform.position, transform.rotation, null);
        }
        basicLifeTime -= Time.deltaTime;
        if(basicLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
