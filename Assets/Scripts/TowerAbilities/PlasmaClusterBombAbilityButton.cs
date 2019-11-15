using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaClusterBombAbilityButton : AbilityButtonController
{
    public override void TowerCastAreaAbility(Tower casterTower)
    {
        ((PlasmaTower)casterTower).CastClusterBomb(aimArea.position);
    }
/*
    public override void Aiming()
    {
        if (aimArea)
        {
            ResourceManager.DisplayCost(true, cost);
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, groundMask))
            {
                mousePos = floorHit.point;
                aimArea.position = floorHit.point;
                //    TowerManager.ClearHighlighting();
                Tower nearestTower = TowerManager.GetNearestTower(aimArea, castTowerType);
                if (nearestTower == null)
                { Cancel(); }
                else if (nearestTower == previousHighlightedTower)
                { return; }
                if (castTowerType == TowerType.Electro)
                {
                    casterTower = (ElectroTower)nearestTower;
                }
                else if (castTowerType == TowerType.Laser)
                {
                    casterTower = (LaserTower)nearestTower;
                }
                else if (castTowerType == TowerType.Plasma)
                {
                    casterTower = (PlasmaTower)nearestTower;
                }

                if (casterTower && casterTower != previousHighlightedTower)
                {
                    casterTower.isHighlighted = true;
                    if (previousHighlightedTower) { previousHighlightedTower.isHighlighted = false; }
                    previousHighlightedTower = casterTower;
                }


            }
        }
    }
    */
}
