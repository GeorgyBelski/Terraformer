using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaTower : Tower
{
    [Header("PlazmaTower")]

    [Header("AutoAttack")]
    public float plazmaBuletSpeed = 50f;
    public int directShotAttack = 150;
    public int blastShotAttack = 100;
    public float damageRadius = 1.5f;

    //public float attackSpeed;

    //private float reloading;
    public GameObject plazmaBullet;

    private PlazmaBullet bullet;

    // Start is called before the first frame update
    void Start()
    {
        //timerAttack = cooldownAttack;
        base.Start();
        type = TowerType.Plazma;
    }
    public override void TowerAttack(Enemy target)
    {
        if (target)
        {
            //LookAtTarger();
            bullet = Instantiate(plazmaBullet, gunpoint.position, gunpoint.rotation).GetComponent<PlazmaBullet>();
            bullet.setSettings(directShotAttack, plazmaBuletSpeed, blastShotAttack, damageRadius, target);
        }
    }

    public override void LookAtTarger()
    {
        //print("+");
        if (target)
        {
            cannon.LookAt(new Vector3(target.transform.position.x, 0, target.transform.position.z));
        }
    }

    public override void EndCasting()
    {
        IsCastingAbility = false;
    }

    internal override void TowerUpdate()
    {
        //throw new System.NotImplementedException();
    }
}
