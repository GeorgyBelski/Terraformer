using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpurtFXController : MonoBehaviour
{
    public GameObject spurtWavePrefub;
    public Animator spurtWaveAnimator;
    GameObject spurtWave;
    Vector3 hight = Vector3.up * 1f;
    Vector3 spurtVector;
    float spurtWaveLenbgth =4f;
    void Start()
    {
        spurtWave = Instantiate(spurtWavePrefub, this.transform.position + hight, this.transform.rotation);
        spurtWave.SetActive(false);
        spurtWaveAnimator = spurtWave.GetComponent<Animator>();
    }


    public void ShowSpurtWave(Vector3 destination)
    {
        
        spurtWave.SetActive(true);
        spurtWaveAnimator.SetBool("show", true);
        spurtWave.transform.position = this.transform.position + hight;
        spurtWave.transform.rotation = this.transform.rotation;

        spurtVector = destination - this.transform.position;
        spurtWaveLenbgth = spurtVector.magnitude / 4;
        spurtWave.transform.localScale = new Vector3(1,1, spurtWaveLenbgth);
    }

    public void Destroy()
    {
        Destroy(spurtWave);
    }
}
