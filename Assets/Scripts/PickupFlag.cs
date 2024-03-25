using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFlag : MonoBehaviour
{
    public bool hasFlag;
    public GameObject ownFlag;
    public Transform flagPos;
    public GameObject flagOriginalPos;
    public PickupFlag otherAgent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform collisionTransform = collision.transform;
        if (collision.gameObject.tag == "flag")
        {
            Debug.Log("flag");
            if (collision.gameObject == ownFlag)
            {
                collisionTransform.position = flagOriginalPos.transform.position;
                collisionTransform.parent = null;
                otherAgent.hasFlag = false;
            }
            else
            {
                collisionTransform.parent = flagPos;
                hasFlag = true;
            }
        }
    }
}
