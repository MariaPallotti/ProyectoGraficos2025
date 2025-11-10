using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovimientoEnemigos : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public Transform[] waypoints;

    int m_CurrentWaypointIndex;
    int frameWait;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        frameWait = 0;
        navMeshAgent.SetDestination(waypoints[0].position);
        animator.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            animator.SetBool("isRunning", false);
            if (frameWait > 120)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                animator.SetBool("isRunning", true);
                frameWait = 0;
            }
            frameWait += 1;
        }
    }
}
