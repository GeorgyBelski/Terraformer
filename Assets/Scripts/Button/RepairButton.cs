using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairButton : MonoBehaviour
{
    public static bool isActive = false;
    public Image repairImage;
    public Button thisButton;
    // Start is called before the first frame update

    public void switchOnRepair()
    {
        isActive = true;
        repairImage.color = thisButton.colors.selectedColor;
        print("+");
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
            print("-");
        }

        if (isActive && Input.GetMouseButtonDown(0))
        {
            thisButton.Select();// = true;
        }
    }
}
