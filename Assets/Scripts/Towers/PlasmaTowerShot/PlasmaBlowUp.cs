using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBlowUp : MonoBehaviour
{
    //Material mt;
    public float damage;
    public float centerDamage = 50f;
    public float radius;
    float startRadius;
    public PlasmaTower thisTower;
    public Material material;
    public Vector3 position;
    int randomizer;
    // Start is called before the first frame update
    void Start()
    {
        startRadius = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        position = this.transform.position;
        //print("+");
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 10, transform.localScale.y + Time.deltaTime *0.8f, transform.localScale.z + Time.deltaTime * 10);
        if (transform.localScale.x >= radius)
        {
            //  Destroy(gameObject);
            this.gameObject.SetActive(false);
            
        }
        
        //mt.color = new Color(mt.color.r, mt.color.g, mt.color.b, mt.color.a - 0.25f / radius);
    }
    private void LateUpdate()
    {
        if (!thisTower) { Destroy(this.gameObject); }
    }
    /*
    private void OnDrawGizmos()
    {
        DrawCube();
    }
    void DrawCube()
    {
        if(isDraw)
        Gizmos.DrawCube(enemyPosition, Vector3.one);
    }
    */
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
                enemy.effectsController.AddBurning(BurningEffect.standardLifetime / 2, ((LaserTower)thisTower.symbiosisTower).damageBurning);
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
        this.damage = damage;
        this.radius = radius;
        this.transform.localScale = Vector3.one * startRadius;
        if (!thisTower) 
        { return; }
        material = GetComponent<MeshRenderer>().materials[0];
     /*   
        if (thisTower.symbiosisTowerType == TowerType.Laser)
        {
            SetColor(thisTower.laserSymbColor2);
        }
        else if (thisTower.symbiosisTowerType == TowerType.Electro)
        {
            SetColor(thisTower.electroSymbColor2);
        }
        else if (thisTower.symbiosisTowerType == TowerType.Plasma)
        {
            SetColor(thisTower.plasmaSymbTrailColor);
        }
        */
    }

    public void SetColor(Color color) 
    {
        material.SetColor("_EmissionColor", color);
    }
}
