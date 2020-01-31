using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseRaycastState {None, SymbiosisLooking, RepairLooking } 
public class TowerManager : MonoBehaviour
{
    public int availablePlazmaTowersCount;
    public int towersNumber;
    public static MouseRaycastState mouseState = MouseRaycastState.None;

    public static List<Tower> towers = new List<Tower>();

    public static Tower terraformer { get; set; }
   // public Terraformer terraformer

    public static List<Tower> availableElectroTowers = new List<Tower>();
    public static List<Tower> availableLaserTowers = new List<Tower>();
    public static List<Tower> availablePlasmaTowers = new List<Tower>();

    public static List<Tower> availableElectroLaserTowers = new List<Tower>();
    public static List<Tower> availableLaserPlasmaTowers = new List<Tower>();
    public static List<Tower> availableElectroPlasmaTowers = new List<Tower>();
    [SerializeField] int electroPlasmaNumber;
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

    [Header("Sounds")]
    public AudioSource uIAudioSource;
    public List<AudioClip> uISounds;

    public static void Restart()
    {
        towers.Clear();
        availableElectroTowers.Clear();
        availableLaserTowers.Clear();
        availablePlasmaTowers.Clear();

        availableElectroLaserTowers.Clear();
        availableLaserPlasmaTowers.Clear();  
        availableElectroPlasmaTowers.Clear();

        transformTowerMap.Clear();
        symbiosisTowers.Clear();

        selectedTower = null;
        highlightedTower = null;
        towerLookingForSymbiosisPartner = null;
    }
    private void Start()
    {

    }
    private void FixedUpdate()
    {
        LookingForSymbiosis();
        LookingForRepairTower();
    }
    void LateUpdate()
    {
        SelectTower();
        availablePlazmaTowersCount = availablePlasmaTowers.Count;
        towersNumber = towers.Count;
        electroPlasmaNumber = availableElectroPlasmaTowers.Count;
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
        Tower nearest = null;
        //  int nearestTowerIndex = -1;
        // for (int i = 0; i < TowerManager.towers.Count; i++)
        towers.ForEach(tower =>  
        {
            if (tower.type == type && tower.IsCastingAbility == false)
            {
                Vector3 toTarget = target.position - tower.transform.position;
                float distance = toTarget.magnitude;
                if (distance < minDistance) {
                    minDistance = distance;
                    nearest = tower;
                }
            }
        });

        //  return TowerManager.towers[nearestTowerIndex];
        return nearest;
      
    }

    public static Tower GetNeaterstAvailableTower(Transform target, TowerType type1, TowerType? type2 = null)
    {
        float minDistance = float.PositiveInfinity;
        Tower nearest = null;
        List<Tower> list = null;
        if (type2 == null || type2 == TowerType.None)
        {
            if (type1 == TowerType.Electro)
            {
                list = availableElectroTowers;
            }
            else if (type1 == TowerType.Laser)
            {
                list = availableLaserTowers;
            }
            else if (type1 == TowerType.Plasma)
            {
                list = availablePlasmaTowers;
            }


        }
        else if ((type1 == TowerType.Electro && type2 == TowerType.Laser) || (type1 == TowerType.Laser && type2 == TowerType.Electro))
        {
            list = availableElectroLaserTowers;
        }
        else if ((type1 == TowerType.Laser && type2 == TowerType.Plasma) || (type1 == TowerType.Plasma && type2 == TowerType.Laser))
        {
            list = availableLaserPlasmaTowers;
        }
        else if ((type1 == TowerType.Electro && type2 == TowerType.Plasma) || (type1 == TowerType.Plasma && type2 == TowerType.Electro))
        {
            list = availableElectroPlasmaTowers;
        }

            list.ForEach(tower =>
            {
                if (!tower.IsCastingAbility)
                {
                    Vector3 toTarget = target.position - tower.transform.position;
                    float distance = toTarget.magnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = tower;
                    }
                }
            });
        return nearest;
    }
    static float CompareDistance(Transform target, Tower tower,Tower nearest, float minDistance) 
    {
        float distance = float.PositiveInfinity;
        if (!tower.IsCastingAbility)
        {
            Vector3 toTarget = target.position - tower.transform.position;
            distance = toTarget.magnitude;
            if (distance < minDistance)
            {
                nearest = tower;
                return distance;
            }
        }
        return minDistance;
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
    public static void StartLookingSimbiosisPartner(Tower tower) 
    {
        towerLookingForSymbiosisPartner = tower;
        mouseState = MouseRaycastState.SymbiosisLooking;
    }
    public static void StartLookingRepairTower()
    {
        mouseState = MouseRaycastState.RepairLooking;
    }

    public void LookingForSymbiosis()
    {
        
        if (mouseState!= MouseRaycastState.SymbiosisLooking)
        {
            return;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            mouseState = MouseRaycastState.None;
            ResourceManager.DisplayCost(false);
            towerLookingForSymbiosisPartner = null;
            if (highlightedTower) { highlightedTower.isHighlighted = false; }
            highlightedTower = null;
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
                        mouseState = MouseRaycastState.None;
                        ResourceManager.DisplayCost(false);
                        highlightedTower.isHighlighted = false;
                        towerLookingForSymbiosisPartner.SetSymbiosis(highlightedTower);
                        symbiosisTowers.Add(highlightedTower);
                        symbiosisTowers.Add(towerLookingForSymbiosisPartner);
                        towerLookingForSymbiosisPartner = null;

                        uIAudioSource.PlayOneShot(uISounds[1], 0.6f);
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

    public void LookingForRepairTower()
    {
        if (mouseState != MouseRaycastState.RepairLooking)
        {
            return;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            mouseState = MouseRaycastState.None;
            ResourceManager.DisplayCost(false);
            if (highlightedTower) { highlightedTower.isHighlighted = false; }
            highlightedTower = null;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayerMask))
        {

            if (highlightedTower) { highlightedTower.isHighlighted = false; }

            transformTowerMap.TryGetValue(hit.transform, out highlightedTower);
            if (highlightedTower)
            {
                int towerRepairCost = (int)highlightedTower.towerHealth.CalculateRepairCost();
                ResourceManager.DisplayCost(true, towerRepairCost);
                if (Input.GetMouseButtonDown(0))
                {
                    if (ResourceManager.RemoveResource(towerRepairCost))
                    {
                        mouseState = MouseRaycastState.None;
                        ResourceManager.DisplayCost(false);
                        RepairButton.isActive = false;
                        highlightedTower.isHighlighted = false;
                        highlightedTower.towerHealth.Repair();

                        uIAudioSource.PlayOneShot(uISounds[0], 0.6f);
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

}
