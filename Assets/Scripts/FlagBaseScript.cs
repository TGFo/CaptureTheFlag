using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagBaseScript : MonoBehaviour
{
    //the transform of the flag that sits in the base on game start
    private Transform ownFlagPos;
    //the actor the base belongs to
    public GameObject ally;
    //the flag the base is responsible for housing
    public GameObject ownFlag;
    //bool to check if the flag is missing
    public bool flagMissing = false;
    //bool stating whether the base is the AI's or the player's
    public bool isAIBase = false;

    private void Start()
    {
        //caching transforms for performance
        ownFlagPos = ownFlag.transform;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == ally)                                    //checks if object entering collider is the bases ally
        {
            if(other.GetComponent<PickupFlag>().hasFlag == true)        //checks if the hasFlag bool of the entering agent is true
            {
                if(isAIBase == true)                                    //checks to see which side should gain a point for this round
                {
                    ScoreManager.instance.IncrementAIScore(1);
                }
                else
                {
                    ScoreManager.instance.IncrementPlayerScore(1);
                }
                ScoreManager.instance.EndRound();                       //ends round
            }
        }
    }

    private void Update()
    {
        if(ownFlagPos.position == transform.position)                   //checks everyframe whether the base's own flag is in its orignal position, setting flagMissing to true if it is not
        {
            flagMissing = false;
        }
        else
        {
            flagMissing = true;
        }
    }

    //Script in charge of the player/AI's home base, allowing for the scoring of points or the acknowledgement that its flag is missing
}
