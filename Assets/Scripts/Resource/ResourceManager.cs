using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public float resource;
    public float resourceMax = 10000;

    public Text resourceCounter;


    public Image resourcefiller
    {
        get { return resourcefiller2; }
        set { resourcefiller2 = value; }
    }

//        {
//        get => resourcefiller2;
//        set => resourcefiller2 = value;
//    }

    public static Image resourcefiller2;

    public Image incomefiller
    {
        get => incomeFilerST;
        set => incomeFilerST = value;
    }

    public static Image incomeFilerST;
    private static float income;



    void Start()
    {
        fillResourceFiller();
        resourceCounter.text = resource.ToString();
        incomefiller.fillAmount = 0;
    }

    public void addResource(float count)
    {
        resource += count;
        if (resource > resourceMax)
            resource = resourceMax;
        fillResourceFiller();
    }

    public void removeResource(float count)
    {
        if (resource - count < 0)
            return;

        resource -= count;
        fillResourceFiller();
    }

    private void fillResourceFiller()
    {
        //print("+");
        resourcefiller2.fillAmount = resource / resourceMax;
        resourceCounter.text = resource.ToString();
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
