using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    //declarations to be used for state logic
    public StateManager stateManager;
    
    //the target the projectile moves towards when an actor attacks
    public GameObject target;
    private Transform targetTransform;

    //the actor that attacked, sending the projectile
    public GameObject sender;
    private Transform senderTranform;

    private PickupFlag targetFlag;

    public NavMeshAgent agent;

    //the max range before the projectile returns to sender
    public float maxRange = 5;
    //bool representing if the target was hit or not
    private bool hitTarget = false;
    string targetTag;

    //enum storing all the projectile's states
    public enum ProjectileStates
    {
        idle,
        chaseTarget,
        returnToSender
    }
    //the projectile's current state
    public ProjectileStates currentState = ProjectileStates.idle;

    private void Start()
    {
        //caching values for performance
        targetTag = target.tag;
        targetFlag = target.GetComponentInChildren<PickupFlag>();
        targetTransform = target.transform;
        senderTranform = sender.transform;
        currentState = ProjectileStates.idle;
    }

    private void Update()
    {
        //performs state behaviour every frame
        PerformStateBehaviour();
        //checks if the projectile hit the target
        if(hitTarget == true)
        {
            //sets hitTarget to false
            hitTarget = false;
            //starts the stun method
            StartCoroutine(StunTarget(stateManager.GetRandomRespawnLocation(targetTag)));
        }
    }

    public void PerformStateBehaviour()
    {
        switch(currentState)
        {
            case ProjectileStates.idle:
                Idle();
                break;
            case ProjectileStates.chaseTarget:
                ChaseTarget();
                break;
            case ProjectileStates.returnToSender:
                ReturnToSender();
                break;
        }
    }
    public void SetState(ProjectileStates state)
    {
        currentState = state;
        Debug.Log("Projectile entering state: " +  state);
    }

    public void Idle()                                                                                                                  //the idle behaviour
    {
        //disables the NavMeshAgent component so the projectile moves with its parent object
        agent.enabled = false;
        //sets hit target to false so the projectile can be launched while in this state
        hitTarget = false;
    }
    public void ChaseTarget()                                                                                                           //chase target behvaiour 
    {
        //checks if the NavMeshAgent component is disabled and enables it if it is
        if(agent.enabled == false)
        {
            agent.enabled = true;
        }
        //set the projectile to move towards its target
        agent.SetDestination(targetTransform.position);
        //checks if the distance between the projectile's target and its current postion is less than or equal to threshold value
        if(Vector2.Distance(transform.position, targetTransform.position) <=0.5f)
        {
            //makes the target drop their flag
            targetFlag.DropFlag();
            //sets hit target to true
            hitTarget = true;
            //sets the current state to return to sender
            SetState(ProjectileStates.returnToSender);
        }
        //checks if the distance between the projectile and its sender is greater than the max range value
        if(Vector2.Distance(transform.position, senderTranform.position) > maxRange)
        {
            //returns the projectile to sender
            SetState(ProjectileStates.returnToSender);
        }
    }
    public void ReturnToSender()                                                                                                        //return to sender state behaviour
    {
        //sets the agents destination to the sender's position
        agent.SetDestination(senderTranform.position);
        //checks if the two postions match or if the distance between the two is less than or equal to 1
        if(transform.position ==  senderTranform.position || Vector2.Distance(transform.position, senderTranform.position) <= 1)
        {
            //sets the current state back to idle
            SetState(ProjectileStates.idle);
            //sets the projectile position back to the sender's position
            transform.position = senderTranform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");
        if(collision.gameObject == target && hitTarget == false)
        {
            hitTarget = true;
            StartCoroutine(StunTarget(stateManager.GetRandomRespawnLocation(collision.tag)));
        }
        else if(collision.gameObject == sender && currentState == ProjectileStates.returnToSender)
        {
            SetState(ProjectileStates.idle);
        }
    }
    public IEnumerator StunTarget(Transform respawnLocation)                                                                              //stuns the target by disabling the object then moving it, enabling it after 3 seconds
    {
        //disables the target
        target.SetActive(false);
        Debug.Log(respawnLocation);
        //sets the target's position to the respawn location
        targetTransform.position = respawnLocation.position;

        //waits 3 seconds
        yield return new WaitForSeconds(3f);

        //reenables the target
        target.SetActive(true);
    }

    //script that manages the state machine for the projectile as it is an AI agent on its own
}
