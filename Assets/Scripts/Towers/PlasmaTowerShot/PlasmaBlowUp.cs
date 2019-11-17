using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBlowUp : MonoBehaviour
{
    //Material mt;
    public float damage;
    public float radius;
    public PlasmaTower thisTower;
    public Material material;
    int randomizer;
    // Start is called before the first frame update
    void Start()
    {
        randomizer = Random.Range(0, 100);
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
        other.TryGetComponent<Enemy>(out Enemy enemy);

        if (enemy)
        {
            //if (other.gameObject.GetComponent<Enemy>().GetHealthRatio() < 1)
            other.gameObject.GetComponent<Enemy>().ApplyDamage((int)damage, this.transform.position, Vector3.zero);
            if (!thisTower) 
            { return; }
            if (thisTower.symbiosisTowerType == TowerType.Laser)
            {
                enemy.effectsController.AddBurning(BurningEffect.standardLifetime/2, ((LaserTower)thisTower.symbiosisTower).damageBurning);
            }
            else if (thisTower.symbiosisTowerType == TowerType.Electro)
            {
                ElectroTower elTower = (ElectroTower)thisTower.symbiosisTower;
                if (randomizer <= elTower.probabilityOfStan)
                {
                    enemy.effectsController.AddStun(elTower.stunDuration / 2);
                }
            }
        }
    }

    public void SetSettings(float damage, float radius)
    {
        this.damage = damage;
        this.radius = radius;
        if (!thisTower) 
        { return; }
        material = GetComponent<MeshRenderer>().materials[0];
        
        if (thisTower.symbiosisTowerType == TowerType.Laser)
        {
            material.SetColor("_EmissionColor", thisTower.laserSymbTrailColor);
        }
        else if (thisTower.symbiosisTowerType == TowerType.Electro)
        {
            material.SetColor("_EmissionColor", thisTower.electroSymbTrailColor);
        }
        else if (thisTower.symbiosisTowerType == TowerType.Plasma)
        {
            material.SetColor("_EmissionColor", thisTower.plasmaSymbTrailColor);
        }
    }
}
