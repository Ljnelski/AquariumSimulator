using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardscape : AquariumObject, IBioMedia
{
    [Header("Hardscape")]
    [SerializeField] private float _surfaceArea;
    [SerializeField] private float _phLean;
    [SerializeField] private float _ammoniaLeachPPM;

    public float SupportedBiomass => _surfaceArea;

    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float ph;
        float ammoniaPPM;


        if(!TryToGetParameter(parameters, Parameter.Ammonia, out ammoniaPPM)) return;
        if(!TryToGetParameter(parameters, Parameter.Ph, out ph)) return;        

        parameters[Parameter.Ph] = Mathf.Min(14f, Mathf.Max(0f, ph + _phLean));
        parameters[Parameter.Ammonia] = Mathf.Max(ammoniaPPM + _ammoniaLeachPPM, 0f);
    }   
}
