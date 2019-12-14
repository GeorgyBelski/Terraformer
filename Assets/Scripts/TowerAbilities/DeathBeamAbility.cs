using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBeamAbility : TowerAbility
{
    [Header("DeathBeam")]
    public Material beamMaterial;
    Material originalMaterial;
    Enemy target;
    public int costPerPeriod = 7;
    bool isActive = false;
    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        CastingControl();
    }
    public void Cast(Enemy target)
    {
        if (!target) { return; }

        isActive = true;
        this.target = target;
        tower.enableAutoattacs = false;
        tower.IsCastingAbility = true;
        timerCast = castTime;
        TowerManager.availableLaserTowers.Remove((LaserTower)tower);
        ((LaserTower)tower).lr.material = beamMaterial;
        ((LaserTower)tower).lr.enabled = true;
        ((LaserTower)tower).lr.widthMultiplier = 1;
        ((LaserTower)tower).lr.startWidth = 0.5f;
    //   Debug.Log("Cast lr.widthCurve.keys[0]: " + ((LaserTower)tower).lr.widthCurve.keys[0].value);
    }
    public override void CastingControl()
    {
        if (!target) 
        { 
            CancelBeam(); 
            return; 
        }

        tower.RotateCannon(target.GetPosition());
        ShootingControl();
        timerCast -= Time.deltaTime;
        if (timerCast <= 0)
        {
            if (target.health > 0 && ResourceManager.resource - costPerPeriod >= 0) 
            {
                timerCast = castTime;
                target.ApplyDamage(damage, Vector3.zero, Vector3.zero);
                ResourceManager.RemoveResource(costPerPeriod);
            }
            else
            {
                CancelBeam();
            }     
        }
    }

    void CancelBeam() 
    {
        if (isActive) 
        {
            isActive = false;
            tower.EndCasting();
            tower.enableAutoattacs = true;
            target = null;
            ((LaserTower)tower).lr.material = ((LaserTower)tower).lrMaterial;
            ((LaserTower)tower).lr.startWidth = ((LaserTower)tower).lrWidthKeys[0];
        //    Debug.Log("Cancel lr.widthCurve.keys[0]: " + ((LaserTower)tower).lr.widthCurve.keys[0].value);
        //    Debug.Log("Cancel lrWidthKeys[0]: " + ((LaserTower)tower).lrWidthKeys[0]);
            //  ((LaserTower)tower).lr.enabled = false;
        }
    }

    public override void ShootingControl()
    {
        ((LaserTower)tower).lr.SetPosition(0, gunpoint.position);
        ((LaserTower)tower).lr.SetPosition(1, target.GetPosition());

    }


}
