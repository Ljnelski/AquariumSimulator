using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardscape : MonoBehaviour, IAquariumObject
{
    [Header("Hardscape")]
    [SerializeField] float phLean;
    [SerializeField] float ammoniaLeachPPM;

    public void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float ph;
        float ammoniaPPM;

        if (!parameters.TryGetValue(Parameter.Ammonia, out ammoniaPPM))
        {
            Debug.LogError("Hardscape ERROR: failed to get Ammonia value from aquarium");
            return;
        }

        if (!parameters.TryGetValue(Parameter.Ph, out ph))
        {
            Debug.LogError("Hardscape ERROR: failed to get nitratePPM value from aquarium");
            return;
        }

        parameters[Parameter.Ph] = Mathf.Min(14f, Mathf.Max(0f, ph + phLean));
        parameters[Parameter.Ammonia] = Mathf.Max(ammoniaPPM + ammoniaLeachPPM, 0f);
    }   
}
