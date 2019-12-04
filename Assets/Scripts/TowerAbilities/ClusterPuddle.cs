using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterPuddle : MonoBehaviour
{
    public float existeTime;
    public float radius;

    private float defaultspeed;
    void Start()
    {
        //mt = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        existeTime -= Time.deltaTime;
        //print("+");
        //transform.position = Nev Vector
        transform.localScale = new Vector3(radius, 0.3f, radius);
        if (existeTime <= 0)
            Destroy(gameObject);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == EnemyManagerPro.enemyLayer && EnemyManagerPro.checking(other.gameObject.GetComponent<Enemy>()))
        {
            
            EnemyEffectsController enemy = other.gameObject.GetComponent<EnemyEffectsController>();
            //defaultspeed = enemy.character.m_MoveSpeedMultiplier;
            enemy.AddSlowdown(2, 0.5f);


            //enemy.character.m_MoveSpeedMultiplier /= 2;
        }
    }

/*    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == EnemyManagerPro.enemyLayer && EnemyManagerPro.checking(other.gameObject.GetComponent<Enemy>()))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.character.m_MoveSpeedMultiplier = defaultspeed;
        }
    }
*/

    public void setSetings(float existeTime, float radius)
    {
        this.existeTime = existeTime;
        this.radius = radius;
    }
}
