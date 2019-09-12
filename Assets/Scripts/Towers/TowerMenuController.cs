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
    public bool isSelected;

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
        if ((isSelected && !TowerMenu.enabled) ||(!isSelected && TowerMenu.enabled))
        {
            TowerMenu.enabled = isSelected;
        /*    if (tower.isSelected)
            {
                material.SetColor("Color_19495AAD", Color.yellow);
            }
            else
            {
                material.SetColor("Color_19495AAD", Color.black);
            }*/
        }
    }
}
