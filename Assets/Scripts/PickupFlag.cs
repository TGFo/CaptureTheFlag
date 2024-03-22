using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFlag : MonoBehaviour
{
    public bool hasFlag;
    public GameObject flag;
    public GameObject flagPos;
    public GameObject flagOriginalPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "flag")
        {
            Debug.Log("flag");
            if (collision.gameObject == flag)
            {
                collision.transform.position = flagOriginalPos.transform.position;
            }
            else
            {
                collision.transform.parent = flagPos.transform;
                hasFlag = true;
            }
        }
    }
}
