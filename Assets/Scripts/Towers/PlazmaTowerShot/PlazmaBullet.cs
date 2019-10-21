using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaBullet : MonoBehaviour
{
    public GameObject plazmaBlowUp;
    private int blowUpDamage;
    private float blowUpSize;

    public int mainDamage;
    public float speed;

    private PlazmaBlowUp blow;
    private Enemy target;

    private float time = 0;

    void Update()
    {
        //transform.position += new Vector3(Vector3.forward.x, transform.position.y - (time * speed - (9.81f * (time * time))/2), Vector3.forward.z) * speed * Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime;
        if (transform.position.y <= 0.4)
        {

            blow = Instantiate(plazmaBlowUp, transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<PlazmaBlowUp>();
            blow.setSetings(blowUpDamage, blowUpSize);
            Destroy(gameObject);
        }
    
    }

    public void setSettings(int mainDamage, float speed, int blowUpDamage, float blowUpSize, Enemy target)
    {
        this.mainDamage = mainDamage;
        this.speed = speed;
        this.blowUpDamage = blowUpDamage;
        this.blowUpSize = blowUpSize;
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
       

        if (other.gameObject.layer == 12)
        {
            target.ApplyDamage(mainDamage, transform.position, Vector3.zero);
            //Debug.Log("entered");
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {
        // print("+");
        if (collision.gameObject.layer == 9)
        {
            
            Instantiate(plazmaBlowUp, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }
    */
 }

