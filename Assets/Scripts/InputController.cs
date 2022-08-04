using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private FrozenBackgroundController backgroundController;
    
    [SerializeField]
    private ClickDetector clickDetector;

    private GameState gameState;
    
    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        gameState.OnGameEnd += backgroundController.Freeze;
        
        if (!clickDetector)
            clickDetector = GetComponent<ClickDetector>();
        
        clickDetector.OnClick += ClickHandler;
    }
    
    private void ClickHandler(Vector2 point)
    {
        if (backgroundController.IsFreezing)
            backgroundController.WarmUp(point);
        
        gameState.ReceiveInput();
    }
}
