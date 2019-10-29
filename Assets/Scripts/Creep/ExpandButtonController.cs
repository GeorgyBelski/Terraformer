using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button thisButton;
    public CreepHexagonGenerator creepHexagonGenerator;
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
        isPointeOnTheButton = false;
        isPointerExit = true;
    }
    void ShowCost()
    {
        if (creepHexagonGenerator && isPointeOnTheButton && !lockShowCost)
        {
            Debug.Log("cost: " + 6 * (creepHexagonGenerator.radius + 1));
            lockShowCost = true;
        }
        else if (!isPointeOnTheButton)
        {

        }
    }

    void EnableButton()
    {
        if(!creepHexagonGenerator)
        { return; }

        if (creepHexagonGenerator.isExpanding || CreepHexagonGenerator.damagedHexagons.Count >0)
        {
            thisButton.interactable = false;
            isPointeOnTheButton = false;
            lockShowCost = false;
        }
        else
        {
            thisButton.interactable = true;
            if (!isPointerExit)
            {
                isPointeOnTheButton = true;
                
            }
        }
    }

    
}
