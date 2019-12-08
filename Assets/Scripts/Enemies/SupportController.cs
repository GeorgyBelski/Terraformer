using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public enum SupportState {Go, Stay, Cast};
public class SupportController : MonoBehaviour
{
    public SupportState state = SupportState.Go;

    [Header("Reference")]
    public Enemy thisUnit;
    public EnemyMouseController emk;
    public Animator animator;

    [Space]
    public float supportRange = 12f;
    public List<Enemy> allies = new List<Enemy>();
    public int supportListMaxLength = 3;
    public float defineDestinationsCooldown = 0.5f;
    public float timerSafeDestination,timerSupportDestination = 0;
    public Dictionary<Tower,float> threats = new Dictionary<Tower, float>();
    public int threatsListMaxLength = 3;
    public int threatsNumber = 0;
   // public List<SupportAbility> supportAbilities = new List<SupportAbility>();
    public SupportAbility activeAbility = null;
    [HideInInspector]
    public Vector3 safeDestination, supportDestination;
    [HideInInspector] public Vector3 lookAt;
    float centerOutFactor = 2f;

    void Start()
    {
        timerSafeDestination = defineDestinationsCooldown / 2;
        state = SupportState.Go;
    }

    void Update()
    {
        Go();
      //  Stay();
        Cast();
        threatsNumber = threats.Count;
        ReduceTimers();
    }

    void ReduceTimers() 
    {
        timerSafeDestination -= Time.deltaTime;
        timerSupportDestination -= Time.deltaTime;
    }

    void Stay()
    {
        if (state != SupportState.Stay) 
        { return;}

    }
    void Go() 
    {
        if (state != SupportState.Go)
        { return; }
        CalculateSafeDestination();
        CalculateSupportDestination();
        if (thisUnit.GetHealthRatio() == 1)
        { emk.SetDestination(supportDestination);}
        else
        { emk.SetDestination(safeDestination); }

    }
    void Cast()
    {
        if (state != SupportState.Cast || emk.character.m_Stun)
        { return; }
        else if (emk.character.m_Stun && state == SupportState.Cast)
        {
            state = SupportState.Go;
            EndCast();
            return;
        }

        emk.SetDestination(transform.position);
        emk.SetRotation(lookAt);
        /*
        int readyAbilities = 0;
        foreach (var ability in supportAbilities)
        {
            if (ability.state != SupportAbilityState.Recharge)
            {
                readyAbilities++;
                break;
            }
        }
        if (readyAbilities == 0) 
        { EndCast(); }
        */
    }
    public void EndCast() 
    {
        state = SupportState.Go;
        activeAbility.EndCast();
        activeAbility = null;
    }
    void OnDrawGizmosSelected()
    {
        
        //  Gizmos.DrawCube(Vector3.zero, Vector3.one*3f);
        Vector3 dest = Vector3.zero;
        foreach (var threat in threats)
        {
            if (threat.Key) 
            {
                Gizmos.color = new Color(1, 0, 0, 1f);
                Gizmos.DrawCube(threat.Key.transform.position + Vector3.up*3f, new Vector3(1, threat.Value, 1));
                Vector3 outOfTower = (transform.position - threat.Key.transform.position).normalized * threat.Value;
                Gizmos.color = new Color(1, 1, 0, 1f);
                Gizmos.DrawLine(transform.position, transform.position + outOfTower);
                dest += outOfTower;
            }
        }
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawLine(transform.position, safeDestination);
        Gizmos.DrawSphere(transform.position + dest, 0.5f);
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(safeDestination, 0.5f);

        foreach (var ally in allies) 
        {
            if (ally) { 
                Gizmos.color = new Color(1- ally.GetHealthRatio(), ally.GetHealthRatio(), 0, 0.4f);
                Gizmos.DrawCube(ally.GetPosition(), new Vector3(1, 1/(0.5f + ally.GetHealthRatio()), 1));
            }
        }
        Gizmos.color = new Color(0, 0, 1, 0.7f);
        Gizmos.DrawSphere(supportDestination, 0.5f);
    }
    void CalculateSafeDestination()
    {
        if (timerSafeDestination > 0) 
        { return; }
        else 
        { timerSafeDestination = defineDestinationsCooldown; }

        DefineThreats();
        if (threats.Count != 0)
        {
            safeDestination = Vector3.zero;
            foreach (var threat in threats)
            {
                if (threat.Key)
                {
                    safeDestination += (transform.position - threat.Key.transform.position).normalized * threat.Value;
                }

            }
            
            safeDestination += (transform.position + transform.position.normalized * CalculateCenterOutFactor(true));
        }
    }
    float CalculateCenterOutFactor(bool safe) 
    { return safe?2:threats.Count*0.5f + ((10 + (1 - thisUnit.GetHealthRatio()) * 20 * threats.Count) / transform.position.magnitude); }
    void DefineThreats() 
    {
        var towers = TowerManager.towers;
        towers.ForEach(tower => {
            float distanceToTower = (transform.position - tower.transform.position).magnitude;
            float dangerDepth = tower.range - distanceToTower;
            if (dangerDepth > 0)
            {
                if (threats.ContainsKey(tower))
                {
                    threats[tower] = dangerDepth;
                }
                else { 
                    float minDangerDepth = 30f;
                    Tower minDangerTower = null;
                    
                    foreach (var threat in threats)
                    {   
                        if (threat.Value < minDangerDepth)
                        {
                            minDangerTower = threat.Key;
                            minDangerDepth = threat.Value;
                        }
                        
                    }
                    safeDestination += transform.position;
                    if (threats.Count >= threatsListMaxLength && dangerDepth > minDangerDepth)
                    {
                        threats.Remove(minDangerTower);
                        threats.Add(tower, dangerDepth);
                    }
                    else if (threats.Count < threatsListMaxLength) 
                    {
                        threats.Add(tower, dangerDepth);
                    }
                }
            }
            else
            {
                if (threats.ContainsKey(tower))
                { threats.Remove(tower); }
            }
        });
    }
    void CalculateSupportDestination() 
    {
        if (timerSupportDestination > 0)
        { return; }
        else
        { timerSupportDestination = defineDestinationsCooldown; }

        DefineAllies();
        Vector3 destination = Vector3.zero;
        if (allies.Count > 0)
        {
            allies.ForEach(ally => {
                destination += ally.transform.position;
            });
            destination /= allies.Count;
            destination += thisUnit.transform.position.normalized * CalculateCenterOutFactor(false);
        }
        supportDestination = destination;
    }
    void DefineAllies()
    {
        float minHealthRatio = 1.1f;
        var units = EnemyManagerPro.enemies;
        Vector3 fromUnitToSupport;

        float maxHealthRation = 0;
        Enemy maxHealthAlly = null;

        ClearDeadAllies();

        units.ForEach(unit =>
        {
            if (unit != thisUnit && unit.type != EnemyType.Totem)
            {
                fromUnitToSupport = unit.transform.position - thisUnit.transform.position;
                if (fromUnitToSupport.magnitude <= supportRange)
                {

                    minHealthRatio = unit.GetHealthRatio();
                    if(!allies.Contains(unit)) 
                    {
                        if(allies.Count < supportListMaxLength) { 
                            allies.Add(unit);
                        }
                        else 
                        {
                            maxHealthRation = 0;
                        
                            allies.ForEach(ally => {
                                if (maxHealthRation < ally.GetHealthRatio())
                                {
                                    maxHealthRation = ally.GetHealthRatio();
                                    maxHealthAlly = ally;
                                }
                            });
                            if (maxHealthAlly && maxHealthRation > unit.GetHealthRatio())
                            {
                                allies.Remove(maxHealthAlly);   
                                allies.Add(unit);
                            }
                        }
                    }
                }
            }
        });
    }
    void ClearDeadAllies()
    {
        List<Enemy> toRemove = new List<Enemy>();
        allies.ForEach(ally => {
            if (ally.GetHealthRatio() <= 0) 
            {
                toRemove.Add(ally);
            }
        });
        toRemove.ForEach(ally => allies.Remove(ally));
    }
}
