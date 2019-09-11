using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuController : MonoBehaviour
{
 //   public bool towerIsClicked;

    [Header("References")]
    public Image TowerMenu;
    public Tower tower;
    Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        ShowTowerMenu();
    }

    void ShowTowerMenu()
    {
        if ((tower.isSelected && !TowerMenu.enabled) ||(!tower.isSelected && TowerMenu.enabled))
        {
            TowerMenu.enabled = tower.isSelected;
            if (tower.isSelected)
            {
                material.SetColor("Color_19495AAD", Color.yellow);
            }
            else
            {
                material.SetColor("Color_19495AAD", Color.black);
            }
        }
    }
}
