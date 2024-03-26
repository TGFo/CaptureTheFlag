using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveToLocation : MonoBehaviour
{
    //test script should be ignored
    public Transform location;
    public Vector2 NavPoint;
    public NavMeshAgent agent;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NavPoint = location.position;
        agent.SetDestination(NavPoint);
    }

    // Update is called once per frame
    void Update()
    {
        NavPoint = location.position;
        agent.SetDestination(NavPoint);
        if(Input.GetKeyUp(KeyCode.E))
        {
            agent.Warp(agent.nextPosition * 1.5f);
            //rb.AddForce(transform.forward * 50);
        }
    }
}
