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
    void Start()
    {
        
    }

    void Update()
    {
        DefineHexagon();
        SmashJump();
        ReduceTimer();
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
    void SmashJump() 
    {
        if (isSmashJump && !effectController.tpCharacter.m_Stun) 
        {
            enemyLogic.enabled = false;
            animator.SetBool("SmashGround", true);
            effectController.navAgent.speed = 0;
            ableToBreak = false;
            isSmashJump = false;
            timerCooldown = cooldown;
        }
        if (hexagon!= null) 
        {
            //  effectController.tpCharacter.Move(hexagon.hexagonGObject.transform.position, false, false);
            this.gameObject.transform.LookAt(hexagon.hexagonGObject.transform.position);
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

    
    public void SmashHexagon() // Activates in Animation
    {
        if (hexagon !=null && hexagon.GetStatus() == HexCoordinatStatus.Attend)
        { 
            hexagon.DamageHexagon();
            hexagon.isTarget = false;
        }
        hexagon = null;
        animator.SetBool("SmashGround", false);
        
        
    }

    public void ReturnToMove() 
    {
        effectController.navAgent.speed = effectController.originalNavAgentSpeed;
        isSmashJump = false;
        enemyLogic.enabled = true;
    }
}
