using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSettings : MonoBehaviour
{
    private List<GameObject> enemyList = new List<GameObject>();

    public float loadingTime = 7;
    float previousLoadingTime;
    float timerLoading;
    float sizeInSecond;
    public float finalSize = 5;
    float previousFinalSize;
    Vector3 previousPosition;
    float startScale;
    
    private GameObject leader;
    private GameObject next;
    
    [Header("Loading Line")]
    public LineRenderer loadingLine;
    [ColorUsageAttribute(true, true)]
    public Color loadingLineColor;
    public float lineSpeed = 2f;
    float previousLineSpeed;
    float originalSpeed;
    public Material loadingLineMaterial;

    private bool spawn = false;
    

    private void Awake()
    {
        startScale = transform.localScale.x;
        sizeInSecond = (finalSize - startScale) / loadingTime;
        previousLoadingTime = loadingTime;
        loadingLine.positionCount = 72;
        loadingLineMaterial = loadingLine.materials[0];
        loadingLineColor = loadingLineMaterial.GetColor("_Color");
        lineSpeed = Mathf.Max(2 + (10 - loadingTime) / 2.5f, 1);
        loadingLineMaterial.SetFloat("_Speed", lineSpeed);
        previousLineSpeed = lineSpeed;
        originalSpeed = lineSpeed;
        
        ReloadLine();
    }

    private void LateUpdate()
    {
    //    SetLoadingLinePosition();
        ShowFinalSize();
    }
    void Update()
    {
        //print("+"); 
        CalculateLoadingSpeed();
        ChangeLineSpeed();
        if (this.enabled)
        {
            transform.localScale = new Vector3(transform.localScale.x + sizeInSecond * Time.deltaTime, transform.localScale.y + sizeInSecond * Time.deltaTime, transform.localScale.z + sizeInSecond * Time.deltaTime);
        
        //print(transform.localScale.x + " " + size);
            if(transform.localScale.x >= finalSize && spawn)
            {
                spawn = false;
                //gameObject.active = false;
                spawnEnemyes();
                transform.localScale = Vector3.zero;
                transform.position = Vector3.zero;
                this.enabled = false;
                //gameObject. = false

            }
        }
    }
    void CalculateLoadingSpeed() 
    {
        if (previousLoadingTime != loadingTime) 
        {
            sizeInSecond = (finalSize - startScale) / loadingTime;
            previousLoadingTime = loadingTime;
        }
    }
    public void spawnEnemyes()
    {
        //print(enemyList.Count);
        //engl = SquadFormationSquare.DegreeToRadian(engl);
        leader = enemyList[0];
        Instantiate(leader, transform.position, this.transform.rotation);
        for (int i = 1; i < enemyList.Count - 1; i++)
        {
            Instantiate(enemyList[i], new Vector3(
                this.transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(i * (360 / enemyList.Count - 1))) * 1 * enemyList.Count/5),
                0,
                this.transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(i * (360 / enemyList.Count - 1))) * 1 * enemyList.Count / 5)), leader.transform.rotation);
        }
        gameObject.active = false;
    }

    public void setSettings(List<GameObject> list)
    {
        enemyList = list;
        //size = list.Count;
        //print(size);
        spawn = true;
        this.enabled = true;

        ReloadLine();
    }

    // Loading Line

    void ShowFinalSize()
    {

        if (previousFinalSize != finalSize)
        {
            Vector3 compass = finalSize / 2 * Vector3.forward;
            for (int i = 0; i < 72; i++)
            {
                Vector3 circlPoint = transform.position + compass;
                circlPoint.y += 0.1f;
                loadingLine.SetPosition(i, circlPoint);
                compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the compass vector around axis on 10 degrees.
            }
            previousFinalSize = finalSize;
            previousPosition = transform.position;
        }
        else 
        {
            SetLoadingLinePosition();
        }
        timerLoading -= Time.deltaTime;
      //  ChangeLineSpeed();
    }
    void SetLoadingLinePosition() 
    {
        if (previousPosition != transform.position)
        {
            for (int i = 0; i < 72; i++)
            {
                loadingLine.SetPosition(i, loadingLine.GetPosition(i) + (transform.position - previousPosition));
            }
            previousPosition = transform.localPosition;
        }
    }
    void ChangeLineSpeed()
    {
        if (previousLineSpeed != lineSpeed) 
        {
            loadingLineMaterial.SetFloat("_Speed", lineSpeed);
            previousLineSpeed = lineSpeed;
            originalSpeed = lineSpeed;
        }
    }
    void ReloadLine()
    {
        lineSpeed = originalSpeed;
        loadingLineMaterial.SetColor("_Color", loadingLineColor);
        timerLoading = loadingTime;
    }

}
