using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbiosis
{

    public static TowerType? ActivateElectroSymbiosisUpgrade(ElectroTower tower)
    {
       // Debug.Log("ActivateElectroSymbiosisUpgrade");
        if (!tower.isSymbiosisInstalled)
        { return null; }
        TowerType symbiosisTowerType = tower.symbiosisTower.type;
        if (symbiosisTowerType == TowerType.Electro)
        {
            tower.autoAttackMaterial.SetColor("_EnergyColor02", tower.electroSymbColor);
            tower.cooldownAttack /= 2;
            var trails = tower.lightningChargeParticleSys.trails;
            trails.ribbonCount = 3;
            tower.lightningLerpSpeed = 3;
        }
        else if (symbiosisTowerType == TowerType.Laser)
        {
            //   tower.particleSystemRenderer.trailMaterial = tower.laserSymbiosisMaterial;          
            tower.autoAttackMaterial.SetColor("_EnergyColor01", tower.laserSymbColor1);
            tower.autoAttackMaterial.SetColor("_EnergyColor02", tower.laserSymbColor2);
            var trails = tower.lightningChargeParticleSys.trails;
            trails.ribbonCount = 1;
            tower.lightningLerpSpeed = 3;
        }


        tower.isSymbiosisInstalled = false;
        return symbiosisTowerType;
    }

    public static TowerType? ActivateLaserSymbiosisUpgrade(LaserTower tower)
    {
        if (!tower.isSymbiosisInstalled)
        { return null; }
        TowerType symbiosisTowerType = tower.symbiosisTower.type;
        if (symbiosisTowerType == TowerType.Electro)
        {
            tower.currentColor1 = tower.electroSymbColor1;
            tower.currentColor2 = tower.electroSymbColor2;
        }
        else if (symbiosisTowerType == TowerType.Laser)
        {
            tower.currentColor2 = tower.laserSymbColor;
            tower.cooldownAttack /= 2;
        }

        tower.isSymbiosisInstalled = false;
        return symbiosisTowerType;
    }

}
