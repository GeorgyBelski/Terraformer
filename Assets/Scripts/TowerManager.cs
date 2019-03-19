using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<Tower> startTowers = new List<Tower>();
    public static List<Tower> towers = new List<Tower>();

    private void Start()
    {
        foreach (Tower startTower in startTowers)
        {
            towers.Add(startTower);
        }
    }
}
