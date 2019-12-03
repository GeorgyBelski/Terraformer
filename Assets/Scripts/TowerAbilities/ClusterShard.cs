using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterShard : MonoBehaviour
{
    public GameObject clasterBlowUp;
    public GameObject calsterPuddle;
    public PlasmaTower thisTower;
    private int blowUpDamage;
    private float blowUpSize;

    private float puddleSize;
    private float puddleTime;

    public int mainDamage;
    public float speed;

    public PlasmaBlowUp blow;
    private ClusterPuddle puddle;
    //private Enemy target;

    private float time = 0;
    private float startPos;
    private Vector3 launchPoint;
    private Vector3 velocity;
    bool isGrounded = false;
    //private float time = 0;


    void Start()
    {
        startPos = transform.position.y;
        CreateBlowUp();
    }

    void CreateBlowUp() 
    {
        blow = Instantiate(clasterBlowUp, transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<PlasmaBlowUp>();
        blow.gameObject.SetActive(false);
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
        if (isGrounded || transform.position.y <= 0) 
        {
            BlowUp();
            isGrounded = false;
        }

    }
    void BlowUp() 
    {
        blow.SetSettings(blowUpDamage, blowUpSize);
        blow.thisTower = thisTower;
        blow.transform.position = this.transform.position + Vector3.up* 0.3f;
        blow.gameObject.SetActive(true);
        puddle = Instantiate(calsterPuddle, new Vector3(transform.position.x, -0.03f, transform.position.z), new Quaternion(0, 0, 0, 0)).GetComponent<ClusterPuddle>();
        puddle.setSetings(puddleTime, puddleSize);
        // Destroy(gameObject);
        if (thisTower)
        { this.gameObject.SetActive(false); }
        else 
        {
            DestroyShard();
        }
    }
    public void DestroyShard() 
    {
        if (blow && blow.gameObject.activeSelf)
        { blow.thisTower = null; }
        else if(blow)
        { Destroy(blow.gameObject); }
        Destroy(gameObject);
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
        this.gameObject.SetActive(true);
        transform.position = thisTower.gunpoint.position;
        time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Globals.groundMask)
        {

            isGrounded = true;
        }
    }
}
