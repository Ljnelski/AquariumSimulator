using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invertebrates : AquariumObject
{
    [SerializeField] private float foodConsumeRate = 1f;
    [SerializeField] private float oxygenConsumeRate = 1f;

    [SerializeField] private float health = 100f; // Range from 0-100 ( float ? )

    [SerializeField] private float hungerAmount = 0; // Range from ?-?
    [SerializeField] private float hungerIncreaseRate = 0; // By what rate the fish gets hungry

    [SerializeField] private float requiredPh = 0; // Range from 0-14

    [SerializeField] private float requiredTemperature = 0; // celsius

    //float happinessAmount = 100; // Range from 0-100 ( float? )

    enum comfortLevel
    {
        Dying = 0,
        Stressed = 1,
        Comfortable = 2,
        Thriving = 3
    }
    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        // Don't know how this fish affects the water...
        throw new System.NotImplementedException();
    }
}
