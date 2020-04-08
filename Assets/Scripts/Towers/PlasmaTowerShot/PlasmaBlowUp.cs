using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBlowUp : MonoBehaviour
{
    //Material mt;
    public float damage;
    public float centerDamage = 50f;
    public float radius;
    Vector3 startScale;
    public PlasmaTower thisTower;
    public bool isReadyToDestroy = false;
    bool isCountDown = false;
    public float timerToDestroy = 1.3f;
    public Material material;
    int randomizer;

    public GameObject blowUpSound;
    //public AudioSource audioSource;
    //public AudioClip blowUpSound; 

    // Start is called before the first frame update
    void Start()
    {
        //audioSource.pitch = Random.Range(1.5f, 1.9f);
        //audioSource.PlayOneShot(blowUpSound, 0.3f);

        startScale = this.transform.localScale;
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        //print("+");

        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 10, transform.localScale.y + Time.deltaTime * 0.8f, transform.localScale.z + Time.deltaTime * 10);
        if (transform.localScale.x >= radius)
        {
            if (thisTower)
            {
                
                this.gameObject.SetActive(false);
            }
            else
            {
                //this.enabled = false;
                Destroy(gameObject);
            }
        }

        //mt.color = new Color(mt.color.r, mt.color.g, mt.color.b, mt.color.a - 0.25f / radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Enemy>(out Enemy enemy);

        if (enemy)
        {
            //if (other.gameObject.GetComponent<Enemy>().GetHealthRatio() < 1)
            Vector3 fromBlowUpCenterToEnemy = this.transform.position - new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
           // Debug.Log("fromBlowUpCenterToEnemy " + fromBlowUpCenterToEnemy.magnitude);
            //isDraw = true;

            if (fromBlowUpCenterToEnemy.magnitude > 0.5f)
            { enemy.ApplyDamage((int)damage, this.transform.position, Vector3.zero); }
            else
            { enemy.ApplyDamage((int)centerDamage, this.transform.position, Vector3.zero); }
            if (!thisTower)
            { return; }
            if (thisTower.symbiosisTowerType == TowerType.Laser)
            {
                enemy.effectsController.AddBurning(Effect.burningDuration / 2, ((LaserTower)thisTower.symbiosisTower).damageBurning);
            }
            else if (thisTower.symbiosisTowerType == TowerType.Electro)
            {
                ElectroTower elTower = (ElectroTower)thisTower.symbiosisTower;
                randomizer = Random.Range(0, 100);
                if (randomizer <= elTower.probabilityOfStan)
                {
                    enemy.effectsController.AddStun(elTower.stunDuration / 2);
                }
            }
        }
      //  else { isDraw = false; }
    }

    public void SetSettings(float damage, float radius)
    {
        Instantiate(blowUpSound, transform.position, transform.rotation);
        this.damage = damage;
        this.radius = radius;
        this.transform.localScale = startScale;
        if (!thisTower) 
        { return; }
     //   material = GetComponent<MeshRenderer>().materials[0];

    }

    public void SetColor(Color color) 
    {
        if (material)
        { material.SetColor("_EmissionColor", color); }
        else { 
            material =  GetComponent<MeshRenderer>().material;
            material.SetColor("_EmissionColor", color); 
        }
    }

}
