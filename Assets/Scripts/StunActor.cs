using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunActor : MonoBehaviour
{
    public float timeLimit = 60f; // Time limit in seconds
    private float currentTime = 0f;
    private bool isTimerRunning = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        RunTimer();
    }

    public void RunTimer()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeLimit)
            {
                FinishTimer();
            }
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    public void FinishTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer finished!");
    }

    // Function to get current time remaining
    public float GetTimeRemaining()
    {
        return timeLimit - currentTime;
    }
}
