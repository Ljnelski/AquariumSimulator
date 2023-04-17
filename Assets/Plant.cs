using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : AquariumObject
{
    [SerializeField] private float ammoniaIntake; // ppm
    [SerializeField] private float nitriteIntake; // ppm
    [SerializeField] private float nitrateIntake; // ppm
    [SerializeField] private float phRange; // 0-14
    public override void DoProcess(AquariumParameterData parameters)
    {
        Debug.LogError("PLANT PROCESS NOT IMPLEMENTED");
        //throw new System.NotImplementedException();
    }
}
