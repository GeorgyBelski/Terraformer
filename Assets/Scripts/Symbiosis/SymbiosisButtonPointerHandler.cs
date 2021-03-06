﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SymbiosisButtonPointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TowerMenuController towerMenuController;
    public Button thisSymiosisButton;

    public UnityEvent onClick; // SymbiosisClickButton(Button clickedButton)

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        towerMenuController.OnPointerDown();
        onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        towerMenuController.OnPointerUp();
      //  thisSymiosisButton.image.overrideSprite = ...
    }
}
