using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameObject>().GetComponent<GameState>();
            }
            return _instance;
        }
    }
    private static GameState _instance;

    public InteractionMode CurrentInteractionMode
    {
        get => _currentInteractionMode;
        set
        {
            _currentInteractionMode = value;
            OnInteractionModeChange(_currentInteractionMode);
        }
    }
    private InteractionMode _currentInteractionMode;

    public Action<InteractionMode> OnInteractionModeChange;

    public float Funds;
    public int DayNumber;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}

