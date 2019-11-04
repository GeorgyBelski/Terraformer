using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaClusterBombAbility : TowerAbility
{
    //Timeless//Timeless//Timeless//Timeless//Timeless//Timeless//Timeless//Timeless//
    public float plazmaBuletSpeed = 50f;
    public int directShotAttack = 150;

    public int blastShotAttack = 100;
    public float damageRadius = 1.5f;

    public float puddleRadius = 7;
    public float puddleTime = 10;
    //Timeless//Timeless//Timeless//Timeless//Timeless//Timeless//Timeless//Timeless//

    Vector3? aim;
    public GameObject bomb;
    public int bombCount = 10;

    private bool isCusting = false;
    private int shootingCount;
    private float rateOfShooting;
    private float realRate;
    private ClusterShard bullet;

    private Vector2 dir;

    private float shootPosX;
    private float shootPosY;
    private float g = 9.81f;

    private float speed;
    private float s2;

    private float tanTheta;
    private float cosTheta;
    private float sinTheta;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        rateOfShooting = castTime / bombCount;
        aim = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCusting && shootingCount > 0)
        {
            //print("+");
            realRate -= Time.deltaTime;
            if (realRate < 0)
            {
                bullet = Instantiate(bomb, transform.position, transform.rotation).GetComponent<ClusterShard>();
                
                bullet.setSettings(directShotAttack, speed, blastShotAttack, damageRadius, puddleTime, puddleRadius, gunpoint.transform.position, new Vector3(speed * cosTheta * dir.x + Random.Range(-2f, 2f), speed * sinTheta + Random.Range(-2f, 2f), speed * cosTheta * dir.y + Random.Range(-2f, 2f)));
                //bullet.setSettings(directShotAttack, speed, blastShotAttack, damageRadius, tower.target, gunpoint.transform.position, new Vector3(speed * Random.Range(0.6f, 1.6f) * cosTheta * dir.x, speed * Random.Range(0.6f, 1.6f) * sinTheta, speed * Random.Range(0.6f, 1.6f) * cosTheta * dir.y));
                shootingCount--;
                realRate = rateOfShooting;
            }
        }

        if (shootingCount <= 0)
        {
            isCusting = false;
            shootingCount = bombCount;
            tower.EndCasting();
        }
    }

    public new void Cast(Vector3 aimPosition)
    {

        base.Cast(aimPosition);
        TowerManager.availablePlazmaTowers.Remove((PlazmaTower)tower);
        aim = aimPosition;

        speed = Vector3.Distance(aimPosition, gunpoint.position)/1.2f;
        print(speed);
        s2 = speed * speed;

        //tower.//RotateCannon((Vector3)aim);

        dir.x = aimPosition.x - gunpoint.transform.position.x;
        dir.y = aimPosition.z - gunpoint.transform.position.z;
        shootPosX = dir.magnitude;
        shootPosY = -gunpoint.transform.position.y;
        dir /= shootPosX;
        float r = s2 * s2 - g * (g * shootPosX * shootPosX + 2f * shootPosY * s2);
        tanTheta = (s2 + Mathf.Sqrt(r)) / (g * shootPosX);
        cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        sinTheta = cosTheta * tanTheta;

        cannon.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));

        //shootingCount = bombCount;
        realRate = rateOfShooting;
        isCusting = true;
    }

    public override void CastingControl()
    {
        throw new System.NotImplementedException();
    }

    public override void ShootingControl()
    {
        throw new System.NotImplementedException();
    }
}
