using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : AquariumObject
{
    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        // Don't know how this fish affects the water...
        throw new System.NotImplementedException();
    }
}
