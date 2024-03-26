using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    public StateManager stateManager;

    public GameObject target;
    private Transform targetTransform;

    public GameObject sender;
    private Transform senderTranform;

    private PickupFlag targetFlag;

    public NavMeshAgent agent;

    public float maxRange = 5;
    private bool hitTarget = false;
    string targetTag;
    public enum ProjectileStates
    {
        idle,
        chaseTarget,
        returnToSender
    }
    public ProjectileStates currentState = ProjectileStates.idle;
    private void Start()
    {
        targetTag = target.tag;
        targetFlag = target.GetComponentInChildren<PickupFlag>();
        targetTransform = target.transform;
        senderTranform = sender.transform;
        currentState = ProjectileStates.idle;
    }

    private void Update()
    {
        PerformStateBehaviour();
        if(hitTarget == true)
        {
            hitTarget = false;
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

    public void Idle()
    {
        agent.enabled = false;
        hitTarget = false;
    }
    public void ChaseTarget()
    {
        if(agent.enabled == false)
        {
            agent.enabled = true;
        }
        agent.SetDestination(targetTransform.position);
        if(Vector2.Distance(transform.position, targetTransform.position) <=0.5f)
        {
            targetFlag.DropFlag();
            hitTarget = true;
            SetState(ProjectileStates.returnToSender);
        }
        if(Vector2.Distance(transform.position, senderTranform.position) > maxRange)
        {
            SetState(ProjectileStates.returnToSender);
        }
    }
    public void ReturnToSender()
    {
        agent.SetDestination(senderTranform.position);
        if(transform.position ==  senderTranform.position || Vector2.Distance(transform.position, senderTranform.position) <= 1)
        {
            SetState(ProjectileStates.idle);
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
    public IEnumerator StunTarget(Transform respawnLocation)
    {
        target.SetActive(false);
        Debug.Log(respawnLocation);
        targetTransform.position = respawnLocation.position;

        yield return new WaitForSeconds(3f);

        target.SetActive(true);
    }
}
