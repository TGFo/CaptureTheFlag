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
    private Transform PlayerBasePos;

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

    public GameObject AISpawnPositionsParent;
    public GameObject playerSpawnsParent;
    [SerializeField] Transform[] AISpawnPositions;
    [SerializeField] Transform[] playerSpawnPositions;

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
        PlayerBasePos = playerBase.GetComponent<Transform>();
        AISpawnPositions = AISpawnPositionsParent.GetComponentsInChildren<Transform>();
        playerSpawnPositions = playerSpawnsParent.GetComponentsInChildren<Transform>();
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
        Debug.Log("Entering State: " + currentState);
    }

    private void PerformStateBehavior()
    {
        if(AIAgent.activeSelf == false)
        {
            return;
        }
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

    private void AvoidPlayer(Transform avoidee, Transform finalLocation)
    {
        float distanceToPlayer = Vector2.Distance(AIPos.position, avoidee.position);

        if (distanceToPlayer <= avoidanceRadius)
        {
            Vector2 moveAwayDirection = (AIPos.position - avoidee.position).normalized;

            Vector2 newPosition = (Vector2)AIPos.position + moveAwayDirection * avoidanceRadius;

            Debug.DrawLine(agent.transform.position, newPosition);
            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, avoidanceRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else
        {

            agent.SetDestination(finalLocation.position);
        }
    }

    private void PursueFlag()
    {
        if(AIPickup.hasFlag == false)
        {
            if(playerPickup.hasFlag == true)
            {
                SetState(AIStates.attackPlayer);
            }
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
            AvoidPlayer(playerPos, playerPos);
            AIAttack.AttackActor();
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
            AvoidPlayer(playerPos, AIBasePos);
        }
        else
        {
            SetState(AIStates.attackPlayer);
        }
    }

    public Transform GetRandomRespawnLocation(string tag)
    {
        Transform[] locationArray = null;
        if(tag != "Player" && tag != "AiAgent")
        {
            return null;
        }
        if(tag == "Player")
        {
            locationArray = playerSpawnPositions;
        }
        else if(tag == "AiAgent")
        {
            locationArray = AISpawnPositions;
        }
        Transform location = locationArray[Random.Range(0, locationArray.Length - 1)];
        Debug.Log(location.position);
        return location;
    }
}