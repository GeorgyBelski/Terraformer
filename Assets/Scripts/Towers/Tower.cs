using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CreepHexagonGenerator;

public enum TowerType { Electro, Laser, Terraformer, Plasma };

public abstract class Tower : MonoBehaviour
{
    
    public TowerType type;

    [Header("Main Attributes")]
    public bool enableAutoattacs = true;
    public int range = 8;
    int previousRange;
    public Color rangeColor;
    public Material gizmoMaterial;
    public Material rangeLineMaterial;
  //  [HideInInspector]
    public bool isHighlighted, isSelected;
    bool castingAbility;
    public bool IsCastingAbility { get => castingAbility; set => castingAbility = value; }
    

    public TargetingType targetingType = TargetingType.Nearest;
    public static int basicSupply = 1;
    public int supply; // cost of autoAttacs per second;

    [Header("Cooldowns")]
    public float cooldownAttack = 1f;
    protected float ordinaryCooldownAttack;
    public float timerAttack = 0f;

    [Header("References")]
    public Transform cannon;
    public Transform gunpoint;
    public TowerHealth towerHealth;
    [HideInInspector]
    public Enemy target;
    public SymbiosisVisualLink visualLinkPrefab;
    [HideInInspector]
    public SymbiosisVisualLink currentVisualLink;

    [Header("Symbiosis")]
    public Tower symbiosisTower;
    public TowerMenuController towerMenuController;
    public bool isSymbiosisInstalled =false;
    public TowerType? symbiosisTowerType = null;

    public Hexagon hexagon;

    int targetIndex = -1;
    LineRenderer rangeline;
    Vector3 previousPosition;
    [HideInInspector]
    public Material towerMaterial;
    Color highlightedColor;
    protected int randomizer;

    protected void Start()
    {
        TowerManager.AddTower(this);
        TowerManager.transformTowerMap.Add(this.transform, this);
        rangeline = gameObject.GetComponent<LineRenderer>();
        ordinaryCooldownAttack = cooldownAttack;
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
        ResourceManager.isTowersSupplyChanged = true;
        
    }

    void Update()
    {
        if (!IsCastingAbility) { 
            target = ChooseTarget();
            LookAtTarger();
            Shooting();
        }
        if (enableAutoattacs)
        {
            TowerUpdate();
            ReduceTimers();
            HighlightTower();
            ShowRange();
        }
        if (hexagon == null) { hexagon = GetHexagon();}
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
    Hexagon GetHexagon()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.3f, Vector3.down, out RaycastHit hit, 2f, CreepHexagonGenerator.creepLayerMask))
        {
            GameObject hexagonGameObject = hit.collider.gameObject;
            if (CreepHexagonGenerator.meshHexagonMap.TryGetValue(hexagonGameObject, out hexagon))
            {
                hexagon.SetStatus(HexCoordinatStatus.Occupied);
                return hexagon;
            }
        }
        return null;
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

    protected virtual void LookAtTarger() {
     //   if (cannon) { 
            if (target)
            {
                cannon.LookAt(target.GetPosition());
            }
    //    }
    }
    void Shooting() {
        if (timerAttack <= 0 && target) {
            TowerAttack(target);
            timerAttack = cooldownAttack;
        }
    }
    public void RotateCannon(Vector3 targetPosition)
    {
        cannon.LookAt(targetPosition);
    }
    public abstract void TowerAttack(Enemy target);

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

    public virtual void EndCasting() { timerAttack = cooldownAttack; }

    void HighlightTower()
    {
        if (isHighlighted && towerMaterial.GetFloat("_Float_Highlight") != 1)
        {
            towerMaterial.SetFloat("_Float_Highlight", 1f);
        }
        else if (!isHighlighted && towerMaterial.GetFloat("_Float_Highlight") != 0)
        {
            towerMaterial.SetFloat("_Float_Highlight", 0);
        }
    }

    public void SetSymbiosis(Tower partner)
    {
        if (!currentVisualLink)
        {
            currentVisualLink = Instantiate(visualLinkPrefab);
        }
        currentVisualLink.gameObject.SetActive(true);
        currentVisualLink.SetEndPoints(cannon.position, partner.cannon.position);
        currentVisualLink.SetEndColors(partner.rangeColor, this.rangeColor);
        symbiosisTower = partner;
        partner.symbiosisTower = this;
        towerMenuController.StartInstallingSymbiosis();
    }

    public void BreakSymbiosis()
    {
        if (currentVisualLink && currentVisualLink.gameObject.activeSelf)
        { currentVisualLink.BreakVisualLink(); }

        if (symbiosisTower)
        {
            if (symbiosisTower.currentVisualLink  && symbiosisTower.currentVisualLink.gameObject.activeSelf)
            { symbiosisTower.currentVisualLink.BreakVisualLink(); }

            isSymbiosisInstalled = false;
            symbiosisTower.isSymbiosisInstalled = false;

            symbiosisTower.towerMenuController.ResetSymbiosisCircleBar().ResetSymbiosisTimers();
            towerMenuController.ResetSymbiosisCircleBar().ResetSymbiosisTimers();

            TowerManager.symbiosisTowers.Remove(this);
            TowerManager.symbiosisTowers.Remove(symbiosisTower);

            DisableSymbiosisUpgrade();
            symbiosisTower.DisableSymbiosisUpgrade();

            towerMaterial.SetFloat("_Float_Symbiosis", 0);
            symbiosisTower.towerMaterial.SetFloat("_Float_Symbiosis", 0);


            symbiosisTower.symbiosisTower = null;
            symbiosisTower = null;

            ResourceManager.isTowersSupplyChanged = true;
            towerMaterial.SetFloat("_Float_Highlight", 0);

            
        }
        
    }

    public abstract void ActivateSymbiosisUpgrade();
    public abstract void DisableSymbiosisUpgrade();

}
