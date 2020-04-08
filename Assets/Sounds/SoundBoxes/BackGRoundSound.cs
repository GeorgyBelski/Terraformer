using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGRoundSound : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> sounds;


    // Start is called before the first frame update
    void Start()
    {
        //audioSource.PlayOneShot(sounds[Random.Range(0, 2)], 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!audioSource.isPlaying)
        {
               audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)], 0.3f);
        }
    }
}
