using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//public enum towerMenuButtonType {Nearesr, Vulnerable, Hardy, Symbiosis }
public class TowerMenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public towerMenuButtonType type;
    public GameObject textPanel;

    private void Start()
    {
        textPanel.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        textPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textPanel.SetActive(false);
    }

}
