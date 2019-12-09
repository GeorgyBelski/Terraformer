using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terraformer : Tower
{
    [Header("Menu")]
    public GameObject menu;
    public TextMeshProUGUI defeat;
    public static bool isOverclock = false;
    public static bool isVictory = false;
    public static float overclockFactor=0;
    public float overclockSpeed = 20f;
    public Transform terraformerMesh;
    public Transform startOverclockWave;
    public static Transform overclockWave;


    new void Start()
    {
        isOverclock = false;
        isVictory = false;
        menu.SetActive(false);
        defeat.gameObject.SetActive(false);
        TowerManager.terraformer = this;
        TowerManager.AddTower(this);
        overclockWave = startOverclockWave;
        overclockWave.gameObject.SetActive(false);
    }

    void Update()
    {
        Overclock();
        OverclockWave();
    }
    public void Overclock() 
    {
        if (isOverclock)
        { terraformerMesh.eulerAngles += Vector3.up * overclockSpeed * overclockFactor * Time.deltaTime; }
    }
    public void OverclockWave() 
    {
        if (isVictory)
        { 
            overclockWave.gameObject.SetActive(true);
            terraformerMesh.eulerAngles += Vector3.up * overclockSpeed * Time.deltaTime/10;
        }
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
