using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TestAnimationController : MonoBehaviour
{
    public Animator m_Animator;
    public Rigidbody m_Rigidbody;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Attack()
    {

    }
    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition ) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            //   v.y = m_Rigidbody.velocity.y;
            //   m_Rigidbody.velocity = v;
            transform.position = m_Animator.rootPosition;
        }
    }
}
