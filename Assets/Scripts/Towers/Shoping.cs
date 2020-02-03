using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using static CreepHexagonGenerator;

public class Shoping : MonoBehaviour
{
    private Color selectedTowerColor;
    [Header("Tower Build Place Prefab")]
    public GameObject towerPlace;

    [Header("Towers Prefabs")]
    public GameObject electroTower;
    public float electroTowerPrice;
    public GameObject laserTower;
    public float lazerTowerPrice;
    public GameObject plasmaTower;
    public float plasmaTowerPrice;

    public TextMeshProUGUI uiElectroTowerPrice;
    public TextMeshProUGUI uiLaserTowerPrice;

    [Header("Buttons")]
    public List<Button> buttons;

    float camRayLength = 60f;
    //public ResourceManager resManager;
  //  public int floorMask;


    private bool selectedElectroBool = false;
    private bool selectedLazerBool = false;
    private bool isPlacing = false;
    private GameObject realTimeTowerPlace;
    TowerPlacing towerPlacing;
    bool isAbleToBuild;
    int creep_GroundMask = CreepHexagonGenerator.creepLayerMask | Globals.groundLayerMask;
    Hexagon hexagon;
    private GameObject selectedTower;
    private float currPrice;

    private ColorBlock defaultColor;
    private Button bSelected;
    private Material mt;
    Color transparentRed, transparentGreen;

    [Header("Sounds")]
    public AudioSource uIAudioSource;
    public List<AudioClip> uISounds;

    //public NavMeshAgent agent;

    void Start()
    {
        uiElectroTowerPrice.text = electroTowerPrice.ToString();
        uiLaserTowerPrice.text = lazerTowerPrice.ToString();
    //    floorMask = LayerMask.GetMask("Ground");
        //defaultColor = 
        selectedTowerColor = Color.green;
        //agent.updateRotation = false;
        transparentRed = new Color(1, 0, 0, 0.5f);
        transparentGreen = new Color(0, 1, 0, 0.5f);
    }

    public void SelectTower(Button b)
    {
        //uIAudioSource.pitch = //Random.Range(0.1f, 1.5f);
        uIAudioSource.PlayOneShot(uISounds[0], 0.6f);
        isPlacing = true;
        selectB(b);
        if (!realTimeTowerPlace)
        {
            realTimeTowerPlace = Instantiate(towerPlace);
            mt = realTimeTowerPlace.gameObject.GetComponent<Renderer>().material;
            towerPlacing = realTimeTowerPlace.GetComponent<TowerPlacing>();
        }
        else
        { realTimeTowerPlace.SetActive(true); }
        
    }

    public void SelectElectroTower(Button b)
    {
     //   if (isPlacing)
     //       Cancel();

        selectedTower = electroTower;
        currPrice = electroTowerPrice;
        SelectTower(b);
    }

    public void SelectLazerTower(Button b)
    {
     //   if (isPlacing)
     //       Cancel();

        selectedTower = laserTower;
        currPrice = lazerTowerPrice;
        SelectTower(b);
    }

    public void SelectPlasmaTower(Button b)
    {
     //   if (isPlacing)
     //       Cancel();

        selectedTower = plasmaTower;
        currPrice = plasmaTowerPrice;
        SelectTower(b);
    }

    private void selectB(Button b)
    {
        if (isPlacing) { 
            b.Select();
        /*  bSelected = b;
            defaultColor = b.colors;
            ColorBlock cb = b.colors;// = selectedTowerColor;
            cb.normalColor = selectedTowerColor;
            cb.highlightedColor = selectedTowerColor;
            b.colors = cb;
        */    
            //selectedElectroBool = true;
            //isPlacing = true;
        }
        else
        {
          /*  bSelected = null;
            b.colors = defaultColor;
            Destroy(realTimeTowerPlace);*/
        }
    }

    private void placeTower()
    {
        uIAudioSource.PlayOneShot(uISounds[3], 0.5f);
        Instantiate(selectedTower, realTimeTowerPlace.transform.position, realTimeTowerPlace.transform.rotation);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isPlacing == true)
        {
            Cancel();
            uIAudioSource.PlayOneShot(uISounds[1], 0.6f);
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) && buttons.Count >= 1)
        {
            SelectElectroTower(buttons[0]);
        }
            

        if (Input.GetKeyUp(KeyCode.Alpha2) && buttons.Count >= 2)
        {
            SelectLazerTower(buttons[1]);
        }


        if (Input.GetKeyUp(KeyCode.Alpha3) && buttons.Count >= 3)
        {
            SelectPlasmaTower(buttons[2]);
        }


        if (isPlacing)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, creep_GroundMask))
            {
                isAbleToBuild = false;
                realTimeTowerPlace.transform.position = floorHit.point;
                if (floorHit.collider.gameObject.layer == CreepHexagonGenerator.creepLayer)
                {
                    GameObject hexagonGameObject = floorHit.collider.gameObject;
                    if (CreepHexagonGenerator.meshHexagonMap.TryGetValue(hexagonGameObject, out hexagon))
                    {
                        if (hexagon.GetStatus() == HexCoordinatStatus.Attend)
                        {
                            realTimeTowerPlace.transform.position = floorHit.transform.position;
                            isAbleToBuild = true;
                        }
                    }

                }

                if (!isAbleToBuild || ResourceManager.resource < currPrice)
                {
                    mt.SetColor("_BaseColor", transparentRed);
                }
                else
                {
                    mt.SetColor("_BaseColor", transparentGreen);
                }

                if(Input.GetMouseButtonDown(0) && (!isAbleToBuild || ResourceManager.resource <= currPrice))
                    uIAudioSource.PlayOneShot(uISounds[2], 0.6f);

                if (Input.GetMouseButtonDown(0) && isAbleToBuild && ResourceManager.resource >= currPrice)
                {
                    ResourceManager.RemoveResource(currPrice);
                    placeTower();
                    Cancel();
                }
                else if (ResourceManager.resource < currPrice)
                {
                    ResourceManager.CostIsTooHighSignal();
                }
            }
        }     
    }

    public void Cancel()
    {
 
        isPlacing = false;
        selectB(bSelected);
        realTimeTowerPlace.SetActive(false);
    }

}
