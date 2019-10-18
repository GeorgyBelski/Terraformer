
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyMouseController : MonoBehaviour
{
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;

    Camera camera;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        camera = Camera.main;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

        //if (agent.destination)
        //{

        //}
        /*
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else {
            character.Move(Vector3.zero, false, false);
        }
        */
    }

    public void SetDest(Vector3 posicion)
    {
        agent.SetDestination(posicion);
    }

    public Vector3 GetDest()
    {
        return agent.destination;
    }



}
