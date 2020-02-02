using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{

    public static bool isTimeStopped = false;
    public static void Restart() 
    {
        isTimeStopped = false;
        Time.timeScale = 1;
    }

    public void StopGameTime() 
    {
        isTimeStopped = true;
        Time.timeScale = 0.2f;
    }

    
}
