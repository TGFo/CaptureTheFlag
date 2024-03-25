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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (movement * Time.fixedDeltaTime));
    }
}
