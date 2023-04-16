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

    public override void DoProcess(AquariumParameterData parameters)
    {
        parameters.ClampParameter(Parameter.Ph, _phLean, 0f, 14f);
        parameters.DecreaseParameter(Parameter.Ammonia, _ammoniaLeachPPM, 0f);
    }   
}
