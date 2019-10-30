using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ResourceManager : MonoBehaviour
{
    [Header("Resourses")]
    public float StartResource = 500;
    public float StartMaxresource = 10000;

    [Header("HUD")]
    public Text resourceCounter;
    public Text resourceCostReference;
    public Text resourceIncome;
    int previousIncome;
    public Image resourcefiller;
    public Image incomefiller;

    public static float resource;
    public static float resourceMax;

    public static Text resourceCounterST;
    public static Text resourceCost;
    public static Image resourcefillerST;
    public static Image incomeFilerST;

    public static float RepairCost = 1f;

    [Header("INcome")]
    public float incomePeriod = 1;
    float timerIncomePeriod;

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
        resourceCost = resourceCostReference;
        resourcefillerST = resourcefiller;
        incomeFilerST = incomefiller;

        fillResourceFiller();
        resourceCounter.text = StartResource.ToString();
        incomefiller.fillAmount = 0;
    }

    void Update()
    {
        Income();
        ShowIncome();
    }

    private void Income()
    {
        timerIncomePeriod -= Time.deltaTime;
        if (timerIncomePeriod <= 0)
        {
            AddResource(CreepHexagonGenerator.creepHexagonGenerator.income);
            timerIncomePeriod = incomePeriod;
        }
    }
    void ShowIncome()
    {
        if (previousIncome != CreepHexagonGenerator.creepHexagonGenerator.income)
        {
            previousIncome = CreepHexagonGenerator.creepHexagonGenerator.income;
            resourceIncome.text = previousIncome.ToString();
        }
    }

    public static void AddResource(float count)
    {
        resource += count;
        if (resource > resourceMax)
        { resource = resourceMax;}  
        fillResourceFiller();
    }

    public static bool RemoveResource(float count)
    {
        if (resource - count < 0)
        { return false; }

        resource -= count;
        fillResourceFiller();
        return true;
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
