using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeReadout : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private TMP_Text _value;

    private void OnEnable()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.OnTimeChanged += UpdateValue;
        }
    }
    private void Start()
    {
        GameState.Instance.OnTimeChanged += UpdateValue;
    }

    private void UpdateValue(int day, int hour, int minute)
    {
        _value.text =
            day.ToString() + ":" +
            hour.ToString("00") + ":" +
            minute.ToString("00");
    }
    private void OnDisable()
    {
        GameState.Instance.OnTimeChanged -= UpdateValue;
    }
}
