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

    public SpawnEnemiesPattern pattern;
    
    private GameObject leader;
    private GameObject next;
    
    [Header("Loading Line")]
    public LineRenderer loadingLine;
    [ColorUsageAttribute(true, true)]
    public Color loadingLineColor;
    public float lineSpeed = 2f;

    private float realLineSpeed;

    float previousLineSpeed;
    float originalSpeed;
    public Material loadingLineMaterial;

    private float startSizeInSecond;
    private bool active = false;
    private bool spwning = false;

    public float spawnRate = 1f;
    private float realSpawnRate = 1f;

    private int enemieCountInList = 0;

    private int columns;
    private int columnCount;
    private float columnsRange;
    private float rotation;
    private Sqad.Formation form;

    private bool isSquad = false;

    public float deactivateTime = 1.5f;
    private float deactivateTimeLeft;
    public bool deactivating = false;
    //private loadingLineMaterial;

    [Header("Sounds")]
    public AudioSource audioSource;
    public List<AudioClip> sounds;

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
            //print(realLineSpeed);
            //realLineSpeed = lineSpeed * transform.localScale.x / finalSize;
            //loadingLineMaterial.SetFloat("_Speed", realLineSpeed);
            transform.localScale = new Vector3(transform.localScale.x + sizeInSecond * Time.deltaTime, transform.localScale.y + sizeInSecond * Time.deltaTime, transform.localScale.z + sizeInSecond * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                //audioSource.pitch = transform.localScale.x / finalSize;
                audioSource.PlayOneShot(sounds[0], 0.6f);
            }

            //print(transform.localScale.x + " " + size);
            if (transform.localScale.x >= finalSize && active)
            {
                //print(sizeInSecond);
                sizeInSecond = 0;
                active = false;
                //gameObject.active = false;
                spwning = true;
                enemieCountInList = 0;
                //spawnEnemyes();
                //transform.localScale = Vector3.zero;
                //transform.position = Vector3.zero;
                //this.enabled = false;
                //gameObject. = false

            }

            if (spwning)
            {
                if (realSpawnRate <= 0){
                    pattern.spawnEnemyes();
                    audioSource.PlayOneShot(sounds[1], 1f);
                    realSpawnRate = spawnRate;
                }
                realSpawnRate -= Time.deltaTime;
            }
        }

        if (deactivating)
        {
            deactivateTimeLeft -= Time.deltaTime;
            deactivate();
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

    public void deactivate()
    {
        deactivating = true;

        //deactivateTimeLeft -= Time.deltaTime;
        if (deactivateTimeLeft > 0)
        {
            return;
        }
        //print("+");
        //if(audioSource.isPlaying)
        spwning = false;
        transform.localScale = Vector3.zero;
        transform.position = Vector3.zero;
        this.enabled = false;
        gameObject.SetActive(false);
        realSpawnRate = spawnRate;
        sizeInSecond = startSizeInSecond;
        deactivating = false;
        deactivateTimeLeft = deactivateTime;
    }

    /*public void spawnEnemyes()
    {
        if(realSpawnRate <= 0)
        {
            //print("+");
            Instantiate(enemyList[enemieCountInList], new Vector3(
                this.transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / enemyList.Count - 1))) * 2),
                0,
                this.transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / enemyList.Count - 1))) * 2)), this.transform.rotation);
            realSpawnRate = spawnRate;
            enemieCountInList++;
        }

        if (enemyList.Count <= enemieCountInList)
        {
            spwning = false;
            transform.localScale = Vector3.zero;
            transform.position = Vector3.zero;
            this.enabled = false;
            gameObject.SetActive(false);
            realSpawnRate = spawnRate;
            sizeInSecond = startSizeInSecond;

        }
        
        //print(enemyList.Count);
        //engl = SquadFormationSquare.DegreeToRadian(engl);
        //leader = enemyList[0];
        //Instantiate(leader, transform.position, this.transform.rotation);
        //for (int i = 1; i < enemyList.Count - 1; i++)
        //{

        //}
        
    }
    */

    /*private void spawnSquad()
    {
        switch(form) {
            case Sqad.Formation.Square:
                new SquadFormationSquare(enemyList[0], enemyList[1], columns, columnCount, columnsRange, 35, rotation);
                break;
            case Sqad.Formation.Circle:
                new SquadFormationCircle(enemyList[0], enemyList[1], columns, columnCount, columnsRange, 35, rotation);
                break;
        }
        spwning = false;
        transform.localScale = Vector3.zero;
        transform.position = Vector3.zero;
        this.enabled = false;
        gameObject.SetActive(false);
        realSpawnRate = spawnRate;
        sizeInSecond = startSizeInSecond;

    }
    */

    public void setSettings(List<GameObject> list, float spawnRate)
    {
        deactivateTimeLeft = deactivateTime;
        this.spawnRate = spawnRate;
        realSpawnRate = spawnRate;
        startSizeInSecond = sizeInSecond;
        enemyList = list;

        active = true;
        this.enabled = true;
        isSquad = false;

        ReloadLine();
    }

    public void setSettings(List<GameObject> list, int columns, int columnCount, float columnsRange, float rotation, Sqad.Formation form)
    {
        deactivateTimeLeft = deactivateTime;
        realSpawnRate = spawnRate;
        startSizeInSecond = sizeInSecond;
        enemyList = list;

        this.columns = columns;
        this.columnCount = columnCount;
        this.columnsRange = columnsRange;
        this.form = form;
        this.rotation = rotation;

        isSquad = true;

        active = true;
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
