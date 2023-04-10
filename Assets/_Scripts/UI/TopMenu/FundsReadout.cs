using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FundsReadout : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private TMP_Text _value;

    private void OnEnable()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.OnAvalibleFundsChange += UpdateValue;
        }
    }
    private void Start()
    {
        GameState.Instance.OnAvalibleFundsChange += UpdateValue;
    }

    private void UpdateValue(float newValue)
    {
        _value.text = "$ " + newValue.ToString("0,0.00");
    }
    private void OnDisable()
    {
        GameState.Instance.OnAvalibleFundsChange -= UpdateValue;
    }
}
