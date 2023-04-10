using System;
using System.Collections;
using System.Collections.Generic;
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

    // Interaction Mode (What can the player do)
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


    // Avalalible funds (Money)
    public float AvalalibleFunds
    {
        get => _avalalibleFinds;
        set
        {
            _avalalibleFinds = value;
            OnAvalibleFundsChange?.Invoke(value);
        }
    }
    private float _avalalibleFinds;
    public Action<float> OnAvalibleFundsChange;

    // Time
    public int Day { get; private set; }
    public int Hour { get; private set; }
    public int Minute { get; private set; }
    public Action<int, int, int> OnTimeChanged;


    [Header("Gameplay Settings")]

    [Header("Funds")]
    [SerializeField] private float _startingFunds;
    [SerializeField] private float _dailyBudget;

    [Header("Time")]
    [SerializeField] private float _tickRate;
    [SerializeField] private int _minutesPerTick;

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

    private void Start()
    {
        Day = 0;
        Hour = 0;
        Minute = 0;

        AvalalibleFunds = _startingFunds;

        StartCoroutine(Tick());
    }

    public void Purchase(float amount)
    {
        AvalalibleFunds -= amount;
    }


    private void NormalizeTime()
    {
        while (Hour > 24)
        {
            Hour -= 24;
            Day++;
        }

        while (Minute > 60)
        {
            Minute -= 60;
            Hour++;
        }
    }

    private void AddTime(int minutes)
    {
        Minute += minutes;
        NormalizeTime();

        OnTimeChanged?.Invoke(Day, Hour, Minute);
    }

    IEnumerator Tick()
    {
        while (true)
        {
            int dayNumber = Day;
            yield return new WaitForSeconds(_tickRate);
            AddTime(_minutesPerTick);

            // Check if the day has increased
            if (Day > dayNumber)
            {
                AvalalibleFunds += _dailyBudget;
            }
        }
    }


}

