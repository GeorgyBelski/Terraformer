using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbiosis
{
    public static int cost;
    static TowerType symbiosisTowerType;
    public static void DisplaySymbiosisCost(bool enable)
    {
        if (enable)
        { ResourceManager.resourceCost.text = "-" + cost; }
        else { ResourceManager.resourceCost.text = ""; }
    }

    public static TowerType? ActivateSymbiosisUpgrade(Tower tower) 
    {
        if (!tower.isSymbiosisInstalled)
        { return null; }

        tower.towerMaterial.SetFloat("_Float_Symbiosis", 1);
        symbiosisTowerType = tower.symbiosisTower.type;

        if (tower.type == TowerType.Electro) { ActivateElectroSymbiosisUpgrade((ElectroTower)tower); }
        else if (tower.type == TowerType.Laser) { ActivateLaserSymbiosisUpgrade((LaserTower)tower); }
        else { ActivatePlasmaSymbiosisUpgrade((PlasmaTower)tower); }

        return symbiosisTowerType;
    }
    public static void ActivateElectroSymbiosisUpgrade(ElectroTower tower)
    {

        if (symbiosisTowerType == TowerType.Electro)
        {
            tower.autoAttackMaterial.SetColor("_EnergyColor02", tower.electroSymbColor);
            tower.towerMaterial.SetColor("_Color_SymbEmition", tower.electroSymbColor);
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
            tower.towerMaterial.SetColor("_Color_SymbEmition", tower.laserSymbColor2);
            var trails = tower.lightningChargeParticleSys.trails;
            trails.ribbonCount = 1;
            tower.lightningLerpSpeed = 3;
        }
        else if (symbiosisTowerType == TowerType.Plasma)
        {
            tower.autoAttackMaterial.SetColor("_EnergyColor01", tower.plasmaSymbColor1);
            tower.autoAttackMaterial.SetColor("_EnergyColor02", tower.plasmaSymbColor2);
            tower.towerMaterial.SetColor("_Color_SymbEmition", tower.plasmaSymbColor2);
            var trails = tower.lightningChargeParticleSys.trails;
            trails.ribbonCount = 1;
            tower.lightningLerpSpeed = 2;
        }
    //    tower.isSymbiosisInstalled = false;
    }

    public static void ActivateLaserSymbiosisUpgrade(LaserTower tower)
    {

        if (symbiosisTowerType == TowerType.Electro)
        {
            tower.currentColor1 = tower.electroSymbColor1;
            tower.currentColor2 = tower.electroSymbColor2;
            tower.towerMaterial.SetColor("_Color_SymbEmition", tower.electroSymbColor2);
        }
        else if (symbiosisTowerType == TowerType.Laser)
        {
            tower.currentColor2 = tower.laserSymbColor;
            tower.towerMaterial.SetColor("_Color_SymbEmition", tower.laserSymbColor);
            tower.cooldownAttack /= 2;
        }
    //    tower.isSymbiosisInstalled = false;
    }

    internal static void ActivatePlasmaSymbiosisUpgrade(PlasmaTower plasmaTower)
    {
        
    }
}
