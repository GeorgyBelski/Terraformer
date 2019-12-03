using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBullet : MonoBehaviour
{
  //  public GameObject plazmaBlowUp;
    private int blowUpDamage;
    private float blowUpSize;

    public int mainDamage;
    public float speed;

    public PlasmaTower thisTower;

    public PlasmaBlowUp blow;
    private Enemy target;

    private float time = 0;
    private float startPos;
    private Vector3 launchPoint;
    private Vector3 velocity;
    public Material trailmaterial;
    public TrailRenderer trailRenderer;
    [ColorUsageAttribute(true, true)]
    public Color OrdinaryTrailColor;
    //private float time = 0;


    void Start()
    {
        startPos = transform.position.y;
        trailRenderer = GetComponent<TrailRenderer>();

        /*
        blow = Instantiate(plazmaBlowUp, transform.position, plazmaBlowUp.transform.rotation).GetComponent<PlasmaBlowUp>();
        blow.thisTower = thisTower;
        
        blow.gameObject.SetActive(false);
        */
    }

    void FixedUpdate()
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
    
    }
    private void LateUpdate()
    {
        if (!thisTower && !this.gameObject.activeSelf) { DestroyBullet(); }
    }
    public void setSettings(int mainDamage, float speed, int blowUpDamage, float blowUpSize, Enemy target, Vector3 launchPoint, Vector3 velocity, PlasmaBlowUp blow)
    {
        this.mainDamage = mainDamage;
        this.speed = speed;
        this.blowUpDamage = blowUpDamage;
        this.blowUpSize = blowUpSize;
        this.target = target;
        this.launchPoint = launchPoint;
        this.transform.localPosition = launchPoint;
        this.velocity = velocity;
        this.blow = blow;
        this.time = 0;
        this.gameObject.SetActive(true);
    }
    public void SetSimbiosisColor() 
    {
        if (thisTower.symbiosisTowerType == TowerType.Laser)
        {
            SetTrailColor(thisTower.laserSymbTrailColor);
        }
        else if (thisTower.symbiosisTowerType == TowerType.Electro)
        {
            SetTrailColor(thisTower.electroSymbTrailColor);
        }
        else if (thisTower.symbiosisTowerType == TowerType.Plasma)
        {
            SetTrailColor(thisTower.plasmaSymbTrailColor);
        }
    }
    public void SetTrailColor(Color color)
    {
        trailRenderer.materials[0].SetColor("_EmissionColor", color);
    }
    void BlowUp() 
    {
        if (blow) 
        { 
            blow.gameObject.SetActive(true);
            blow.SetSettings(blowUpDamage, blowUpSize);
            blow.transform.position = this.transform.position;
        }
        //  Destroy(gameObject);
        if (thisTower)
        { this.gameObject.SetActive(false); }
        else
        { DestroyBullet(); }
    }

    public void DestroyBullet() 
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Globals.groundLayer)
        {
            BlowUp();
        }
    }
 }

