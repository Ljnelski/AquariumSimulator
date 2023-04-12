using System.Collections.Generic;
using UnityEngine;

public class Heater : AquariumObject
{
    [Header("Heater")]
    [SerializeField] private float _maxTempurature;
    [SerializeField] private float _strengthDegrees;
    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float aquariumTempurature;

        if (!TryToGetParameter(parameters, Parameter.Temperature, out aquariumTempurature)) return;

        parameters[Parameter.Temperature] = Mathf.Min(aquariumTempurature + _strengthDegrees, _maxTempurature);
    }
}
