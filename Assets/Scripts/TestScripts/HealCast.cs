using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCast : MonoBehaviour
{
    Material mt;
    public static float healPower = 100f;
    public static float radius = 5f;

    
    // Start is called before the first frame update
    void Start()
    {

        mt = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x + 0.25f, transform.localScale.y, transform.localScale.z + 0.25f);
        if(transform.localScale.x >= radius)
        {
            Destroy(gameObject);
        }
        //mt.color.a -= 1;
        //Debug.Log(mt.color.r);
        //mt.color = new Color(mt.color.r + 1, mt.color.g, mt.color.b, mt.color.a - 4); // Interesting effect
        mt.color = new Color(mt.color.r, mt.color.g, mt.color.b, mt.color.a - 0.25f / radius);
        if (mt.color.a <= 0)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == EnemyManagerPro.enemyLayer && EnemyManagerPro.checking(other.gameObject.GetComponent<Enemy>()))
        {
            if (other.gameObject.GetComponent<Enemy>().GetHealthRatio() < 1)
                other.gameObject.GetComponent<Enemy>().ApplyHeal((int)healPower);
        }
    }
}
