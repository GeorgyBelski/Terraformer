using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETAbility1 : MonoBehaviour
{
    public enum State { Redy, Activated, Recharge};
    public float castTime = 3.8f;
    public float timerCast;

    public Transform gunpoint;
    public GameObject thunderball;
    public Transform tmp;

    ElectroTower elTower;
    


    void Start()
    {
     //   elTower = GetComponent<ElectroTower>();
    }

    void Update()
    {
        timerCast -= Time.deltaTime;
        if (tmp)
        {
            tmp.localScale *= 1.1f;
        }

        if (timerCast <= 0) {
            timerCast = 0;
            if (tmp) {
                Destroy(tmp.gameObject);
                tmp = null;

            }
        }
    }

    public void ETA1Activate() {
        tmp = Instantiate(thunderball, Vector3.zero, this.transform.rotation).transform;
        timerCast = castTime;
    }
}
