using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlowUpCast : MonoBehaviour
{
    private float burningDamage;

    private float radius;
    private float damage;

    private float blowUpsize;
    private float blowUpDamage;

    private bool isReal = false;

    public GameObject blowUpPrefab;
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf) { 
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 5, transform.localScale.y, transform.localScale.z + Time.deltaTime * 5);
            if (transform.localScale.x >= radius)
            {
                if (isReal)
                {
                    transform.localScale = new Vector3(0, transform.localScale.y, 0);
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            print("+");
            Enemy en = other.GetComponent<Enemy>();
            if (!isReal)
            {
                en.ApplyDamage((int)damage, transform.position, Vector3.zero);
                en.effectsController.AddBurning(5, (int)burningDamage);
                return;
            }

            if (en.effectsController.effects.ContainsKey(Effect.Type.Burning) && isReal)
            {
                en.ApplyDamage((int)damage, transform.position, Vector3.zero);
                LaserBlowUpCast blowUp = Instantiate(blowUpPrefab, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), transform.rotation).GetComponent<LaserBlowUpCast>();
                blowUp.Set(blowUpsize, blowUpDamage, burningDamage);
            }
            //other.GetComponent<Enemy>().ApplyDamage(mainDamage, transform.position, Vector3.zero);
            //Debug.Log("entered");
        }
    }

    public void Set(float radius, float damage, float bloblowUpsize, float blowUpDamage, float burningDamage)
    {
        this.radius = radius;
        this.damage = damage;
        this.blowUpsize = bloblowUpsize;
        this.blowUpDamage = blowUpDamage;
        this.burningDamage = burningDamage;
        this.isReal = true;

    }

    public void Set(float radius, float damage, float burningDamage)
    {
        this.radius = radius;
        this.damage = damage;
        this.burningDamage = burningDamage;
        this.isReal = false;
    }
}
