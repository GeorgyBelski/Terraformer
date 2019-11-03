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

    private Vector2 dir;

    private float shootPosX;
    private float shootPosY;
    private float g = 9.81f;
    //private float s = 20f;
    private float s2;

    private float tanTheta;
    private float cosTheta;
    private float sinTheta;

    void Start()
    {
        s2 = plazmaBuletSpeed * plazmaBuletSpeed;
        dir = new Vector2();
        base.Start();
        type = TowerType.Plazma;
        //print(TowerManager.availablePlazmaTowers);
    }
    public override void TowerAttack(Enemy target)
    {
        if (target)
        {
            /////////Debug for Drawing Trajectory
            float realSpeed = plazmaBuletSpeed;
            Vector3 prev = gunpoint.transform.position, next;
            for (int i = 1; i <= 20; i++)
            {
                
                float t = i / 10f;
                //realSpeed -= (0.5f * 0.002f * realSpeed * realSpeed);
                float dx = realSpeed * cosTheta * t;
                float dy = realSpeed * sinTheta * t - 0.5f * g * t * t;
                next = gunpoint.transform.position + new Vector3(dir.x * dx, dy, dir.y * dx);
                Debug.DrawLine(prev, next, Color.blue, 5, true);
                prev = next;
                // print(prev + "+" + next);
            }
            ///////////////////////////////

            //LookAtTarger();
            bullet = Instantiate(plazmaBullet, gunpoint.position, gunpoint.rotation).GetComponent<PlazmaBullet>();
            bullet.setSettings(directShotAttack, plazmaBuletSpeed, blastShotAttack, damageRadius, target, gunpoint.transform.position, new Vector3(plazmaBuletSpeed * cosTheta * dir.x, plazmaBuletSpeed * sinTheta, plazmaBuletSpeed *cosTheta * dir.y));
        }
    }

    //private Vector3 prevPos;
    //private float prevTime = 0;

    private void findeTrajectory()
    {
       
        //Rigidbody targetRigidbody;
        Vector3 endPosition = target.transform.position;
        /*
        if (prevPos == null)
            prevPos = endPosition;

        if (prevTime == 0)
            prevTime = Time.deltaTime;

        print((prevPos / Time.deltaTime - endPosition / Time.deltaTime).magnitude);
        prevPos = endPosition;
        prevTime = Time.deltaTime;
        */
        switch(target.type)   
        {
            case EnemyType.Tank:
                //EnemyMouseController emk = target.GetComponent<EnemyMouseController>();
                //print(targetRigidbody.velocity.magnitude);
                //print(Vector3.Distance(transform.position, target.transform.position) / base.range);
                endPosition += target.transform.forward * 2 * plazmaBuletSpeed / g * 0.5f * 2.5f * Vector3.Distance(transform.position, target.transform.position) / base.range;
                
            break;
            case EnemyType.Solder:
                endPosition += target.transform.forward * 6f * (Vector3.Distance(transform.position, target.transform.position) / base.range - 0.2f);// * plazmaBuletSpeed / g * 2.5f * (Vector3.Distance(transform.position, target.transform.position) / base.range - 0.2f);
            break;
            //endPosition += target.transform.forward * emk.agent.speed;//((Vector3.Distance(transform.position, target.transform.position)) / base.range) * 6 * emk.agent.speed;
        }
        dir.x = endPosition.x - gunpoint.transform.position.x;
        dir.y = endPosition.z - gunpoint.transform.position.z;
        shootPosX = dir.magnitude;
        shootPosY = -gunpoint.transform.position.y;
        dir /= shootPosX;
        float r = s2 * s2 - g * (g * shootPosX * shootPosX + 2f * shootPosY * s2);
        tanTheta = (s2 - Mathf.Sqrt(r)) / (g * shootPosX);
        cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        sinTheta = cosTheta * tanTheta;
    }

    protected override void LookAtTarger()
    {
        //print("+");
        if (target)
        {
            
            cannon.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));
            //cannon.LookAt(target.GetPosition());
        }
    }

    public override void EndCasting()
    {
        IsCastingAbility = false;
    }

    internal override void TowerUpdate()
    {
        if (target)
        {
            findeTrajectory();
        }
        //throw new System.NotImplementedException();
    }

    public void CastClusterBomb(Vector3 aimPosition)
    {

    }

    public override void ActivateSymbiosisUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void DisableSymbiosisUpgrade()
    {
        throw new System.NotImplementedException();
    }
}
