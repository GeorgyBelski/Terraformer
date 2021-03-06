﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terraformer : Tower
{
    [Header("Menu")]
    //public GameObject menu;
    //   public TextMeshProUGUI defeat;
    public static Terraformer terraformer;
    public static bool isOverclock = false;
    public static bool isVictoryWave = false;
    public static float overclockFactor=0;
    public float overclockSpeed = 20f;
    public Transform terraformerMesh;
    public Transform ring;

    Vector3 ringStartPosition;
    Vector3 RingRotationY;
    float previousRingOffset = 0f;
    public Transform startOverclockWave;
    public static Transform overclockWave;

    //public GameObject ringObject;
    private bool playFinalSound = true;

    private Material mt;
    private Color baseEmissionColor;
    private bool finalSoundActive = true;
    private float prevOverclockFactor = 0;

    [Header("Ui Sound")]
    public AudioSource uiSource;
    public AudioClip emergencySound;

    new void Start()
    {
        terraformer = this;
        mt = ring.GetComponent<Renderer>().material;
        baseEmissionColor = mt.GetColor("_EmissionColor");
        isOverclock = false;
        isVictoryWave = false;
        ringStartPosition = ring.position;
    //    menu.SetActive(false);
     //   defeat.gameObject.SetActive(false);
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

            float emission = Mathf.PingPong(Time.time * 3 * overclockFactor, 200f * overclockFactor) + 2f;
            //print(overclockFactor);
            Color finalColor = baseEmissionColor * Mathf.LinearToGammaSpace(emission);

            mt.SetColor("_EmissionColor", finalColor);
            if (overclockFactor == 0)
                mt.SetColor("_EmissionColor", baseEmissionColor);
            //print(overclockFactor + " " + overclockSpeed);
            audioSource.pitch = overclockFactor;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(abilitiesSounds[0], 2.5f);
            }
            
        }
    }
    public void OverclockWave() 
    {
        if (!finalSoundActive && prevOverclockFactor > overclockFactor)
        {
            audioSource.Stop();
            finalSoundActive = true;
        }
        if(overclockFactor > 0.5f && finalSoundActive)
        {
            finalSoundActive = false;
            audioSource.PlayOneShot(abilitiesSounds[2], 2.5f);
        }


            

        if (isVictoryWave)
        {
            if (playFinalSound)
            {
                //audioSource.Stop();
                audioSource.pitch = 1;
                audioSource.PlayOneShot(abilitiesSounds[1], 2f);
                playFinalSound = false;
            }
            
            overclockWave.gameObject.SetActive(true);
            //isVictory = false;
        }
        prevOverclockFactor = overclockFactor;
    }
    public void IntermediateVictory() 
    {
        towerHealth.health = towerHealth.maxHealth;
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
    public override void DestroyBulletsAndAbilities()
    {
    }
    public void playEmergency()
    {
        //audioSource.Stop();
        uiSource.pitch = 1;
        uiSource.PlayOneShot(emergencySound, 1f);
    }

    
}
