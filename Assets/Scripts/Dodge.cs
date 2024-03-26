using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dodge : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody2D rb;
    public GameObject ownProjectile;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Projectile") && collision.gameObject != ownProjectile)
        {
            Vector2 projectileDirection = collision.transform.position - transform.position;
            Vector2 dodgeDirection = Vector2.Perpendicular(projectileDirection);
            if(rb != null)
            {
                if(Input.GetKeyDown(KeyCode.LeftAlt))
                rb.AddForce(dodgeDirection * 50, ForceMode2D.Impulse);
            }
            if(agent != null)
            {
                agent.Move(dodgeDirection * 1000);
            }
        }
    }
}
