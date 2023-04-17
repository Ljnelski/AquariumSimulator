using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plant : AquariumObject
{
    [Header("Plant")]
    [SerializeField] private float _oxygenProduction;

    [SerializeField] private float _ammoniaIntake; // ppm
    [SerializeField] private float _nitriteIntake; // ppm
    [SerializeField] private float _nitrateIntake; // ppm
    [SerializeField] private float _phRange; // 0-14
    public override void DoProcess(AquariumParameterData parameters)
    {
        parameters.SubtractFromParameter(Parameter.Ammonia, _ammoniaIntake);
        parameters.SubtractFromParameter(Parameter.Nitrite, _nitriteIntake);
        parameters.SubtractFromParameter(Parameter.Nitrate, _nitrateIntake);

        parameters.AddToParameter(Parameter.Oxygen, _oxygenProduction);
    }
}
