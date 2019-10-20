using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terraformer : Tower
{
    new void Start()
    {
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
