using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static List<Tower> towers = new List<Tower>();

    public static List<ElectroTower> availableElectroTowers = new List<ElectroTower>();
    public static List<LaserTower> availableLaserTowers = new List<LaserTower>();

    public static float selectedTowerRange = 1.5f;

    int towerEnemyLayerMask = (1 << 13 | 1 << 12);
    int towerLayer = 13;
    public static TowerMenuController selectedTower;


    private void Start()
    {

    }

    void LateUpdate() {
        //   TowerCount = towers.Count;
        SelectTower();
    }

    public static void AddTower(Tower tower)
    {
        towers.Add(tower);
        if (tower is ElectroTower)
        {
            availableElectroTowers.Add((ElectroTower)tower);
        }
        else if (tower is LaserTower)
        {
            availableLaserTowers.Add((LaserTower)tower);
        }
    }

    public static void RemoveTower(Tower tower) {
        towers.Remove(tower);
        if (tower.type == TowerType.Electro)
        {
            availableElectroTowers.Remove((ElectroTower)tower);
        }
        else if (tower.type == TowerType.Laser)
        {
            availableLaserTowers.Remove((LaserTower)tower);
        }
    }

    public static Tower GetNearestTower(Transform target, TowerType type) {
        float minDistance = float.PositiveInfinity;
        int nearestTowerIndex = -1;
        for (int i = 0; i < TowerManager.towers.Count; i++)
        {
            if (TowerManager.towers[i].type == type && TowerManager.towers[i].IsCastingAbility == false)
            {
                Vector3 toTarget = target.position - TowerManager.towers[i].transform.position;
                float distance = toTarget.magnitude;
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestTowerIndex = i;
                }
            }
        }
        if (nearestTowerIndex == -1) {
            return null;
        }
        return TowerManager.towers[nearestTowerIndex];
    }

    public static void ClearHighlighting() {
      /*  foreach (Tower tower in towers) {
            tower.isHighlighted = false;
        }*/
        towers.ForEach(tower => tower.isHighlighted = false);
    }

    public void SelectTower()
    {

        if (AbilityButtonController.aimingAbility == null && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, towerEnemyLayerMask))
            {
                if (hit.transform.gameObject.layer == towerLayer)
                {
                    if (selectedTower){selectedTower.isSelected = false; }

                    selectedTower = hit.transform.gameObject.GetComponent<TowerMenuController>();
                    selectedTower.isSelected = true;
                }
                
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedTower) { selectedTower.isSelected = false; }
        }
    }
}
