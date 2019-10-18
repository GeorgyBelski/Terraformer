using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_moving : MonoBehaviour
{
    public float speed = 3f;

    private GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.Find("Tower");
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(new Vector3(tower.transform.position.x, transform.position.y, tower.transform.position.z));
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        
    }
}
