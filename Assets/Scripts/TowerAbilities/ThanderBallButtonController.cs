﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanderBallButtonController : AbilityButtonController
{
    /*
    public enum State { Ready, Aiming, Casting, Recharging};
    public State currentState = State.Ready;
    public float castTime = 2.0f;
    public float timerCast;
    public float coolDown = 5.0f;
    public float timerCoolDown;
    public Color buttomTintReady;
    public Color buttomTintRecharging;

    public Transform gunpoint;
   // public GameObject thunderball;
    public GameObject aimAreaPrefab;
    public Transform aimArea;
    Animator aimAreaAnimator;

    ElectroTower casterTower;
    float camRayLength = 90f;
    int groundMask;
    Vector3 mousePos;

    Image buttonImage;
    Button button;

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        buttonImage.color = buttomTintReady;
        buttonImage.fillAmount = 1f;
        aimArea = null;
    }

    void Update()
    {
        ReduceTimers();

        ButtonAvailabilityControl();
        if (currentState == State.Ready && Input.GetKey(KeyCode.Q)) {
            ETA1Activate();
        }

        if (currentState == State.Aiming) {
            Aiming();
            if (Input.GetMouseButtonDown(0))
            {
                if (casterTower)
                {
                    currentState = State.Casting;
                    buttonImage.color = buttomTintRecharging;
                    timerCoolDown = coolDown;
                    timerCast = castTime;
                    TowerManager.ClearSelection();
                    casterTower.CastThanderBall(aimArea.position);
                }
                else {
                    currentState = State.Ready;
                    RemoveAimArea();
                }
                
                RemoveAimArea();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                currentState = State.Ready;
                RemoveAimArea();
            }
            
        }
    }

    public void ETA1Activate()
    {
        timerCast = castTime;

        if (currentState == State.Ready) {
            currentState = State.Aiming;
            SetAimArea();
            
        }

    }

    private void SetAimArea()
    {
        if (!aimArea)
        {
            GameObject aimAreaGameObject = Instantiate(aimAreaPrefab, Vector3.zero, this.transform.rotation);
            aimArea = aimAreaGameObject.transform;
            aimAreaAnimator = aimAreaGameObject.GetComponent<Animator>();

        }
        else {
            aimAreaAnimator.SetBool("isSetted", false);
        }
    }

    void Aiming()
    {
        if (aimArea) {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, groundMask))
            {
                mousePos = floorHit.point;
                aimArea.position = floorHit.point;
                TowerManager.ClearSelection();

                casterTower = (ElectroTower)TowerManager.GetNearestTower(aimArea, TowerType.Electro);
                if (casterTower) {
                    casterTower.isSelected = true;
                }
                    
            }
        }
    }

    void ReduceTimers() {
        
        if (timerCoolDown <= 0)
        {
            timerCoolDown = 0;
            if (currentState == State.Recharging) {
                currentState = State.Ready;
                buttonImage.color = buttomTintReady;
            }
            
        }
        else{

            timerCoolDown -= Time.deltaTime;
            buttonImage.fillAmount = (coolDown - timerCoolDown) / coolDown;
        }

        if (timerCast <= 0)
        {
            timerCast = 0;
            if (currentState == State.Casting)
            {
                currentState = State.Recharging;
                casterTower.EndCasting();

            }
        }
        else{

            timerCast -= Time.deltaTime;
        }
    }
    void RemoveAimArea() {
        if (aimArea) {
            //    Destroy(aimArea.gameObject);
            //   aimArea = null;
            aimAreaAnimator.SetBool("isSetted", true);
        }
    }

    void ButtonAvailabilityControl() {
        if (TowerManager.availableElectroTowers.Count == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
    */
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        ((ElectroTower)casterTower).CastThanderBall(aimArea.position);
    }
}
