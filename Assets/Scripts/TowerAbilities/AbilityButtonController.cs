using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityButtonController : MonoBehaviour
{
    public enum State { Ready, Aiming, Casting, Recharging };
    public enum Type { Area, SingleTarget};
    public KeyCode key;
    public TowerType castTowerType;
    public State currentState = State.Ready;
    public Type type;
    //   public float castTime = 2.0f;
    //   public float timerCast;
    public float coolDown = 5.0f;
    public int cost = 10;
    public float timerCoolDown;
    public Color buttonTintReady;
    public Color buttonTintRecharging;
    [HideInInspector]
    public GameObject parent;
    public static AbilityButtonController aimingAbility;

    public Transform gunpoint;
    // public GameObject thunderball;
    public GameObject aimAreaPrefab;
    protected Transform aimArea;
    Animator aimAreaAnimator;

    [SerializeField] Tower casterTower;
    Tower previousHighlightedTower;
    Enemy target;
    float camRayLength = 90f;
    int groundMask;
    int enemyMask;
    int enemy_groundMask;
  //  Vector3 mousePos;

    Image buttonImage, outLineImage;
    Button button;

    void Start()
    {
        groundMask = Globals.groundLayerMask;
        enemyMask = EnemyManagerPro.enemyLayerMask;
        enemy_groundMask = enemyMask | groundMask;
        if (!buttonImage) buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        parent = transform.parent.gameObject;
        outLineImage = parent.GetComponent<Image>();
        outLineImage.enabled = false;
        buttonImage.color = buttonTintReady;
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
                if (type == Type.SingleTarget && !target) { return; }

                if (casterTower)
                {
                    if (ResourceManager.RemoveResource(cost))
                    {
                        ResourceManager.DisplayCost(false);
                        currentState = State.Recharging;
                        buttonImage.color = buttonTintRecharging;
                        timerCoolDown = coolDown;
                        casterTower.isHighlighted = false;
                        previousHighlightedTower = null;

                        if (type == Type.Area) 
                        { 
                            TowerCastAreaAbility(casterTower); 
                        }
                        else 
                        { 
                            TowerCastSingleTargetAbility(casterTower, target);
                            target = null;
                        }
                        outLineImage.enabled = false;
                        aimingAbility = null;
                    }
                    else
                    {
                        ResourceManager.CostIsTooHighSignal();
                        Cancel();
                    }
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
                ResourceManager.DisplayCost(false);
                Cancel();
            }

        }
    }
    public virtual void TowerCastAreaAbility(Tower casterTower) { }/// <summary>
                                                                   /// casterTower.CastThanderBall(aimArea.position);  // example for ElectroTower
                                                                   /// </summary>
    public virtual void TowerCastSingleTargetAbility(Tower casterTower, Enemy target) { }

    public void Cancel() {       
        currentState = State.Ready;
        RemoveAimArea();
        outLineImage.enabled = false;
        aimingAbility = null;
        if (casterTower)
        {
            casterTower.isHighlighted = false;
        }
        previousHighlightedTower = null;
    }
    public void Activate()
    {
        //    timerCast = castTime;
        if (button.interactable == false)
        { return; }

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
            GameObject aimAreaGameObject = Instantiate(aimAreaPrefab);
            aimArea = aimAreaGameObject.transform;
            aimAreaAnimator = aimAreaGameObject.GetComponent<Animator>();

        }
        else
        {
            aimAreaAnimator.SetBool("isVanising", false);
        }
    }

    public virtual void Aiming()
    {
        if (aimArea)
        {
            ResourceManager.DisplayCost(true, cost);
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (type == Type.Area && Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, groundMask))
            {
              //  Debug.DrawLine(Camera.main.transform.position, floorHit.point, Color.cyan);
                aimArea.position = floorHit.point;
                DefineCasterTower();
            }
            else if (type == Type.SingleTarget && Physics.Raycast(camRay, out RaycastHit enemyHit, camRayLength, enemy_groundMask))
            {
            //    Debug.DrawLine(Camera.main.transform.position, enemyHit.point, Color.yellow);
                aimArea.position = enemyHit.collider.gameObject.transform.position;
                EnemyManagerPro.TransformEnemyMap.TryGetValue(enemyHit.collider.transform, out target);
                if (target)
                {
                 //   Debug.DrawLine(Camera.main.transform.position, enemyHit.point, Color.yellow);
                    aimArea.position = enemyHit.collider.transform.position;
                    DefineCasterTower();
                }
            //    if(target) Debug.DrawLine(Camera.main.transform.position, enemyHit.point, Color.yellow);
            //    else Debug.DrawLine(Camera.main.transform.position, enemyHit.point, Color.white);
            //    Debug.Log(target);
                
            }
        }
    }
    void DefineCasterTower() 
    {
        Tower nearestTower = TowerManager.GetNearestTower(aimArea, castTowerType);
        if (nearestTower == null)
        { Cancel(); }
        else if (nearestTower == previousHighlightedTower)
        { return; }
        if (castTowerType == TowerType.Electro)
        {
            casterTower = (ElectroTower)nearestTower;
        }
        else if (castTowerType == TowerType.Laser)
        {
            casterTower = (LaserTower)nearestTower;
        }
        else if (castTowerType == TowerType.Plasma)
        {
            casterTower = (PlasmaTower)nearestTower;
        }

        if (casterTower && casterTower != previousHighlightedTower)
        {
            casterTower.isHighlighted = true;
            if (previousHighlightedTower) { previousHighlightedTower.isHighlighted = false; }
            previousHighlightedTower = casterTower;
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
                buttonImage.color = buttonTintReady;
            }
        }
        else
        {
            timerCoolDown -= Time.deltaTime;
            buttonImage.fillAmount = (coolDown - timerCoolDown) / coolDown;
        }
    }
    void RemoveAimArea()
    {
        if (aimArea)
        {
            //    Destroy(aimArea.gameObject);
            //   aimArea = null;
            aimAreaAnimator.SetBool("isVanising", true);
        }
    }

    void ButtonAvailabilityControl()
    {
        int availableTowersCount = 0;
        if (castTowerType == TowerType.Electro)
        {
            availableTowersCount = TowerManager.availableElectroTowers.Count;
        }
        else if (castTowerType == TowerType.Laser)
        {
            availableTowersCount = TowerManager.availableLaserTowers.Count;
        }
        else if(castTowerType == TowerType.Plasma)
        {
            availableTowersCount = TowerManager.availablePlasmaTowers.Count;
        }

        if (availableTowersCount == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
