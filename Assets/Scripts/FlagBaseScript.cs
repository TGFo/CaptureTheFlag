using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagBaseScript : MonoBehaviour
{
    private Transform ownFlagPos;
    public GameObject ally;
    public GameObject ownFlag;
    public bool flagMissing = false;
    public bool isAIBase = false;

    private void Start()
    {
        ownFlagPos = ownFlag.transform;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == ally)
        {
            if(other.GetComponent<PickupFlag>().hasFlag == true)
            {
                if(isAIBase == true)
                {
                    ScoreManager.instance.IncrementAIScore(1);
                }
                else
                {
                    ScoreManager.instance.IncrementPlayerScore(1);
                }
                ScoreManager.instance.EndRound();
            }
        }
        if (other.gameObject == ownFlag)
        {
            //Debug.Log("why");
            flagMissing = false;
        }
    }

    private void Update()
    {
        if(ownFlagPos.position == transform.position)
        {
            flagMissing = false;
        }
        else
        {
            flagMissing = true;
        }
    }
}
