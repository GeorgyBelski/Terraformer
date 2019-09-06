using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType { Electro, Laser };

public abstract class Tower : MonoBehaviour
{
    
    public TowerType type;

    [Header("Main Attributes")]
    public int range = 8;
    public Color rangeColor;
    public Material gizmoMaterial;
    public Material rangelineMaterial;
    public bool isSelected;
    bool castingAbility;
    public bool IsCastingAbility { get => castingAbility; set => castingAbility = value; }

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
    int targetIndex = -1;
    LineRenderer rangeline;

    protected void Start()
    {
        TowerManager.AddTower(this);
        rangeline = gameObject.GetComponent<LineRenderer>();
        if (!rangeline) { 
            rangeline = gameObject.AddComponent<LineRenderer>();
        }
        rangeline.positionCount = 72;
        rangeline.material = rangelineMaterial;
        rangeline.textureMode = LineTextureMode.RepeatPerSegment;
        rangeline.widthMultiplier = 0.05f;
        rangeline.loop = true;
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
        
        float distanceToTarget = range;
        float distanceTmp = distanceToTarget;
        targetIndex = -1;
        for (int i = 0; i < EnemyManagerPro.enemies.Count; i++) {
            if (EnemyManagerPro.enemies[i] == null) {
                return null;
            }
            distanceTmp = (EnemyManagerPro.enemies[i].GetPosition() - this.transform.position).magnitude;
            if (distanceTmp < distanceToTarget)
            {
                targetIndex = i;
                distanceToTarget = distanceTmp;
            }
        }
        if (targetIndex == -1)
        {
            return null;
        }
        else {
            return EnemyManagerPro.enemies[targetIndex];
        }
            
    }
    void LookAtTarger() {
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

        //range
        Vector3 compass = range * Vector3.forward;
    //    GL.Begin(GL.LINES);
    //    if(gizmoMaterial)
    //        gizmoMaterial.SetPass(0);
    //    GL.Color(rangeColor);
        for (int i = 0; i < 72; i++)
        {
            Vector3 circlPoint = transform.position  + compass;
            circlPoint.y = 0.1f;
    //        GL.Vertex(circlPoint);
            if (rangeline) rangeline.SetPosition(i, circlPoint);
            compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 10 degrees.
        }
    //    GL.End();

    }

    public abstract void EndCasting();
}
