using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [Header("Resourses")]
    public float resource;
    public float resourceMax = 10000;

    [Header("HUD")]
    //public TextMeshPro
    public TextMeshProUGUI resourceCounter;
    public Image resourcefiller;
    public Image incomefiller;

    public static float resourceST;
    public static float resourceMaxST;

    public static TextMeshProUGUI resourceCounterST;
    public static Image resourcefillerST;
    public static Image incomeFilerST;

    public static float RepairCost = 1f;
    

    //private static float income;
    //        {
    //        get => resourcefiller2;
    //        set => resourcefiller2 = value;
    //    }
    void Start()
    {
        resourceST = resource;
        resourceMaxST = resourceMax;

        resourceCounterST = resourceCounter;
        resourcefillerST = resourcefiller;
        incomeFilerST = incomefiller;

        fillResourceFiller();
        resourceCounter.text = resource.ToString();
        incomefiller.fillAmount = 0;
    }

    public static void addResource(float count)
    {
        resourceST += count;
        if (resourceST > resourceMaxST)
            resourceST = resourceMaxST;
        fillResourceFiller();
    }

    public static void removeResource(float count)
    {
        if (resourceST - count < 0)
            return;

        resourceST -= count;
        fillResourceFiller();
    }

    private static void fillResourceFiller()
    {
        //print("+");
        resourcefillerST.fillAmount = resourceST / resourceMaxST;
        resourceCounterST.text = resourceST.ToString();
    }

    private static void fillIncomeFiller()
    {

    }
    
    /*
    public void countIncome()
    {

    }
    */
}
