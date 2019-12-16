using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerManager : MonoBehaviour
{
    public int availablePlazmaTowersCount;
    public int towersNumber;

    public static List<Tower> towers = new List<Tower>();

    public static Tower terraformer { get; set; }
   // public Terraformer terraformer

    public static List<ElectroTower> availableElectroTowers = new List<ElectroTower>();
    public static List<LaserTower> availableLaserTowers = new List<LaserTower>();
    public static List<PlasmaTower> availablePlasmaTowers = new List<PlasmaTower>();

    public static float selectedTowerRange = 1.5f;
    public static Dictionary<Transform, Tower> transformTowerMap = new Dictionary<Transform, Tower>();
    public static HashSet<Tower> symbiosisTowers = new HashSet<Tower>();

    int towerEnemyLayerMask = (1 << 13 | 1 << 12);
    int towerLayerMask = 1 << 13;
    int towerLayer = 13;
    public static Tower selectedTower;
    public static Tower highlightedTower;
    public static Tower towerLookingForSymbiosisPartner;

    public int symbiosisCostMultiplayer = 8;

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        LookingForSymbiosis();
    }
    void LateUpdate()
    {
        SelectTower();
        availablePlazmaTowersCount = availablePlasmaTowers.Count;
        towersNumber = towers.Count;
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
        else if (tower is PlasmaTower)
        {
            availablePlasmaTowers.Add((PlasmaTower)tower);
        }
    }

    public static void RemoveTower(Tower tower) {
        towers.Remove(tower);
        transformTowerMap.Remove(tower.transform);
        if (tower.type == TowerType.Electro)
        {
            availableElectroTowers.Remove((ElectroTower)tower);
        }
        else if (tower.type == TowerType.Laser)
        {
            availableLaserTowers.Remove((LaserTower)tower);
        }
        else if(tower.type == TowerType.Plasma)
        {
            availablePlasmaTowers.Remove((PlasmaTower)tower);
        }
    }
    public static bool IsAvailableTowersByType(TowerType type)
    {
        if (type == TowerType.Electro) { return availableElectroTowers.Count > 0 ? true : false; }
        else if (type == TowerType.Laser) { return availableLaserTowers.Count > 0 ? true : false; }
        else if (type == TowerType.Plasma) { return availablePlasmaTowers.Count > 0 ? true : false; }
        else { return false; }
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

                    //   selectedTower = hit.transform.gameObject.GetComponent<Tower>();
                    transformTowerMap.TryGetValue(hit.transform, out selectedTower);
                    if (selectedTower)
                    { selectedTower.isSelected = true; }
                }
                
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedTower) { selectedTower.isSelected = false; }
        }
    }

    public void LookingForSymbiosis()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ResourceManager.DisplayCost(false);
            towerLookingForSymbiosisPartner = null;
            if (highlightedTower) { highlightedTower.isHighlighted = false; }
            highlightedTower = null;
            return;
        }
        if (!towerLookingForSymbiosisPartner)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayerMask))
        {

            if (highlightedTower) { highlightedTower.isHighlighted = false; }

            //   selectedTower = hit.transform.gameObject.GetComponent<Tower>();
            transformTowerMap.TryGetValue(hit.transform, out highlightedTower);
            if (highlightedTower && highlightedTower != towerLookingForSymbiosisPartner && !symbiosisTowers.Contains(highlightedTower))
            {
                Symbiosis.cost = (int)(symbiosisCostMultiplayer * (towerLookingForSymbiosisPartner.transform.position - highlightedTower.transform.position).magnitude);
                ResourceManager.DisplayCost(true, Symbiosis.cost);
                if (Input.GetMouseButtonDown(0))
                {
                    if (ResourceManager.RemoveResource(Symbiosis.cost))
                    {
                        ResourceManager.DisplayCost(false);
                        highlightedTower.isHighlighted = false;
                        towerLookingForSymbiosisPartner.SetSymbiosis(highlightedTower);
                        symbiosisTowers.Add(highlightedTower);
                        symbiosisTowers.Add(towerLookingForSymbiosisPartner);
                        towerLookingForSymbiosisPartner = null;
                    }
                    else
                    {
                        ResourceManager.CostIsTooHighSignal();
                    }

                }
                else
                {
                    highlightedTower.isHighlighted = true;
                }
            }

        }
        else
        {
            ResourceManager.DisplayCost(false);
            if (highlightedTower) { highlightedTower.isHighlighted = false; }
            highlightedTower = null;
        }

    }
    public static void Restart()
    {
        towers.Clear();
        availableElectroTowers.Clear();
        availableLaserTowers.Clear();
        availablePlasmaTowers.Clear();
        transformTowerMap.Clear();
        symbiosisTowers.Clear();

        selectedTower =null;
        highlightedTower = null;
        towerLookingForSymbiosisPartner = null;
    }
}
