using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetingType { Nearest, MostVurnerable, MostHardy };

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

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        previousSelectedTargetingButton = NearestButton;
        ClickButton(NearestButton);
    }

    void Update()
    {
        ShowTowerMenu();
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

    public void SymbiosisClickButton(Button clickedButton)
    {
        if (!tower.symbiosisTower)
        {
            TowerManager.towerLookingForSymbiosisPartner = tower;
            Debug.Log("TowerManager.towerLookingForSymbiosisPartner = " + tower);
        }
        else
        {

        }
    }

}
