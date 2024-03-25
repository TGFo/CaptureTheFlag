using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBaseScript : MonoBehaviour
{
    public GameObject ownFlag;
    public bool flagMissing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != ownFlag && (!other.CompareTag("Player") || !other.CompareTag("AiAgent")))
        {

        }
        if (other.gameObject == ownFlag && flagMissing == true)
        {
            flagMissing = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == ownFlag)
        {
            flagMissing = true;
        }
    }
}
