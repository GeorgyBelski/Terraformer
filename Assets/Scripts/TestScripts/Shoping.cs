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
    public GameObject lazerTower;

    float camRayLength = 60f;
    public int floorMask;


    private bool selectedElectroBool = false;
    private bool selectedLazerBool = false;
    private bool isPlacing = false;
    private GameObject realTimeTowerPlace;

    private ColorBlock defaultColor;
    private Button bSelected;
    private Material mt;

    //public NavMeshAgent agent;

    void Start()
    {
        floorMask = LayerMask.GetMask("Ground");
        //defaultColor = 
        selectedTowerColor = Color.green;
        //agent.updateRotation = false;
    }
    public void SelectElectroTower(Button b)
    {
        if (!selectedElectroBool)
        {
            bSelected = b;
            defaultColor = b.colors;
            ColorBlock cb = b.colors;// = selectedTowerColor;
            cb.normalColor = selectedTowerColor;
            cb.highlightedColor = selectedTowerColor;
            b.colors = cb;
            selectedElectroBool = true;
            isPlacing = true;
            realTimeTowerPlace = Instantiate(towerPlace, Vector3.zero, this.transform.rotation);
            mt = realTimeTowerPlace.gameObject.GetComponent<Renderer>().material;
        }
        else
        {
            bSelected = null;
            b.colors = defaultColor;
            selectedElectroBool = false;
            isPlacing = false;
            Destroy(realTimeTowerPlace);
        }
        
        //Debug.Log("Electro");
    }

    public void SelectLazerTower()
    {
        //Debug.Log("Lazer");
    }

    void Update()
    {
        if (isPlacing)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                
                realTimeTowerPlace.transform.position = new Vector3(floorHit.point.x, floorHit.point.y + realTimeTowerPlace.transform.localScale.y - 0.3f, floorHit.point.z);
                if (realTimeTowerPlace.GetComponent<TowerPlacing>().isOnTower)
                {
                    // = new Material();//.SetColor(Color.red);
                    mt.color = new Color(1, 0, 0, 0.5f);
                    //mt.color.a = 100;


                }
                else
                {
                    mt.color = new Color(0, 1, 0, 0.5f);
                }
                //Debug.Log(Input.GetMouseButtonDown(0));

                /* Vector3 playerToMouse = floorHit.point - transform.position;
                   playerToMouse.y = 0f;

                   Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                   rb.MoveRotation(newRotation);
                */
                //transform.LookAt(new Vector3(floorHit.point.x, transform.position.y, floorHit.point.z));
                if (Input.GetMouseButtonDown(0) && !realTimeTowerPlace.GetComponent<TowerPlacing>().isOnTower)
                {

                    Instantiate(electroTower, realTimeTowerPlace.transform.position, realTimeTowerPlace.transform.rotation);
                    Destroy(realTimeTowerPlace);
                    //agent.
                    //isPlacing = false;
                    SelectElectroTower(bSelected);
                }
            }
        }
    }

}
