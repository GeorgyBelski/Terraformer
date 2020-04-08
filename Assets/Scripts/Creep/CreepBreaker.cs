using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CreepBreaker : MonoBehaviour
{
    public bool ableToBreak = false, isSmashJump =false;
    CreepHexagonGenerator.Hexagon hexagon;
    public Animator animator;
    public EnemyEffectsController effectController;
    public Enemy_Logic enemyLogic;
    public float cooldown = 3f;
    public float timerCooldown;
    public Image chargeBar;
    public float timerBreakingCreep;

    [Header("Sounds")]
    public AudioSource audioSource;
    public List<AudioClip> sounds;

    void Start()
    {
        
    }

    void Update()
    {
        DefineHexagon();
        SmashJump();
        ReduceTimer();
        ReduceTimerBreakingCreep();
    }

    void ReduceTimer() 
    {
        timerCooldown -= Time.deltaTime;
        if (timerCooldown <= 0) 
        {
            timerCooldown = 0;
            if (hexagon == null) 
            {
                ableToBreak = true;
            }
        }
        if (chargeBar)
        { chargeBar.fillAmount = 1 - timerCooldown / cooldown; }
    }
    void ReduceTimerBreakingCreep()
    {
        if (enemyLogic.isCasting)
        {
            timerBreakingCreep -= Time.deltaTime;
            if (timerBreakingCreep <= 0)
            {
                timerBreakingCreep = 0;
                FinishBreakingCreep();
            }
        }
    }
    
    void DefineHexagon()
    {
        if (!ableToBreak || !enemyLogic.isGoingToDest) { return; }

        if (Physics.Raycast(transform.position + Vector3.up*0.3f, Vector3.down, out RaycastHit hit, 2f, CreepHexagonGenerator.creepLayerMask))
        {
           // Debug.DrawRay(transform.position + Vector3.up * 0.3f, Vector3.down * hit.distance, Color.yellow);
            GameObject hexagonGameObject = hit.collider.gameObject;
            if (CreepHexagonGenerator.meshHexagonMap.TryGetValue(hexagonGameObject, out hexagon))
            {
                if (hexagon.GetStatus() == HexCoordinatStatus.Attend && !hexagon.isTarget)
                {
                    isSmashJump = true;
                    hexagon.isTarget = true;
                }
                else 
                { hexagon = null; }
            }
        }
      //  breakHexagon = false;
    }
    void SmashJump()
    {
        if (isSmashJump && !effectController.tpCharacter.m_Stun)
        {
            //enemyLogic.enabled = false;
            StopToBreakingCreep();
            animator.SetBool("SmashGround", true);
            //  effectController.navAgent.speed = 0;
            ableToBreak = false;
            isSmashJump = false;
            //timerCooldown = cooldown;

        }
        if (hexagon != null)
        {
            //  effectController.tpCharacter.Move(hexagon.hexagonGObject.transform.position, false, false);
            this.gameObject.transform.LookAt(hexagon.hexagonGObject.transform.position);
        }
    }
    public void StopToBreakingCreep()
    {
        enemyLogic.isCasting = true;
        timerBreakingCreep = 2.0f;
        animator.SetBool("Attack", false);
        effectController.navAgent.speed = 0;
    }


    public void SmashHexagon() // Activates in Animation
    {
        if (hexagon !=null && hexagon.GetStatus() == HexCoordinatStatus.Attend)
        {
            audioSource.pitch = Random.Range(0.9f, 1.2f);
            audioSource.PlayOneShot(sounds[0], 0.4f);
            hexagon.DamageHexagon();
            hexagon.isTarget = false;
            timerCooldown = cooldown;
        }
        hexagon = null;
        //animator.SetBool("SmashGround", false);
        
        
    }
    public void FinishBreakingCreep()
    {
        enemyLogic.isCasting = false;
       // Debug.Log("SmashGround false");
        animator.SetBool("SmashGround", false);
        effectController.navAgent.speed = effectController.originalNavAgentSpeed;
    }

}
