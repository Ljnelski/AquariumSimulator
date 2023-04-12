using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class Fish : AquariumObject
{

    [Header("Fish")]
    [SerializeField] private float _maxHealth = 100f; // Range from 0-100 

    [SerializeField] private float _foodConsumption = 1f;
    [SerializeField] private float _metabolism; // By what rate the fish gets hungry -- should be fed once or twice a day, with each feeding consisting of 2-3 pellets or flakes
    [SerializeField] private float _oxygenConsumptionPPM = 1f;

    [Header("Output")]
    [SerializeField] private float _ammoniaProducedPPM;

    [Header("Ph Tolerance")]
    [SerializeField] private float _maxPh = 7.5f; // Range from 0-14 -- For Betta fish, the ph should be : 6.5 -> 7.5 
    [SerializeField] private float _minPh = 6.5f; // Range from 0-14 -- For Betta fish, the ph should be : 6.5 -> 7.5 
    
    [Header("Tempurature Tolerance")]
    [SerializeField] private float _maxTemperature = 24; // celsius -- For Betta fish, the temp should be 24C-28C
    [SerializeField] private float _minTemperature = 28; // celsius -- For Betta fish, the temp should be 24C-28C

    private float _currentHealth;
    private float _currentHunger; // Zero is starving

    [Header("Debug")]
    public float phComfortDisplay;

    enum comfortLevel
    {
        Dying = 0,
        Stressed = 1,
        Comfortable = 2,
        Thriving = 3
    }

    private float PhSatisfaction(float ph)
    {
        float amountOutsideTolerance;
        float distanceToLimitOfPh;

        float MAX_COMFORT = 100f;

        Debug.Log("PhSatifaction");

        if (ph > 14 || ph < 0)
        {
            Debug.LogError("Fish ERROR: Ph Value is out of the possible range of 0-14: " + ph);
            return -1;
        }

        // Check if the water ph is outside the tolerance zone of the fish
        if (ph > _maxPh)
        {
            amountOutsideTolerance = ph - _maxPh;
            distanceToLimitOfPh = 14 - _maxHealth;
        }
        else if (ph < _minPh)
        {
            amountOutsideTolerance = _minPh - ph;
            distanceToLimitOfPh = 14 - _minPh;
        }
        else
        {
            phComfortDisplay = MAX_COMFORT;
            return MAX_COMFORT; //fully comfortable
        }


        // Calculate the fish comfort. Linear equation y = mx + b
        
        // the slope, or m
        float comfortDecayRate = (MAX_COMFORT / distanceToLimitOfPh);

        float phComfort = -comfortDecayRate * amountOutsideTolerance + MAX_COMFORT;
        phComfortDisplay = phComfort;
        return phComfort;

    }

    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float avalibleOxygenPPM;
        float availableAmmoniaPPM;
        float aquariumPh;

        float actualOxygenConsumption;

        if (!TryToGetParameter(parameters, Parameter.Ammonia, out availableAmmoniaPPM)) return;
        if (!TryToGetParameter(parameters, Parameter.Oxygen, out avalibleOxygenPPM)) return;
        if (!TryToGetParameter(parameters, Parameter.Ph, out aquariumPh)) return;

        // Check if there is enough oxygen
        if (avalibleOxygenPPM > _oxygenConsumptionPPM)
        {
            actualOxygenConsumption = _oxygenConsumptionPPM;
        } 
        else
        {
            actualOxygenConsumption = _oxygenConsumptionPPM - avalibleOxygenPPM;
        }

        PhSatisfaction(aquariumPh);

        // Change Parameters
        parameters[Parameter.Ammonia] = availableAmmoniaPPM + _ammoniaProducedPPM;
        parameters[Parameter.Oxygen] = avalibleOxygenPPM - _oxygenConsumptionPPM;
        


    }
}
