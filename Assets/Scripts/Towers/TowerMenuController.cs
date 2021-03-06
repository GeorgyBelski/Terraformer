﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TargetingType { Nearest, MostVurnerable, MostHardy };
//public enum SimbiosisState {None, Breaked };
public class TowerMenuController : MonoBehaviour
{
    

    [Header("References")]
    public Image TowerMenu;
    public Tower tower;
    Material material;
    

    [Header("Targeting Buttons")]
    public Button NearestButton;
    public Button VurnerableButton;
    public Button HardyButton;
    public Button previousSelectedTargetingButton;

    [Header("Symbiosis Buttons")]
    public Image symbiosisCircleBar;
    public float setSymbiosisTime = 0.8f;
    public float timerSetSymbiosisTime = 0f; // Increasing
    bool isInstallingSymbiosis = false;
    public float breakSymbiosisHoldingTime = 0.6f;
    public float timerBreakSymbiosisHoldingTime; // Decreasing
    public bool isHoldingSimbiosisClick = false;
    public bool isBreakingSymbiosis = false;

    [Header("Sounds")]
    public AudioSource audioSource;
    public List<AudioClip> sounds;

    void Start()
    {
      //  material = GetComponent<MeshRenderer>().material;
        previousSelectedTargetingButton = NearestButton;
        ClickButton(NearestButton);
        symbiosisCircleBar.fillAmount = 0;
    }

    void LateUpdate()
    {
        CalculateSize();
    }
    void Update()
    {
        
        ShowTowerMenu();
        BreakingSymbiosis();
        InstallingSymbiosis();
        //if(RepairButton.isActive && tower.isSelected)

        /*
        if (Input.GetMouseButtonUp(0) && RepairButton.isActive && tower.isSelected)
        {
            audioSource.pitch = 1;
            audioSource.PlayOneShot(sounds[0], 0.6f);
            //RepairButton.isActive = false;
            //print("Repair");
            tower.towerHealth.Repair();
        }
        */
    }

    

    void ShowTowerMenu()
    {
        if (((tower.isSelected && !TowerMenu.IsActive()) ||(!tower.isSelected && TowerMenu.IsActive())) && !RepairButton.isActive)
        {
            //audioSource.pitch = 1;
            //audioSource.PlayOneShot(sounds[1], 0.6f);
            TowerMenu.gameObject.SetActive(tower.isSelected);
        }
    }

    void CalculateSize() 
    {
        float distanceToCamera = (Camera.main.transform.position - TowerMenu.transform.position).magnitude;
        float scalefactor = Mathf.Min(2.5f, 1 + distanceToCamera / 40);
        TowerMenu.transform.localScale = Vector3.one * scalefactor;
    }

    public void ClickButton(Button clickedButton)
    {
        previousSelectedTargetingButton.interactable = true;
        clickedButton.interactable = false;
        previousSelectedTargetingButton = clickedButton;
        if (clickedButton == NearestButton)
        {
            tower.targetingType = TargetingType.Nearest;
        }
        else if (clickedButton == VurnerableButton)
        {
            tower.targetingType = TargetingType.MostVurnerable;
        }
        else {
            tower.targetingType = TargetingType.MostHardy;
        }
    }
    public void StartInstallingSymbiosis()
    {
        isInstallingSymbiosis = true;
        tower.symbiosisTower.towerMenuController.isInstallingSymbiosis = true;
    }
    private void InstallingSymbiosis()
    {
        if (isInstallingSymbiosis)
        {
            timerSetSymbiosisTime += Time.deltaTime;
            float ratio = timerSetSymbiosisTime / setSymbiosisTime;
            symbiosisCircleBar.fillAmount = ratio;
            if (tower.currentVisualLink && tower.currentVisualLink.isActiveAndEnabled)
            {
                tower.currentVisualLink.SetGradientProgress(ratio);
            }
            if (timerSetSymbiosisTime >= setSymbiosisTime)
            {
                ConfirmSymbiosis();
                isInstallingSymbiosis = false;
                timerSetSymbiosisTime = 0;
            }
        }
    }
    public void ConfirmSymbiosis()
    {
       // Debug.Log("ConfirmSymbiosis");
        symbiosisCircleBar.fillAmount = 1;
        timerBreakSymbiosisHoldingTime = breakSymbiosisHoldingTime;
        if (tower && tower.symbiosisTower)
        {
            tower.isSymbiosisInstalled = true;
            tower.ActivateSymbiosisUpgrade();

            ResourceManager.isTowersSupplyChanged = true;
            //  tower.symbiosisTower.isSymbiosisInstalled = true;
            //  tower.symbiosisTower.ActivateSymbiosisUpgrade();

        }
        else
        {
            ResetSymbiosisTimers();
        }
        
    }

    public void SymbiosisClickButton(Button clickedButton)
    {
        if (!tower.isSymbiosisInstalled)
        {
            TowerManager.StartLookingSimbiosisPartner(tower);
           // Debug.Log("TowerManager.towerLookingForSymbiosisPartner = " + tower);
        }
        else
        {
            isBreakingSymbiosis = true;
        }
    }
    void BreakingSymbiosis()
    {
        if (isBreakingSymbiosis && isHoldingSimbiosisClick)
        {
            timerBreakSymbiosisHoldingTime -= Time.deltaTime;
            symbiosisCircleBar.fillAmount = timerBreakSymbiosisHoldingTime / breakSymbiosisHoldingTime;
            if (timerBreakSymbiosisHoldingTime <= 0)
            {
                tower.symbiosisTower.towerMenuController.ResetSymbiosisTimers();
                ResetSymbiosisTimers();
                tower.BreakSymbiosis();
                isBreakingSymbiosis = false;
            }
        }
    }

    public TowerMenuController ResetSymbiosisTimers()
    {
        timerSetSymbiosisTime = 0;
        timerBreakSymbiosisHoldingTime = 0;
        return this;
    }
    public TowerMenuController ResetSymbiosisCircleBar()
    {
        symbiosisCircleBar.fillAmount = 0;
        return this;
    }

    public void OnPointerDown()
    {
        isHoldingSimbiosisClick = true;
    }

    public void OnPointerUp()
    {
        isHoldingSimbiosisClick = false;
        isBreakingSymbiosis = false;
        if (timerBreakSymbiosisHoldingTime > 0)
        { CancelBreakingSymbiosis(); }
        
    }

    private void CancelBreakingSymbiosis()
    {
        symbiosisCircleBar.fillAmount = 1;
        timerBreakSymbiosisHoldingTime = breakSymbiosisHoldingTime;
    }
}
