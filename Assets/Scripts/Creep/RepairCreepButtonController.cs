using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RepairCreepButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Button thisButton;
    public bool isPointeOnTheButton = false, isPointerExit = true;
    public bool lockShowCost = false;

    void Start()
    {

    }
    void Update()
    {
        EnableButton();
        ShowCost();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointeOnTheButton = true;
        isPointerExit = false;
        lockShowCost = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ResourceManager.resourceCost.text = "";
        isPointeOnTheButton = false;
        isPointerExit = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ResourceManager.resourceCost.text = "";
    }

    void ShowCost()
    {
        if (!lockShowCost && isPointeOnTheButton)
        {
            CreepHexagonGenerator.DisplayRepairingCost();
            lockShowCost = true;
        }
        
    }
    void EnableButton()
    {
        if (CreepHexagonGenerator.creepHexagonGenerator.isRepairing || CreepHexagonGenerator.damagedHexagons.Count == 0)
        {
            if (thisButton.interactable)
            {
                thisButton.interactable = false;
                isPointeOnTheButton = false;
                lockShowCost = false;
            }
        }
        else if(!thisButton.interactable)
        {
            thisButton.interactable = true;
            if (!isPointerExit)
            {
                isPointeOnTheButton = true;
            }
        }
    }
}
