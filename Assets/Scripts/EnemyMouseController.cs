
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyMouseController : MonoBehaviour
{
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;

    public float rotationTime = 0.4f, timerRotation;
    bool isRotate = false;
    Vector3 lookAtPoint;
    public float rotationSpeed = 5;
  //  Ray ray;
  //  RaycastHit hit;

    void Start()
    {
        agent.updateRotation = false;
    }

    void FixedUpdate()
    {
        Rotation();
    }
    void Update()
    {

        

        if (agent.enabled && agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

    }

    public void SetDestination(Vector3 position)
    {
        if (agent.enabled)
        { agent.SetDestination(position); }
    }
    public void SetRotation(Vector3 lookAtPoint) 
    {
        isRotate = true;
        this.lookAtPoint = lookAtPoint;
     //   character.Rotate(lookAtPoint);
        timerRotation = rotationTime;
        
    }

    void Rotation() 
    {
        if (isRotate) 
        {
            timerRotation -= Time.deltaTime;
            if (timerRotation > 0)
            {
                Vector3 direction = lookAtPoint - transform.position;
             //   Debug.DrawLine(transform.position, transform.position + direction, Color.cyan, 1f);             
                Quaternion toRotation = Quaternion.LookRotation(direction);
             //   Debug.DrawLine(transform.position, transform.position + toRotation*Vector3.forward, Color.blue, 1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);  
            }
            else 
            {isRotate = false; }
        }
    }
    public Vector3 GetDest()
    {
        return agent.destination;
    }



}
