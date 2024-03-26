using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFlag : MonoBehaviour
{
    public bool hasFlag;
    public GameObject ownFlag;
    private Transform otherFlag = null;
    public Transform flagHand;
    public Transform flagOriginalPos;
    public PickupFlag otherAgent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        otherFlag = collision.transform;
        if (collision.gameObject.CompareTag("flag"))
        {
            //Debug.Log("flag");
            if (collision.gameObject == ownFlag)
            {
                ResetFlag();
            }
            else
            {
                otherFlag.parent = flagHand;
                hasFlag = true;
            }
        }
    }

    public void DropFlag()
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

    public void ResetFlag()
    {
        otherFlag.position = flagOriginalPos.position;
        otherFlag.parent = null;
        otherAgent.hasFlag = false;
    }
}
