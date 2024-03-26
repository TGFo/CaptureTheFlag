using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    float xMove;
    float yMove;
    Vector2 movement;
    public int speedMult = 2;
    public Attack playerAttack;
    
    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        movement.y = yMove * speedMult;
        movement.x = xMove * speedMult;
        // Get the player's direction
        Vector2 direction = new Vector2(xMove, yMove).normalized;

        // Debug Log
        //Debug.Log("Player Direction: " + direction);
        Debug.DrawLine(transform.position, (Vector2)transform.position + direction);

        if(Input.GetKeyUp(KeyCode.Space ))
        {
            playerAttack.AttackActor();
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (movement * Time.fixedDeltaTime));
    }
    
     //script that allows the player to move their character with WASD
}
