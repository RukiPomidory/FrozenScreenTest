using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Action OnGameStart;
    public Action OnGameEnd;

    public float timeout = 30;

    private State state;
    private float timerStart;
    
    public void ReceiveInput()
    {
        timerStart = Time.time;
        
        if (state == State.Playing)
        {
            return;
        }

        state = State.Playing;
        OnGameStart?.Invoke();
    }

    private void Update()
    {
        if (state != State.Playing)
            return;
        
        UpdateTimeoutTimer();
    }

    private void UpdateTimeoutTimer()
    {
        if (Time.time - timerStart < timeout)
            return;

        state = State.Waiting;
        OnGameEnd?.Invoke();
    }
    
    
    public enum State
    {
        Playing,
        Waiting
    }
}
