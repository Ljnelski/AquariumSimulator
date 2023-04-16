using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "AquariumParameterData", menuName = "ScriptableObjects/AquariumParameterData", order = 2)]
public class AquariumParameterData : ScriptableObject
{
    private Dictionary<Parameter, float> _parameters = new Dictionary<Parameter, float>();

    public float AccessParameterValue(Parameter targetParameter)
    {
        float targetValue = 0f;
        // If it does not exist, then add it
        if (!_parameters.TryGetValue(targetParameter, out targetValue))
        {
            _parameters.Add(targetParameter, targetValue);
        }

        return targetValue;
    }

    public void IncreaseParameter(Parameter parameter, float value)
    {
        // Make sure the parameter exists/is intalized
        float targetParameter = AccessParameterValue(parameter);

        // Do operation
        _parameters[parameter] = targetParameter + value;
    }

    public void IncreaseParameter(Parameter parameter, float value, float max)
    {
        float targetParameter = AccessParameterValue(parameter);

        // Do operation
        _parameters[parameter] = MathF.Min(targetParameter + value, max);
    }

    public void DecreaseParameter(Parameter parameter, float value)
    {
        DecreaseParameter(parameter, -value, 0f);
    }    
   
    public void DecreaseParameter(Parameter parameter, float value, float min) 
    {
        float targetParameter = AccessParameterValue(parameter);

        // Do operation
        _parameters[parameter] = MathF.Max(targetParameter - value, min);
    }

    public void ClampParameter(Parameter parameter, float value, float min, float max)
    {
        float targetParameter = AccessParameterValue(parameter);

        _parameters[parameter] = Mathf.Clamp(targetParameter + value, min, max);
    }
}
