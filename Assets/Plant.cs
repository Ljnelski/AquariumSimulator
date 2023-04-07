using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : AquariumObject
{
    [SerializeField] private float ammoniaIntake; // ppm
    [SerializeField] private float nitriteIntake; // ppm
    [SerializeField] private float nitrateIntake; // ppm
    [SerializeField] private float phRange; // 0-14
    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        throw new System.NotImplementedException();
    }
}
