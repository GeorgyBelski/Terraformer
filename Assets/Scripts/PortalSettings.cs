using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSettings : MonoBehaviour
{
    private List<GameObject> enemyList = new List<GameObject>();

    public float loadingTime = 7;
    public float finalSize = 5;
    public float deactivateTime = 1.5f;

    public SpawnEnemiesPattern pattern;
    public bool deactivating = false;

    private SpawnEnemiesPattern.WaveType patternType;
    private Sqad.Formation formation;

    float previousLoadingTime;
    float timerLoading;
    float sizeInSecond;
    float previousFinalSize;
    Vector3 previousPosition;
    float startScale;
    private float startSizeInSecond;

    [Header("Loading Line")]
    public LineRenderer loadingLine;
    [ColorUsageAttribute(true, true)]
    public Color loadingLineColor;
    public float lineSpeed = 2f;

    private float realLineSpeed;

    float previousLineSpeed;
    float originalSpeed;
    public Material loadingLineMaterial;


    private bool active = false;
    private bool spwning = false;
    private GameObject leader;
    private GameObject next;
    public float spawnRate = 1f;
    private float realSpawnRate = 1f;
    private float delay;
    private int enemieCountInList = 0;
    private int columns;
    private int columnCount;
    private float columnsRange;
    private float rotation;
    private Sqad.Formation form;
    private bool isSquad = false;
    private float deactivateTimeLeft;

    //private loadingLineMaterial;

    [Header("Sounds")]
    public AudioSource audioSource;
    public List<AudioClip> sounds;
    public AudioClip activatingSound;

    public AudioSource activatingAudioSource;
    private bool activating = false;

    private void Awake()
    {
        //audioSource.PlayOneShot(activatingSound, 0.5f);
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
        delay -= Time.deltaTime;

        if (delay > 0)
            return;

        if (activating)
        {
            activatingAudioSource.PlayOneShot(activatingSound, 0.7f);
            ReloadLine();
            loadingLine.enabled = true;
            activating = false;
        }
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
            audioSource.volume = transform.localScale.x / finalSize;
            //print(transform.localScale.x + " " + size);
            if (transform.localScale.x >= finalSize && active)
            {
                sizeInSecond = 0;
                active = false;
                spwning = true;
                enemieCountInList = 0;

            }

            if (spwning)
            {
                if (realSpawnRate <= 0){
                    spawnEnemyes();
                    audioSource.PlayOneShot(sounds[1], 0.5f);
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

    public void setSettings(List<GameObject> list, float spawnRate, float delay, float portalLoadTime, float portalFinzlSize)
    {
        print("+"); 
        deactivateTimeLeft = deactivateTime;
        this.spawnRate = spawnRate;
        this.delay = delay;
        this.loadingTime = portalLoadTime;
        this.finalSize = portalFinzlSize;
        patternType = SpawnEnemiesPattern.WaveType.Simple;

        realSpawnRate = spawnRate;
        startSizeInSecond = sizeInSecond;
        enemyList = list;
        transform.localScale = Vector3.zero;
        loadingLine.enabled = false;

        active = true;
        this.enabled = true;
        isSquad = false;
        activating = true;


        //audioSource.PlayOneShot(activatingSound, 0.5f);
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
        activating = true;
        active = true;
        this.enabled = true;

        ReloadLine();

        //audioSource.PlayOneShot(activatingSound, 0.5f);
    }

    public void spawnEnemyes()
    {
        switch (patternType)
        {
            case SpawnEnemiesPattern.WaveType.Simple:
                simpleSpawn();
                break;
            case SpawnEnemiesPattern.WaveType.Squad:
                squadSpawn();
                break;
        }

    }

    private void simpleSpawn()
    {

        if (deactivating)
            return;

        /*
         * Enemy enem = Instantiate(enemyList[enemieCountInList], new Vector3(
            transform.position.x + (Mathf.Sin(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / enemyList.Count - 1))) * 2),
            0,
            transform.position.z + (Mathf.Cos(SquadFormationSquare.DegreeToRadian(enemieCountInList * (360 / enemyList.Count - 1))) * 2)), transform.rotation)
            .GetComponent<Enemy>();
            */
        Enemy enem = Instantiate(enemyList[enemieCountInList], new Vector3(
        transform.position.x,
        0,
        transform.position.z), transform.rotation)
        .GetComponent<Enemy>();

        //enem.maxHealth = enem.maxHealth + 100 * wave;
        //enem.health = enem.maxHealth;

        enemieCountInList++;

        if (enemyList.Count <= enemieCountInList)
        {
            //enemieCountInList = 0;

            deactivate();
            //print("+");
            //return;

        }


    }

    private void squadSpawn()
    {
        if (deactivating)
            return;
        switch (formation)
        {
            case Sqad.Formation.Square:
                //new SquadFormationSquare(list[0], list[1], columns, columnCount, columnsRange, realPortalRange, portalPosition);
                break;
            case Sqad.Formation.Circle:
                //new SquadFormationCircle(list[0], list[1], columns, columnCount, columnsRange, realPortalRange, portalPosition);
                break;
        }
        deactivate();
    }

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
