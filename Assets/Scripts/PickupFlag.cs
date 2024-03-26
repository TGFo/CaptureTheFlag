using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFlag : MonoBehaviour
{
    //bool to check if actor is currently holding a flag
    public bool hasFlag;
    //storing own flag so not all flags can be picked up
    public GameObject ownFlag;
    //caching transforms to save on performance later
    private Transform otherFlag = null;
    public Transform flagHand;
    public Transform flagOriginalPos;
    //other actor's pickup script allowing for the flag to be reset without issue
    public PickupFlag otherAgent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        otherFlag = collision.transform;
        if (collision.gameObject.CompareTag("flag"))
        {
            //Debug.Log("flag");
            if (collision.gameObject == ownFlag)    //checks if the object colliding with the player/AI is their own flag
            {
                ResetFlag();
            }else                                   //if not the player/AI's own flag then parents the flag to the flag pos transform and sets has flag to true
            {
                otherFlag.parent = flagHand;
                hasFlag = true;
            }
        }
    }

    public void DropFlag()  //method to drop the flag, setting its parent to null and the hasflag bool to false
    {
        if(otherFlag == null) 
        {
            return;
        }
        if(hasFlag == true)
        {
            hasFlag = false;
            otherFlag.parent = null;
        }
    }

    public void ResetFlag()     //resets the flag back to its original position, unparents the flag from the player/AI that originally had it, and sets hasFlag to false for the other agent
    {
        otherFlag.position = flagOriginalPos.position;
        otherFlag.parent = null;
        otherAgent.hasFlag = false;
    }
}
