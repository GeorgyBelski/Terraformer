using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairButton : MonoBehaviour
{
    public static bool isActive = false;
    bool previousButtonState = isActive;
    public Image repairImage;
    public Button thisButton;
    // Start is called before the first frame update

    public void switchOnRepair()
    {
        isActive = true;
        previousButtonState = isActive;
        repairImage.color = thisButton.colors.selectedColor;
        //print("+");
        TowerManager.StartLookingRepairTower();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            switchOnRepair();
            thisButton.Select();    
        }
            

        if (isActive && Input.GetMouseButtonDown(1))
        {
            isActive = false;
            repairImage.color = thisButton.colors.normalColor;
            //print("-");
        }
       if (previousButtonState && !isActive)
        {
            //thisButton.
            repairImage.color = thisButton.colors.normalColor;
            previousButtonState = isActive;
        }
    }
}
