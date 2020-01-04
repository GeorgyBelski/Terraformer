using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RepairButton : MonoBehaviour
{
    public static bool isActive = false;
    public Image repairImage;
    public Button thisButton;

    [Header("Sounds")]
    public AudioSource uIAudioSource;
    public List<AudioClip> uISounds;

    // Start is called before the first frame update

    public void switchOnRepair()
    {
        thisButton.Select();
        uIAudioSource.PlayOneShot(uISounds[0], 0.6f);
        isActive = true;
        repairImage.color = thisButton.colors.selectedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
            switchOnRepair();

        if (gameObject != EventSystem.current.currentSelectedGameObject && isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //isActive = true;
                thisButton.Select();
            }
            else
            {
                repairImage.color = thisButton.colors.normalColor;
                isActive = false;
            }
          
        }

            
        /*
        if (isActive && Input.GetMouseButtonDown(1))
        {
            
            //uIAudioSource.PlayOneShot(uISounds[1], 0.6f);
            isActive = false;

            //print("-");
        }
        */
    }
}
