using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terraformer : Tower
{
    [Header("Menu")]
    public GameObject menu;
    public TextMeshProUGUI defeat;


    new void Start()
    {
        menu.SetActive(false);
        defeat.gameObject.SetActive(false);
        TowerManager.terraformer = this;
        TowerManager.AddTower(this);
    }

    void Update()
    {

    }

    public override void TowerAttack(Enemy target)
    {

    }

    internal override void TowerUpdate()
    {
        
    }

    public override void EndCasting()
    {

    }

    public override void ActivateSymbiosisUpgrade()
    {
        
    }

    public override void DisableSymbiosisUpgrade()
    {
        
    }
}
