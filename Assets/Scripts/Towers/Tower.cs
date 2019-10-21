using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TowerType { Electro, Laser, Terraformerw, Plazma };

public abstract class Tower : MonoBehaviour
{
    
    public TowerType type;

    [Header("Main Attributes")]
    public int range = 8;
    int previousRange;
    public Color rangeColor;
    public Material gizmoMaterial;
    public Material rangeLineMaterial;
    public bool isHighlighted;
    public bool isSelected;
    bool castingAbility;
    public bool IsCastingAbility { get => castingAbility; set => castingAbility = value; }
    public TargetingType targetingType = TargetingType.Nearest;

    [Header("Cooldowns")]
    public float cooldownAttack = 1f;
    public float timerAttack = 0f;
    /*
    public float cooldownAbility1 = 10f;
    protected float timerAbility1 = 0f;

    public float cooldownAbility2 = 15f;
    protected float timerAbility2 = 0f;
    */
    [Header("References")]
    public Transform cannon;
    public Transform gunpoint;
    public TowerHealth towerHealth;
    public Enemy target;

    [Header("Symbiosis")]
    public Tower symbiosisTower;
    public TowerMenuController towerMenuController;
    public bool isSymbiosisInstalled =false;

    int targetIndex = -1;
    LineRenderer rangeline;
    Vector3 previousPosition;
    Material towerMaterial;
    Color highlightedColor;

    protected void Start()
    {
        TowerManager.AddTower(this);
        TowerManager.transformTowerMap.Add(this.transform, this);
        rangeline = gameObject.GetComponent<LineRenderer>();
        if (!rangeline) { 
            rangeline = gameObject.AddComponent<LineRenderer>();
        }
        rangeline.positionCount = 72;
        rangeline.material = rangeLineMaterial;
        rangeline.material.SetColor("_BaseColor", rangeColor);
        rangeline.textureMode = LineTextureMode.RepeatPerSegment;
        rangeline.widthMultiplier = 0.05f;
        rangeline.loop = true;

        towerMaterial = GetComponent<MeshRenderer>().material;
        highlightedColor = new Color(1, 1, .5f);
    }

    void Update()
    {
        if (!IsCastingAbility) { 
            target = ChooseTarget();
            LookAtTarger();
            Shooting();
        }

        TowerUpdate();
        ReduceTimers();
        HighlightTower();
        ShowRange();
    }
    internal abstract void TowerUpdate();

    void ReduceTimers() {
        if (timerAttack > 0)
        {
            timerAttack -= Time.deltaTime;
        }
        else {
            timerAttack = 0;
        }
    }
    Enemy ChooseTarget() {
        if (targetingType == TargetingType.Nearest)
        {
            return ChooseNearest(EnemyManagerPro.enemies);
        }
        else if (targetingType == TargetingType.MostVurnerable)
        {
            return ChooseMostVurnerable(GetEnemiesInRange());
        }
        else {
            return ChooseMostHardy(GetEnemiesInRange());
        }
    }

    List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemies = EnemyManagerPro.enemies;
        List<Enemy> enemiesInRange = new List<Enemy>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if ((enemies[i].GetPosition() - this.transform.position).magnitude < range)
            {
                enemiesInRange.Add(enemies[i]);
            }
        }
        return enemiesInRange;
    }

    Enemy ChooseNearest(List<Enemy> enemies)
    {
        float distanceMin = range;
        float distanceToTarget = distanceMin;
        Enemy newTarget = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                return null;
            }
            distanceToTarget = (enemies[i].GetPosition() - this.transform.position).magnitude;
            if (distanceToTarget < distanceMin)
            {
                newTarget = enemies[i];
                distanceMin = distanceToTarget;
            }
        }
        if (!newTarget){ return null;}
        else { return newTarget;}
    }
    
    Enemy ChooseMostVurnerable(List<Enemy> enemies)
    {
        float ratioMin = 99f;
        Enemy newTarget = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                return null;
            }
            if (enemies[i].healthRatio < ratioMin)
            {
                newTarget = enemies[i];
                ratioMin = enemies[i].healthRatio;
            }
        }
        if (!newTarget) { return null; }
        else { return newTarget; }
    }
    Enemy ChooseMostHardy(List<Enemy> enemies)
    {
        int healthMax = 0;
        Enemy newTarget = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                return null;
            }
            if (enemies[i].maxHealth > healthMax)
            {
                newTarget = enemies[i];
                healthMax = enemies[i].maxHealth;
            }
        }
        if (!newTarget) { return null; }
        else { return newTarget; }
    }

    virtual public void LookAtTarger() {
     //   if (cannon) { 
            if (target)
            {
                cannon.LookAt(target.GetPosition());
            }
    //    }
    }
    void Shooting() {
        if (timerAttack <= 0) {
            TowerAttack(target);
            timerAttack = cooldownAttack;
        }
    }
    public void RotateCannon(Vector3 targetPosition)
    {
        cannon.LookAt(targetPosition);
    }
    public abstract void TowerAttack(Enemy target);
/*
    private void OnDrawGizmos()
    {
        // link to enemy
        for (int i = 0; i < EnemyManagerPro.enemies.Count; i++)
        {
            if (i == targetIndex)
            {
                Gizmos.color = new Color(1, 0.4f, 0.3f);
             //   if (EnemyManagerPro.enemies[i]) {
                Gizmos.DrawLine(EnemyManagerPro.enemies[i].GetPosition(), this.transform.position);
            //    }
            }
        //    else { Gizmos.color = Color.gray;}   
        }

    }*/
    void ShowRange()
    {
        if (previousRange != range || previousPosition != transform.position)
        {
            Vector3 compass = range * Vector3.forward;
            for (int i = 0; i < 72; i++)
            {
                Vector3 circlPoint = transform.position + compass;
                circlPoint.y += 0.1f;
                if (rangeline) rangeline.SetPosition(i, circlPoint);
                compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 10 degrees.
            }
            previousRange = range;
            previousPosition = transform.position;
        }

        if (isSelected)
        {
            rangeline.enabled = true;
        }
        else if (!isSelected)
        {
            rangeline.enabled = false;
        }

    }

    public abstract void EndCasting();

    void HighlightTower()
    {
        if (isHighlighted && towerMaterial.GetColor("Color_19495AAD") != highlightedColor)
        {
            towerMaterial.SetColor("Color_19495AAD", highlightedColor);
        }
        else if(!isHighlighted && towerMaterial.GetColor("Color_19495AAD") != Color.black)
        {
            towerMaterial.SetColor("Color_19495AAD", Color.black);
        }
    }

    public void SetSymbiosis(Tower partner)
    {
        symbiosisTower = partner;
        partner.symbiosisTower = this;
        towerMenuController.StartInstallingSymbiosis();
    }

    public void BreakSymbiosis()
    {
        if (symbiosisTower)
        {
            isSymbiosisInstalled = false;
            symbiosisTower.isSymbiosisInstalled = false;

            symbiosisTower.towerMenuController.ResetSymbiosisCircleBar().ResetSymbiosisTimers();
            towerMenuController.ResetSymbiosisCircleBar().ResetSymbiosisTimers();

            TowerManager.symbiosisTowers.Remove(this);
            TowerManager.symbiosisTowers.Remove(symbiosisTower);

            symbiosisTower.symbiosisTower = null;
            symbiosisTower = null;
        }
        
    }
}
