using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum AIStates
    {
        pursueFlag,
        avoidPlayer,
        attackPlayer,
        returnFlag,
        scorePoint
    }
    private AIStates currentState;
    public GameObject playerFlag;
    public GameObject AIFlag;

    void Start()
    {
        SetState(AIStates.pursueFlag);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(AIStates.Jumping);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetState(AIStates.Attacking);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            SetState(AIStates.Walking);
        }
        else
        {
            SetState(AIStates.Idle);
        }

        PerformStateBehavior();
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
            case AIStates.Idle:
                break;
            case AIStates.Walking:
                break;
            case AIStates.Jumping:
                break;
            case AIStates.Attacking:
                break;
        }
    }

    private void EnterState()
    {
        Debug.Log("Entering State: " + currentState);
    }
}
