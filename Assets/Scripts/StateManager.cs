using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    public enum AIStates                                        //enum holding all AI states
    {
        pursueFlag,
        attackPlayer,
        returnFlag,
        scorePoint
    }
    [SerializeField]private AIStates currentState;              //the AI's current state
    
    //Declarations to be used for logic
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

    //the spawn positions to be used when the player or AI has to respawn after being attacked
    public GameObject AISpawnPositionsParent;
    public GameObject playerSpawnsParent;
    [SerializeField] Transform[] AISpawnPositions;
    [SerializeField] Transform[] playerSpawnPositions;

    private NavMeshAgent agent;
    //radius around which the AI should avoid a destination
    public float avoidanceRadius = 5;

    void Start()
    {
        //caching componenets for performance
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
        //sets the state to pursue on game start
        SetState(AIStates.pursueFlag);
    }

    void Update()
    {
        //performs behaviour every frame
        PerformStateBehavior();
        Debug.DrawLine(agent.transform.position, agent.destination);
    }

    private void SetState(AIStates newState)        //method mainly used so new states can be shown as they are entered on the console
    {
        currentState = newState;
        Debug.Log("Entering State: " + currentState);
    }

    private void PerformStateBehavior()
    {
        if(AIAgent.activeSelf == false)             //returns if navmeshagent component is disabled so state logic does not run
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

    private void AvoidPlayer(Transform avoidee, Transform finalLocation)                                        //method that allows the AI to avoid a destination while moving towards another
    {
        //distance between AI and avoidance location
        float distanceToPlayer = Vector2.Distance(AIPos.position, avoidee.position);
        //checks if distance is less than or equal to avoidance radius
        if (distanceToPlayer <= avoidanceRadius)
        {
            //the direction the AI should move
            Vector2 moveAwayDirection = (AIPos.position - avoidee.position).normalized;
            //the AI's new position it should move to
            Vector2 newPosition = (Vector2)AIPos.position + moveAwayDirection * avoidanceRadius;

            Debug.DrawLine(agent.transform.position, newPosition);
            //sets the destination of the NavMeshAgent to the new position outside the avoidance radius
            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, avoidanceRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            //if the distance is greater than the avoidance radius then the agent should move to its final location
            agent.SetDestination(finalLocation.position);
        }
    }

    private void PursueFlag()                                                   //state that makes the agent pursue the enemy flag
    {
        //checks if the AI currently is not holding a flag
        if(AIPickup.hasFlag == false)
        {
            //checks if the player is holding a flag
            if(playerPickup.hasFlag == true)
            {
                //attacks the player if they have the AI flag
                SetState(AIStates.attackPlayer);
            }
            agent.SetDestination(playerFlagPos.position);
        }
        else
        {
            //moves the AI to score a point if it is holding a flag
            SetState(AIStates.scorePoint);
        }
    }
    private void AttackPlayer()                                                 //state that makes the AI agent attack the player should the conditions be correct
    {
        //checks if the AI has no flag and if the player is holding a flag
        if(AIPickup.hasFlag == false && playerPickup.hasFlag == true)
        {
            //stays within a certain radius of the player attacking while not getting too close
            AvoidPlayer(playerPos, playerPos);
            AIAttack.AttackActor();
        }
        else 
        {
            //if the AI has a flag and the player has no flag the AI moves to return its flag to homebase
            if(playerPickup.hasFlag == false) 
            {
                SetState(AIStates.returnFlag);
            }
        }
    }
    private void ReturnFlag()                                                   //state that makes the AI agent rush towards its flag so it can be returned to homebase
    {

        //checks if the AI base is missing its flag
        if(AIBase.flagMissing == true)
        {
            //sets the AI to move towards its flag
            agent.SetDestination(AIFlagPos.position);
        }
        else
        {
            //sets the AI back to pursueing the enemy flag if its flag is safe
            SetState(AIStates.pursueFlag);
        }
    }
    private void ScorePoint()                                                   //state that moves the AI towards its homebase while avoiding the player should it have the player's flag
    {
        //checks if the AI is currently holding a flag
        if(AIPickup.hasFlag == true)
        {
            //avoid the player while moving towards its home base
            AvoidPlayer(playerPos, AIBasePos);
        }
        else
        {
            //sets the AI to attack the player should it not have a flag while in this state
            SetState(AIStates.attackPlayer);
        }
    }

    public Transform GetRandomRespawnLocation(string tag)                       //method that returns a random transform based on the provided object tag, in this case "Player" or "AiAgent"
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

    //Script that manages the AI and some other aspects of the game
}