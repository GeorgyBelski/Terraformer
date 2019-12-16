﻿using System.Collections;
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
    public Transform ring;
    Vector3 ringStartPosition;
    Vector3 RingRotationY;
    float previousRingOffset = 0f;
    public Transform startOverclockWave;
    public static Transform overclockWave;

    private Material mt;
    private Color baseEmissionColor;

    new void Start()
    {
 
        mt = terraformerMesh.GetComponent<Renderer>().material;
        baseEmissionColor = mt.GetColor("_EmissionColor");
        isOverclock = false;
        isVictory = false;
        ringStartPosition = ring.position;
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
        { 
            RingRotationY += Vector3.up * overclockSpeed * overclockFactor * Time.deltaTime;
            ring.eulerAngles = RingRotationY + Vector3.forward * overclockFactor * 95;
            float positionOffset = 0f;
            float positionFactor;
            previousRingOffset = ring.position.y - ringStartPosition.y;
            positionFactor = 3 * 1 / (1 + overclockFactor*2);
            ring.position = Vector3.up * (positionFactor * overclockFactor + positionOffset)+ ringStartPosition;
        }
    }
    public void OverclockWave() 
    {
        if (isVictory)
        { 
            overclockWave.gameObject.SetActive(true);
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
