using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [Header("Resourses")]
    public float StartResource = 500;
    public float StartMaxresource = 10000;

    [Header("HUD")]
    public Text resourceCounter, resourceCost;
    public Image resourcefiller;
    public Image incomefiller;

    public static float resource;
    public static float resourceMax;

    public static Text resourceCounterST;
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
        resource = StartResource;
        resourceMax = StartMaxresource;

        resourceCounterST = resourceCounter;
        resourcefillerST = resourcefiller;
        incomeFilerST = incomefiller;

        fillResourceFiller();
        resourceCounter.text = StartResource.ToString();
        incomefiller.fillAmount = 0;
    }

    public static void addResource(float count)
    {
        resource += count;
        if (resource > resourceMax)
            resource = resourceMax;
        fillResourceFiller();
    }

    public static void removeResource(float count)
    {
        if (resource - count < 0)
            return;

        resource -= count;
        fillResourceFiller();
    }

    private static void fillResourceFiller()
    {
        //print("+");
        resourcefillerST.fillAmount = resource / resourceMax;
        resourceCounterST.text = resource.ToString();
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
