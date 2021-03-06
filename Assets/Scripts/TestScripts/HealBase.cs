﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBase : MonoBehaviour
{
    
    [Header("Heal setup")]
    public float healpower = 50f;
    public float radius = 3f;

    //[Header("Main setup")]
    public float liveTime = 20f;
    private float realLiveTime;

    public float healRate = 2f;
    private float realHealRate;

    public Transform StartPoint;
    public GameObject cast;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip castSound;

    void Start()
    {
     //   EnemyManagerPro.AddEnemy(this.GetComponent<Enemy>());
        HealCast.healPower = healpower;
        HealCast.diameter = radius*2;
        realHealRate = healRate;
    }

    // Update is called once per frame
    void Update()
    {
        realHealRate -= Time.deltaTime;
        liveTime -= Time.deltaTime;
        if(realHealRate <= 0)
        {
            Instantiate(cast, StartPoint.position, StartPoint.rotation, null);

            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(castSound, 0.4f);
            //castgm.GetComponent<HealCast>().
            realHealRate = healRate;
        }

        if(liveTime <= 0)
        {
            EnemyManagerPro.RemoveEnemy(this.GetComponent<Enemy>());
            Destroy(gameObject);
        }

    }
}
