using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpurtWaveDeactivator : MonoBehaviour
{
    public Animator animator;

    public void StopShowSpurtWave()
    {
        animator.SetBool("show", false);
        this.gameObject.SetActive(false);
    }
}
