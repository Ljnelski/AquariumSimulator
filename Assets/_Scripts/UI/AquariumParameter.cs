using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AquariumParameter : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private Aquarium aquarium;
    [SerializeField] private TMP_Text value;

    [Header("Behaviour")]
    [SerializeField] private Parameter parameter;

    private void OnEnable()
    {
        aquarium.OnParameterUpdate += UpdateValue;
    }
    private void UpdateValue()
    {
        value.text = aquarium.AccessParameterValue(parameter).ToString("n2") + " ppm";
    }
    private void OnDisable()
    {
        aquarium.OnParameterUpdate -= UpdateValue;
    }

}
