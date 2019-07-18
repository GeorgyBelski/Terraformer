using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityButtonController : MonoBehaviour
{
    public enum State { Ready, Aiming, Casting, Recharging };
    public KeyCode key;
    public TowerType castTowerType;
    public State currentState = State.Ready;
    public float castTime = 2.0f;
    public float timerCast;
    public float coolDown = 5.0f;
    public float timerCoolDown;
    public Color buttomTintReady;
    public Color buttomTintRecharging;
    public GameObject parent;
    public static AbilityButtonController aimingAbility;

    public Transform gunpoint;
    // public GameObject thunderball;
    public GameObject aimAreaPrefab;
    protected Transform aimArea;
    Animator aimAreaAnimator;

    ElectroTower casterTower;
    float camRayLength = 90f;
    int groundMask;
    Vector3 mousePos;

    Image buttonImage, outLineImage;
    Button button;

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        if(!buttonImage) buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        outLineImage = parent.GetComponent<Image>();
        outLineImage.enabled = false;
        buttonImage.color = buttomTintReady;
        buttonImage.fillAmount = 1f;
        aimArea = null;
    }

    void Update()
    {
        ReduceTimers();

        ButtonAvailabilityControl();
        if (currentState == State.Ready && Input.GetKey(key))
        {
            Activate();
        }

        if (currentState == State.Aiming)
        {
            Aiming();
            if (Input.GetMouseButtonDown(0))
            {
                if (casterTower)
                {
                  //  print("casterTower: " + casterTower);
                    currentState = State.Casting;
                    buttonImage.color = buttomTintRecharging;
                    timerCoolDown = coolDown;
                    timerCast = castTime;
                    TowerManager.ClearSelection();
                    ///   casterTower.CastThanderBall(aimArea.position);
                    TowerCastAreaAbility(casterTower, aimArea.position);
                    outLineImage.enabled = false;
                    aimingAbility = null;
                }
                else
                {
                    currentState = State.Ready;
                    RemoveAimArea();
                }

                RemoveAimArea();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Cancel();
            }

        }
    }
    public abstract void TowerCastAreaAbility(Tower casterTower, Vector3 aimAreaPosition);  /// <summary>
                                                                                            /// casterTower.CastThanderBall(aimArea.position);  // example for ElectroTower
                                                                                            /// </summary>

    public void Cancel() {
        currentState = State.Ready;
        RemoveAimArea();
        outLineImage.enabled = false;
        aimingAbility = null;
    }
    public void Activate()
    {
        timerCast = castTime;

        if (currentState == State.Ready)
        {
            currentState = State.Aiming;
            if (aimingAbility) {
                aimingAbility.Cancel();
            }
            aimingAbility = this;
            outLineImage.enabled = true;
            ShowAimArea();

        }

    }

    private void ShowAimArea()
    {
        if (!aimArea)
        {
            GameObject aimAreaGameObject = Instantiate(aimAreaPrefab, Vector3.zero, this.transform.rotation);
            aimArea = aimAreaGameObject.transform;
            aimAreaAnimator = aimAreaGameObject.GetComponent<Animator>();

        }
        else
        {
            aimAreaAnimator.SetBool("isSetted", false);
        }
    }

    void Aiming()
    {
        if (aimArea)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, groundMask))
            {
                mousePos = floorHit.point;
                aimArea.position = floorHit.point;
                TowerManager.ClearSelection();
                Tower nearestTower = TowerManager.GetNearestTower(aimArea, castTowerType);
                if (castTowerType == TowerType.Electro)
                {
                    casterTower = (ElectroTower)nearestTower;
                }
                else if (castTowerType == TowerType.Laser)
                {
                    casterTower = (ElectroTower)nearestTower;
                }
                
                if (casterTower)
                {
                    casterTower.isSelected = true;
                 //   print("Aiming tower: " + casterTower);
                }

            }
        }
    }

    void ReduceTimers()
    {

        if (timerCoolDown <= 0)
        {
            timerCoolDown = 0;
            if (currentState == State.Recharging)
            {
                currentState = State.Ready;
                buttonImage.color = buttomTintReady;
            }
        }
        else
        {
            timerCoolDown -= Time.deltaTime;
            buttonImage.fillAmount = (coolDown - timerCoolDown) / coolDown;
        }

        if (timerCast <= 0)
        {
            timerCast = 0;
            if (currentState == State.Casting)
            {
                currentState = State.Recharging;
                casterTower.EndCasting();
            }
        }
        else
        {
            timerCast -= Time.deltaTime;
        }
    }
    void RemoveAimArea()
    {
        if (aimArea)
        {
            //    Destroy(aimArea.gameObject);
            //   aimArea = null;
            aimAreaAnimator.SetBool("isSetted", true);
        }
    }

    void ButtonAvailabilityControl()
    {
        if (TowerManager.availableElectroTowers.Count == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
