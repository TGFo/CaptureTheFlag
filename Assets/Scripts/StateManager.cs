using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    public enum AIStates
    {
        pursueFlag,
        attackPlayer,
        returnFlag,
        scorePoint
    }
    [SerializeField]private AIStates currentState;

    public FlagBaseScript playerBase;
    public FlagBaseScript AIBase;

    private Transform AIBasePos;

    public Transform playerFlagPos;
    public Transform AIFlagPos;

    private Transform playerPos;
    private Transform AIPos;

    public GameObject player;
    public GameObject AIAgent;

    private PickupFlag playerPickup;
    private PickupFlag AIPickup;

    private Attack playerAttack;
    private Attack AIAttack;

    public GameObject spawnPositionsParent;
    public Transform[] spawnPositions;

    private NavMeshAgent agent;
    public float avoidanceRadius = 5;

    void Start()
    {
        agent = AIAgent.GetComponent<NavMeshAgent>();
        playerPos = player.GetComponent<Transform>();
        AIPos = AIAgent.GetComponent<Transform>();
        playerPickup = player.GetComponent<PickupFlag>();
        AIPickup = AIAgent.GetComponentInChildren<PickupFlag>();
        playerAttack = player.GetComponent<Attack>();
        AIAttack = AIAgent.GetComponent<Attack>();
        AIBasePos = AIBase.GetComponent<Transform>();
        spawnPositions = spawnPositionsParent.GetComponentsInChildren<Transform>();
        SetState(AIStates.pursueFlag);
    }

    void Update()
    {
        PerformStateBehavior();
        Debug.DrawLine(agent.transform.position, agent.destination);
    }

    private void SetState(AIStates newState)
    {
        currentState = newState;
        EnterState();
    }

    private void PerformStateBehavior()
    {
        switch (currentState)
        {
            case AIStates.pursueFlag:
                PursueFlag();
            break;
            case AIStates.attackPlayer:
                AttackPlayer();
            break;
            case AIStates.returnFlag:
                ReturnFlag();
            break;
            case AIStates.scorePoint:
                ScorePoint();
            break;
        }
    }

    private void EnterState()
    {
        Debug.Log("Entering State: " + currentState);
    }

    private void AvoidPlayer(Transform finalLocation)
    {
        // Calculate the distance between the agent and the player
        float distanceToPlayer = Vector2.Distance(AIPos.position, playerPos.position);

        if (distanceToPlayer <= avoidanceRadius)
        {
            // Calculate the direction away from the player
            Vector2 moveAwayDirection = (AIPos.position - playerPos.position).normalized;

            // Calculate the new position outside the avoidance radius
            Vector2 newPosition = (Vector2)AIPos.position + moveAwayDirection * avoidanceRadius;

            // Find a point on the NavMesh closest to the calculated position outside the avoidance radius
            NavMeshHit hit;
            Debug.DrawLine (agent.transform.position, newPosition);
            if (NavMesh.SamplePosition(newPosition, out hit, avoidanceRadius, NavMesh.AllAreas))
            {
                // Set the destination of the NavMeshAgent to the new position outside the avoidance radius
                agent.SetDestination(newPosition);
            }
        }
        else
        {
            // If the distance to the player is greater than the avoidance radius, move towards the player
            agent.SetDestination(finalLocation.position);
        }
    }

    private void PursueFlag()
    {
        if(AIPickup.hasFlag == false)
        {
            agent.SetDestination(playerFlagPos.position);
        }
        else
        {
            SetState(AIStates.scorePoint);
        }
    }
    private void AttackPlayer()
    {
        if(AIPickup.hasFlag == false && playerPickup.hasFlag == true)
        {
            AvoidPlayer(playerPos);
            AIAttack.AttackActor(playerPos);
        }
        else 
        {
            if(playerPickup.hasFlag == false) 
            {
                SetState(AIStates.returnFlag);
            }
        }
    }
    private void ReturnFlag()
    {
        if(AIBase.flagMissing == true)
        {
            agent.SetDestination(AIFlagPos.position);
        }
        else
        {
            SetState(AIStates.pursueFlag);
        }
    }
    private void ScorePoint()
    {
        if(AIPickup.hasFlag == true)
        {
            AvoidPlayer(AIBasePos);
        }
        else
        {
            SetState(AIStates.attackPlayer);
        }
    }
    public void KillActor(GameObject actor)
    {

    }
}