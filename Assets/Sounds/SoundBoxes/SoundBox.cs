using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    public float existTime = 3f;

    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        if (audio)
            audio.pitch = Random.Range(0.7f, 1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        existTime -= Time.deltaTime;
        if (existTime <= 0)
            Destroy(gameObject);
    }
}
