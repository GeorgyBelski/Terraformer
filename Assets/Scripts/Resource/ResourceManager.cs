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
    public TextMeshProUGUI resourceCounter;
    public TextMeshProUGUI resourceCostReference;
    public TextMeshProUGUI resourceProceeds, sigh;
    int previousProceeds;
    public Image resourcefiller;
    public Image incomefiller;
    public Image OverclockBar;
    public TextMeshProUGUI victory;

    public static float resource;
    public static float resourceMax;

    public static TextMeshProUGUI resourceCounterST;
    public static TextMeshProUGUI resourceCost;
    public static Animator resourceCostAnimator;
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

    public static void Restart() 
    {
        isTowersSupplyChanged = true;
    }

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
        if (resourceCost)
        { resourceCostAnimator = resourceCost.GetComponent<Animator>(); }
        resourcefillerST = resourcefiller;
        incomeFilerST = incomefiller;

        fillResourceFiller();
        resourceCounter.text = StartResource.ToString();
        incomefiller.fillAmount = 0;

        incomeFromHexagon = startIncomeFromHexagon;
        billingPeriod = startBillingPeriod;

        incomeColor = resourceProceeds.faceColor;
        OverclockBar.fillAmount = 0;
        victory.gameObject.SetActive(false);
    }

    void Update()
    {
        CulculateTowersSupply();
        CalculateProceeds();
        ShowProceeds();
        ApplyProceeds();
        OverclockTerraformer();
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
            resourceProceeds.faceColor = lossColor;
            sigh.enabled = false;
            signOfPreviosProceeds = false;
        }
        else if(proceeds > 0 && !signOfPreviosProceeds)
        {
            resourceProceeds.faceColor = incomeColor;
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
        resourceCounterST.text = resource.ToString("F0");
    }

    private static void fillIncomeFiller()
    {

    }

    /*
    public void countIncome()
    {

    }
    */
    public static void DisplayCost( bool enable, int cost = 0)
    {
        if (enable)
        { resourceCost.text = "-" + cost; }
        else
        { resourceCost.text = ""; }
    }

    public static void CostIsTooHighSignal()
    {
        resourceCostAnimator.SetBool("tooHigh", true);
    }
    public static void ExitCostIsTooHighSignal()
    {
        resourceCostAnimator.SetBool("tooHigh", false);
    }

    public void OverclockTerraformer()
    {
        if (resource == resourceMax)
        {
            OverclockBar.fillAmount += proceeds * Time.deltaTime / 400;
            Terraformer.isOverclock = true;
        }
        else if (OverclockBar.fillAmount > 0)
        {
            OverclockBar.fillAmount -= proceeds * Time.deltaTime / 400;
            Terraformer.isOverclock = true;
        }
        else { Terraformer.isOverclock = false; }

        Terraformer.overclockFactor = OverclockBar.fillAmount;
        if (OverclockBar.fillAmount == 1) 
        { ApplyVictory(); }
    }
    public void ApplyVictory()
    {
        //  victory.gameObject.SetActive(true);
        Terraformer.isVictory = true;
        MenuController.ShowVictory(true);
        if (EnemyManagerPro.enemies.Count != 0)
        {
            var enemy = EnemyManagerPro.enemies.ToArray()[0];
            enemy.ApplyDamage(enemy.maxHealth, Vector3.zero, Vector3.zero);
        }
        /*
        foreach (var enemy in EnemyManagerPro.enemies) {
            if (enemy)
            { enemy.ApplyDamage(enemy.maxHealth, Vector3.zero, Vector3.zero); }
        }
        */
    }
}
