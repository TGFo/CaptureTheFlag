using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public GameObject sender;
    public NavMeshAgent agent;
    public enum projectileStates
    {
        idle,
        chaseTarget,
        returnToSender
    }
    public projectileStates currentState = projectileStates.idle;
    private void Start()
    {
        currentState = projectileStates.idle;
    }
}
