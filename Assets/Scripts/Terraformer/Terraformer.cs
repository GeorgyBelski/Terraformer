using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terraformer : Tower
{
    public override void TowerAttack(Enemy target)
    {
        //throw new System.NotImplementedException();
    }

    internal override void TowerUpdate()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        TowerManager.terraformer = this;
        base.Start();
    }
}
