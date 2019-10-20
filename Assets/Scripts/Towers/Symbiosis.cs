using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbiosis
{

    public static TowerType? ActivateElectroSymbiosisUpgrade(ElectroTower tower)
    {
        if (!tower.isSymbiosisInstalled)
        { return null; }
        TowerType symbiosisTowerType = tower.symbiosisTower.type;
        if (symbiosisTowerType == TowerType.Electro)
        {

        }
        else if (symbiosisTowerType == TowerType.Laser)
        {
            tower.particleSystemRenderer.trailMaterial = tower.laserSymbiosisMaterial;

            var trails = tower.lightningChargeParticleSys.trails;
            trails.ribbonCount = 1;
            tower.lightningLerpSpeed = 3;
        }


        tower.isSymbiosisInstalled = false;
        return symbiosisTowerType;
    }

}
