using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerDamager : Enemy_Logic
{
    public int damageOnAttack;
    public GameObject powerCristall;
    public Image chargeBar;

    public float jumpDistance;
    public float chargingTime;
    public SpurtFXController SpurtFXController;

    private float realChargingTime;
    private bool isChargingReady = false;
    private bool isHealingJump = false;
    private Material cristalMaterial;

    private Vector3 dest;

    public override void Attack()
    {
        targetTower.towerHealth.ApplyDamage(damageOnAttack, Vector3.zero, Vector3.zero);
    }

    // Start is called before the first frame update
    void Start()
    {
        cristalMaterial = powerCristall.GetComponent<Renderer>().material;
        //readyToJump();
        realChargingTime = chargingTime;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        realChargingTime += Time.deltaTime;
        if (!isChargingReady)
        {
            if (realChargingTime < chargingTime)
            {
                charging();             
            }
            else
            {
                readyToJump();
                //print("=");
            } 
        }

        base.Update();
    }

    void charging()
    {
        //print("+");
        chargeBar.fillAmount = realChargingTime / chargingTime;
        
        //cristalMaterial.color = new Color(255 - (100 / chargingTime * realChargingTime), 0 + (100 / chargingTime * realChargingTime), 0);
    }

    void readyToJump()
    {
        isChargingReady = true;
    }

    void jump(Vector3 jumpPos)
    {
        SpurtFXController.ShowSpurtWave(dest);
        transform.position = dest;
        isChargingReady = false;
        realChargingTime = 0;
        charging();
    }

    public override void check()
    {
        if (isGoingToDest && isHealingJump)
            isHealingJump = false;

        if (isChargingReady)
        {


            if (isGoingToDest)
            {
                dest = emk.GetDest();
                if (Vector3.Distance(emk.GetDest(), transform.position) < jumpDistance)
                    jump(dest);
            }
            if (isGiveUp && !isHealingJump)
            {
                isHealingJump = true;
                transform.LookAt(emk.GetDest());
                //print(transform.eulerAngles.x + "; " + transform.eulerAngles.z);
                dest = new Vector3(transform.position.x + Mathf.Sin(Sqad.DegreeToRadian(transform.eulerAngles.y)) * jumpDistance, transform.position.y, transform.position.z + Mathf.Cos(Sqad.DegreeToRadian(transform.eulerAngles.y)) * jumpDistance);
                jump(dest);
            }
        }
           
        base.check();
    }
}
