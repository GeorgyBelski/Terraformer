using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public enum SupportState {Go, Stay, Cast};
public class SupportController : MonoBehaviour
{
    public SupportState state = SupportState.Go;

    [Header("Reference")]
    public EnemyMouseController emk;
    public Animator animator;

    [Space]
    public List<Enemy> allies = new List<Enemy>();
    public int supportListMaxLength = 3;
    public Dictionary<Tower,float> threats = new Dictionary<Tower, float>();
    public int threatsListMaxLength = 3;
    public int threatsNumber = 0;
    int currentSupportIndex = 0, currentAttackertIndex = 0;
    Vector3 destination;

    void Start()
    {

    }

    void Update()
    {
        Go();
      //  Stay();
        Cast();
        threatsNumber = threats.Count;
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
        CalculateDestination();


    }
    void Cast() 
    {
        if (state != SupportState.Cast || emk.character.m_Stun)
        { return; }


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
        Gizmos.DrawLine(transform.position, destination);
        Gizmos.DrawSphere(transform.position + dest, 0.5f);
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(destination, 0.5f);
    }
    void CalculateDestination()
    {
        DefineThreats();
        if (threats.Count != 0)
        {
            destination = Vector3.zero;
            foreach (var threat in threats)
            {
                if (threat.Key)
                {
                    destination += (transform.position - threat.Key.transform.position).normalized * threat.Value;
                }

            }
            destination += transform.position;
        }
    }

    void DefineThreats() 
    {
        float maxDangerDepth = 0;
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
                        if (threat.Value > minDangerDepth)
                        {
                            minDangerTower = threat.Key;
                            minDangerDepth = threat.Value;
                        }
                        
                    }
                    destination += transform.position;
                    if (threats.Count == threatsListMaxLength && dangerDepth > minDangerDepth)
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
    

}
