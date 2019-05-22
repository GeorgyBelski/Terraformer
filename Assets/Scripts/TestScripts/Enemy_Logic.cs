using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Logic : MonoBehaviour
{

    private EnemyMouseController emk;
    private Enemy enem;
    private Transform destTower = null;
    private bool isAttack = false;
    private bool isStand = true;
    private bool isGiveUp = false;
    private bool isRush = false;

    private Transform destHel = null;
    private float checkTime = 0.5f;//change!!!!!!!!!!!!!!!!!!!!!
    private float realcheckTime;//change!!!!!!!!!!!!!!!!!!!!!
    private float brawe = 0; //= Random.RandomRange(0.1f, 0.7f);
    private float min;

    // Start is called before the first frame update
    void Start()
    {
        brawe = Random.Range(0.1f, 0.7f);
        realcheckTime = checkTime;
        emk = GetComponent<EnemyMouseController>();
        enem = GetComponent<Enemy>();
        EnemyManagerPro.addEnemy("Damager", enem);
        //float min;

    }

    // Update is called once per frame
    void Update()
    {
        realcheckTime -= Time.deltaTime;
        if(realcheckTime <= 0)
        {
            realcheckTime = checkTime;
            
            check();
            
        }
        if (isStand)
        {
            check();
            //isStand = false;
            //isGiveUp = false;
            //state_Attack();
 
        }
        
        //if (isGiveUp == true)
        //{
        //   state_Attack();

        //}
        //Debug.Log(enem.GetHealthRatio());
    }

    void state_Attack()
    {
        if (TowerManager.towers.Count > 0)
        {
            min = Vector3.Distance(TowerManager.towers[0].gameObject.transform.position, transform.position);
            destTower = TowerManager.towers[0].gameObject.transform;

            //Debug.Log(TowerManager.towers.Count);
            for (int i = 0; i < TowerManager.towers.Count; i++)
            {
                Transform current_dist = TowerManager.towers[i].gameObject.transform;

                if (min > Vector3.Distance(current_dist.position, transform.position))
                {
                    min = Vector3.Distance(current_dist.position, transform.position);
                    destTower = current_dist;
                }
            }
            //Debug.Log(destTower.position);
            emk.SetDest(destTower);
            //isAttack = true;
        }
        else
        {
            isStand = true;
        }
    }
    void state_GiveUp()
    {
        if(EnemyManagerPro.enemiesMap["Healer"].Count > 0)
        {
            //Debug.Log("checking Heal");
            min = Vector3.Distance(EnemyManagerPro.enemiesMap["Healer"][0].gameObject.transform.position, transform.position);
            destHel = EnemyManagerPro.enemiesMap["Healer"][0].gameObject.transform;
            for (int i = 0; i < EnemyManagerPro.enemiesMap["Healer"].Count; i++)
            {
                Transform current_dist = EnemyManagerPro.enemiesMap["Healer"][i].gameObject.transform;

                if (min > Vector3.Distance(current_dist.position, transform.position))
                {
                    min = Vector3.Distance(current_dist.position, transform.position);
                    destHel = current_dist;
                }
            }
            //Debug.Log("going");
            emk.SetDest(destHel);
        }
        
    }

    public void check()
    {
        
        float heals = enem.GetHealthRatio();
        //Debug.Log(heals + " " + brawe);
        if((heals - brawe) <= 0 && heals != 0) //Логика состояний
        {
            //Debug.Log("checking");
            isStand = false;
            isAttack = false;
            isGiveUp = true;
            //state_Attack();
            state_GiveUp();
        }
        else
        {
            if (isGiveUp)
            {
                if(heals > 0.5 + brawe/2)
                {
                    isStand = false;
                    isAttack = true;
                    isGiveUp = false;
                    state_Attack();
                }
                else
                {
                    state_GiveUp();
                }
            }
            else
            {
                isStand = false;
                isAttack = true;
                isGiveUp = false;
                state_Attack();
            }

            /*
            if(isGiveUp && heals > 0.5f) { 

            }
            else
            {
                
            }
            */
        }
    }
}
