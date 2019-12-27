using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Button thisButton;
    public bool isPointeOnTheButton = false, isPointerExit = true;
    public bool lockShowCost = false;
    public Image highlight;


    void Start()
    {

    }

    void Update()
    {
        EnableButton();
        ShowCost();
        ShowHighlight();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointeOnTheButton = true;
        isPointerExit = false;
        lockShowCost = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ResourceManager.resourceCostAnimator.GetBool("tooHigh"))
        { ResourceManager.DisplayCost(false); }
        isPointeOnTheButton = false;
        isPointerExit = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (CreepHexagonGenerator.expansionCost <= ResourceManager.resource)
        { ResourceManager.DisplayCost(false); }
        else
        { lockShowCost = false; }
    }
    void ShowCost()
    {
        if (isPointeOnTheButton && !lockShowCost && thisButton.interactable)
        {
            CreepHexagonGenerator.DisplayExpensionCost();
            lockShowCost = true;
        }
    }

    void EnableButton()
    {
        if (CreepHexagonGenerator.creepHexagonGenerator.isExpanding || CreepHexagonGenerator.damagedHexagons.Count > 0)
        {
            if (thisButton.interactable)
            {
                thisButton.interactable = false;
                isPointeOnTheButton = false;
                lockShowCost = false;
                
            }

        }
        else if (!thisButton.interactable)
        {
            thisButton.interactable = true;
            if (!isPointerExit)
            {
                isPointeOnTheButton = true;

            }
        }
    }

    void ShowHighlight() 
    {
        if (CreepHexagonGenerator.expansionCost <= ResourceManager.resource)
        {
            if (!highlight.enabled)
            { highlight.enabled = true; }
        }
        else 
        {
            if (highlight.enabled)
            { highlight.enabled = false; }
        }
    }

    
}
