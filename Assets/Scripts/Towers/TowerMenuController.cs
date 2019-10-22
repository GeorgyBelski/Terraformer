using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TargetingType { Nearest, MostVurnerable, MostHardy };
public enum SimbiosisState {None, Breaked };
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
    public Button SymbiosisButton;
    public Image symbiosisCircleBar;
    public float setSymbiosisTime = 0.8f;
    public float timerSetSymbiosisTime = 0f; // Increasing
    bool isInstallingSymbiosis = false;
    public float breakSymbiosisHoldingTime = 0.6f;
    public float timerBreakSymbiosisHoldingTime; // Decreasing
    public bool isHoldingClick = false;
    public bool isBreakingSymbiosis = false;

    void Start()
    {
      //  material = GetComponent<MeshRenderer>().material;
        previousSelectedTargetingButton = NearestButton;
        ClickButton(NearestButton);
        symbiosisCircleBar.fillAmount = 0;
    }

    void Update()
    {
        ShowTowerMenu();
        BreakingSymbiosis();
        InstallingSymbiosis();
    }

    

    void ShowTowerMenu()
    {
        if ((tower.isSelected && !TowerMenu.IsActive()) ||(!tower.isSelected && TowerMenu.IsActive()))
        {
            TowerMenu.gameObject.SetActive(tower.isSelected);
        }
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
            symbiosisCircleBar.fillAmount = timerSetSymbiosisTime / setSymbiosisTime;
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
        symbiosisCircleBar.fillAmount = 1;
        timerBreakSymbiosisHoldingTime = breakSymbiosisHoldingTime;
        if (tower && tower.symbiosisTower)
        {
            tower.isSymbiosisInstalled = true;
            tower.ActivateSymbiosisUpgrade();

            tower.symbiosisTower.isSymbiosisInstalled = true;
            tower.symbiosisTower.ActivateSymbiosisUpgrade();

        }
        else
        {
            ResetSymbiosisTimers();
        }
        
    }

    public void SymbiosisClickButton(Button clickedButton)
    {
        if (!tower.symbiosisTower)
        {
            TowerManager.towerLookingForSymbiosisPartner = tower;
           // Debug.Log("TowerManager.towerLookingForSymbiosisPartner = " + tower);
        }
        else
        {
            isBreakingSymbiosis = true;
        }
    }
    void BreakingSymbiosis()
    {
        if (isBreakingSymbiosis && isHoldingClick)
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
        isHoldingClick = true;
    }

    public void OnPointerUp()
    {
        isHoldingClick = false;
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
