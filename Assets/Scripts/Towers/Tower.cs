using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType { Electro, Laser };

public abstract class Tower : MonoBehaviour
{
    
    public TowerType type;

    [Header("Main Attributes")]
    public int range = 8;
    public Color color;
    public Material gizmoMaterial;
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
    public TowerHealth towerHealth;
    public Enemy target;
    int targetIndex = -1;

    void Start()
    {

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
        /*
        if (timerAbility1 > 0)
        {
            timerAbility1 -= Time.deltaTime;
        }
        else {
            timerAbility1 = 0;
        }
        if (timerAbility2 > 0)
        {
            timerAbility2 -= Time.deltaTime;
        }
        else {
            timerAbility2 = 0;
        }
        */
    }
    Enemy ChooseTarget() {
        
        float distanceToTarget = range;
        float distanceTmp = distanceToTarget;
        targetIndex = -1;
        for (int i = 0; i < EnemyManagerPro.enemies.Count; i++) {
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
    public abstract void TowerAttack(Enemy target);

    private void OnDrawGizmos()
    {
        // link to enemy
        for (int i = 0; i < EnemyManagerPro.enemies.Count; i++)
        {
            if (i == targetIndex)
            {
                Gizmos.color = Color.green;
            }
            else {
                Gizmos.color = Color.gray;
            }
            if (EnemyManagerPro.enemies[i]) {
                Gizmos.DrawLine(EnemyManagerPro.enemies[i].GetPosition(), this.transform.position);
            }
            
        }

        //range
        Vector3 compass = range * Vector3.forward;
        GL.Begin(GL.LINES);
        if(gizmoMaterial)
            gizmoMaterial.SetPass(0);
        GL.Color(color);
        for (int i = 0; i < 72; i++)
        {
            Vector3 circlPoint = transform.position - Vector3.up + compass;
            GL.Vertex(circlPoint);
            compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 10 degrees.
        }
        GL.End();

    }
}
