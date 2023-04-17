using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AquariumParameter : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private AquariumParameterData _aquariumParameterData;
    [SerializeField] private TMP_Text _parameterName;
    [SerializeField] private TMP_Text _value;

    [Header("Behaviour")]
    [SerializeField] private Parameter parameter;
    [SerializeField] private string suffix = " PPM";

    private void OnValidate()
    {
        _parameterName.text = parameter.ToString();
    }

    private void OnEnable()
    {
        _aquariumParameterData.OnParameterUpdate += UpdateValue;
    }
    private void UpdateValue()
    {
        _value.text = _aquariumParameterData.AccessParameterValue(parameter).ToString("n2") + suffix;
    }
    private void OnDisable()
    {
        _aquariumParameterData.OnParameterUpdate -= UpdateValue;
    }

}
