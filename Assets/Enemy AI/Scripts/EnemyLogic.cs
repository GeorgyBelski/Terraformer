using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private Seeker seeker;
    private Path path;
    private AstarPath aPath;
    private CharacterController chc;
    private Vector3 targetPosition;
    private int currentWayPoint = 0;
    public float speed = 10;
    public float stopDist = 3;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        chc = GetComponent<CharacterController>();
        targetPosition = GameObject.FindWithTag("Tower").transform.position;
        GetNewPath();
        
    }

    private void GetNewPath()
    {
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    }

    private void OnPathComplete(Path newPath)
    {
        Debug.Log("4");
        if (!newPath.error)
        {
            Debug.Log("4+");
            path = newPath;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(path == null)
        {
            //Debug.Log("1");
            return;
        }
        if(currentWayPoint >= path.vectorPath.Count)
        {
            //Debug.Log("2");
            return;
        }

        Vector3 vc = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        
        vc *= speed * Time.deltaTime;
        //Debug.Log("+");
        transform.LookAt(path.vectorPath[currentWayPoint]);
        chc.SimpleMove(vc);
        //AstarPath.
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < stopDist)
        {
            //aPath.Scan();
            //Debug.Log("3");
            currentWayPoint++;
        }
    }
}
