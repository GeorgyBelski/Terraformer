using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterShard : MonoBehaviour
{
    public GameObject clasterBlowUp;
    public GameObject calsterPuddle;
    private int blowUpDamage;
    private float blowUpSize;

    private float puddleSize;
    private float puddleTime;

    public int mainDamage;
    public float speed;

    private PlazmaBlowUp blow;
    private ClusterPuddle puddle;
    //private Enemy target;

    private float time = 0;
    private float startPos;
    private Vector3 launchPoint;
    private Vector3 velocity;
    //private float time = 0;


    void Start()
    {
        startPos = transform.position.y;
    }

    void Update()
    {
        time += Time.deltaTime;
        Vector3 p = launchPoint + velocity * time;
        p.y -= 0.5f * 9.81f * time * time;
        //p.x = px - p.x / 100; 
        transform.localPosition = p;
        //time += Time.deltaTime;
        //if()
        //transform.position += new Vector3(transform.forward.x, startPos - (time * speed - (9.81f * (time * time))/2) - startPos, transform.forward.z) * speed * Time.deltaTime;
        //transform.position += transform.forward * speed * Time.deltaTime;
        if (transform.position.y <= 0.4)
        {

            blow = Instantiate(clasterBlowUp, transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<PlazmaBlowUp>();
            blow.setSetings(blowUpDamage, blowUpSize);
            puddle = Instantiate(calsterPuddle, new Vector3(transform.position.x, -0.03f, transform.position.z), new Quaternion(0, 0, 0, 0)).GetComponent<ClusterPuddle>();
            puddle.setSetings(puddleTime, puddleSize);
            Destroy(gameObject);
        }

    }

    public void setSettings(int mainDamage, float speed, int blowUpDamage, float blowUpSize, float puddleSize, float puddleTime, Vector3 launchPoint, Vector3 velocity)
    {
        this.mainDamage = mainDamage;
        this.speed = speed;
        this.blowUpDamage = blowUpDamage;
        this.blowUpSize = blowUpSize;
        this.puddleSize = puddleSize;
        this.puddleTime = puddleTime;
        this.launchPoint = launchPoint;
        this.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.layer == 12)
        {
            other.GetComponent<Enemy>().ApplyDamage(mainDamage, transform.position, Vector3.zero);
            //Debug.Log("entered");
        }
    }
}
