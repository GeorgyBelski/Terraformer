using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBlowUp : MonoBehaviour
{
    //Material mt;
    public float damage;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        //mt = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //print("+");
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 10, transform.localScale.y + Time.deltaTime *0.8f, transform.localScale.z + Time.deltaTime * 10);
        if (transform.localScale.x >= radius)
        {
            Destroy(gameObject);
        }
        //mt.color = new Color(mt.color.r, mt.color.g, mt.color.b, mt.color.a - 0.25f / radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == EnemyManagerPro.enemyLayer && EnemyManagerPro.checking(other.gameObject.GetComponent<Enemy>()))
        {
            //if (other.gameObject.GetComponent<Enemy>().GetHealthRatio() < 1)
                other.gameObject.GetComponent<Enemy>().ApplyDamage((int)damage, this.transform.position, Vector3.zero);
        }
    }

    public void setSetings(float damage, float radius)
    {
        this.damage = damage;
        this.radius = radius;
    }
}
