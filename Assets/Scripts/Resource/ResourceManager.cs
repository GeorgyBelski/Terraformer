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
    int proceeds;
    bool signOfPreviosProceeds = true;
    public int towersSupply;
    public static bool isTowersSupplyChanged = true;

    [Header("HUD")]
    public Text resourceCounter;
    public Text resourceCostReference;
    public Text resourceProceeds, sigh;
    int previousProceeds;
    public Image resourcefiller;
    public Image incomefiller;

    public static float resource;
    public static float resourceMax;

    public static Text resourceCounterST;
    public static Text resourceCost;
    public static Image resourcefillerST;
    public static Image incomeFilerST;

    public static float RepairCost = 1f;

    [Header("Income")]
    public int startIncomeFromHexagon = 1;
    public static int incomeFromHexagon = 1, income;
    public float startBillingPeriod = 2f;
    public static float billingPeriod = 2f;

    public Color incomeColor, lossColor;

    float timerBillingPeriod;
    [Header("Costs")]
    public int basicTowerSupply = 2;
    public int simbiosisTowerSupply = 1;
    


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

        incomeFromHexagon = startIncomeFromHexagon;
        billingPeriod = startBillingPeriod;

        incomeColor = resourceProceeds.color;
    }

    void Update()
    {
        CulculateTowersSupply();
        CalculateProceeds();
        ShowProceeds();
        ApplyProceeds();
    }
    void CulculateTowersSupply()
    {
        if (!isTowersSupplyChanged)
        { return; }

        //   towersSupply = 0;
        //   TowerManager.towers.ForEach(tower => towersSupply += tower.supply);
        towersSupply = TowerManager.towers.Count * basicTowerSupply + TowerManager.symbiosisTowers.Count * simbiosisTowerSupply;
        isTowersSupplyChanged = false;
    }
    private void ApplyProceeds()
    {
        timerBillingPeriod -= Time.deltaTime;
        if (timerBillingPeriod <= 0)
        {
            AddResource(proceeds);
            timerBillingPeriod = billingPeriod;
        }
    }
    void CalculateProceeds()
    {
        proceeds = ResourceManager.income - towersSupply;
        if (proceeds <= 0 && signOfPreviosProceeds)
        {
            resourceProceeds.color = lossColor;
            sigh.enabled = false;
            signOfPreviosProceeds = false;
        }
        else if(proceeds > 0 && !signOfPreviosProceeds)
        {
            resourceProceeds.color = incomeColor;
            sigh.enabled = true; ;
            signOfPreviosProceeds = true;
        }
    }
    void ShowProceeds()
    {
        if (previousProceeds != proceeds)
        {
            previousProceeds = proceeds;
            resourceProceeds.text = previousProceeds.ToString();
        }
    }

    public static void AddResource(float count)
    {
        if (count < 0)
        {
            RemoveResource(-count);
            return;
        }
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
