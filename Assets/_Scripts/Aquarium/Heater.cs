using System.Collections.Generic;
using UnityEngine;

public class Heater : AquariumObject
{
    [Header("Heater")]
    [SerializeField] private float _maxTempurature;
    [SerializeField] private float _strengthDegrees;
    public override void DoProcess(AquariumParameterData parameters)
    {
        parameters.IncreaseParameter(Parameter.Temperature, _strengthDegrees, _maxTempurature);
    }
}
