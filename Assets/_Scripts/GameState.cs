using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance {get; private set;}


    public InteractionMode CurrentInteractionMode;
    public Action<InteractionMode> OnInteractionModeChange;

    public float Funds;
    public int DayNumber;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance == this)
        {
            Destroy(this);
        }
    }
}
