using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Shoping : MonoBehaviour
{
    private Color selectedTowerColor;
    [Header("Tower Build Place Prefab")]
    public GameObject towerPlace;

    [Header("Towers Prefabs")]
    public GameObject electroTower;
    public float electroTowerPrice;
    public GameObject lazerTower;
    public float lazerTowerPrice;

    public Text uiElectroTowerPrice;
    public Text uiLazerTowerPrice;

    [Header("Buttons")]
    public List<Button> buttons;

    float camRayLength = 60f;
    //public ResourceManager resManager;
    public int floorMask;


    private bool selectedElectroBool = false;
    private bool selectedLazerBool = false;
    private bool isPlacing = false;
    private GameObject realTimeTowerPlace;
    private GameObject selectedTower;
    private float currPrice;

    private ColorBlock defaultColor;
    private Button bSelected;
    private Material mt;

    //public NavMeshAgent agent;

    void Start()
    {
        uiElectroTowerPrice.text = electroTowerPrice.ToString();
        uiLazerTowerPrice.text = lazerTowerPrice.ToString();
        floorMask = LayerMask.GetMask("Ground");
        //defaultColor = 
        selectedTowerColor = Color.green;
        //agent.updateRotation = false;
    }
    public void SelectElectroTower(Button b)
    {
        if (isPlacing)
            Cancel();

        isPlacing = true;
        selectedTower = electroTower;
        currPrice = electroTowerPrice;
        selectB(b);
        realTimeTowerPlace = Instantiate(towerPlace, Vector3.zero, this.transform.rotation);
        mt = realTimeTowerPlace.gameObject.GetComponent<Renderer>().material;
    }

    public void SelectLazerTower(Button b)
    {
        if (isPlacing)
            Cancel();

        isPlacing = true;
        selectedTower = lazerTower;
        currPrice = lazerTowerPrice;
        selectB(b);
        realTimeTowerPlace = Instantiate(towerPlace, Vector3.zero, this.transform.rotation);
        mt = realTimeTowerPlace.gameObject.GetComponent<Renderer>().material;

        //Debug.Log("Lazer");
    }

    private void selectB(Button b)
    {
        if (isPlacing) { 
            bSelected = b;
            defaultColor = b.colors;
            ColorBlock cb = b.colors;// = selectedTowerColor;
            cb.normalColor = selectedTowerColor;
            cb.highlightedColor = selectedTowerColor;
            b.colors = cb;
            //selectedElectroBool = true;
            //isPlacing = true;
        }
        else
        {
            bSelected = null;
            b.colors = defaultColor;
            Destroy(realTimeTowerPlace);
        }
    }

    private void placeTower()
    {
        Instantiate(selectedTower, realTimeTowerPlace.transform.position, realTimeTowerPlace.transform.rotation);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            SelectElectroTower(buttons[0]);

        if (Input.GetKeyUp(KeyCode.Alpha2))
            SelectLazerTower(buttons[1]);

        if (isPlacing)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {

                realTimeTowerPlace.transform.position = new Vector3(System.Convert.ToInt32(floorHit.point.x), floorHit.point.y + realTimeTowerPlace.transform.localScale.y - 0.3f, System.Convert.ToInt32(floorHit.point.z));
                if (realTimeTowerPlace.GetComponent<TowerPlacing>().isOnTower || ResourceManager.resource < currPrice)
                {
                    mt.color = new Color(1, 0, 0, 0.5f);
                }
                else
                {
                    mt.color = new Color(0, 1, 0, 0.5f);
                }
               
                if (Input.GetMouseButtonDown(0) && !realTimeTowerPlace.GetComponent<TowerPlacing>().isOnTower && ResourceManager.resource >= currPrice)
                {
                    ResourceManager.removeResource(currPrice);
                    placeTower();
                    isPlacing = false;
                    selectB(bSelected);
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && isPlacing == true)
        {
            isPlacing = false;
            selectB(bSelected);
        }
    }

    public void Cancel()
    {
        isPlacing = false;
        selectB(bSelected);
    }

}
