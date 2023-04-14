using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class Fish : AquariumObject
{

    [Header("Fish")]
    [SerializeField] private HealthIndicator _healthIndicator;

    [SerializeField] private float _maxHealth = 100f; // Range from 0-100 
    [SerializeField] private float _maxHunger;
    [SerializeField] private float _foodConsumption = 1f;
    [SerializeField] private float _metabolism; // By what rate the fish gets hungry -- should be fed once or twice a day, with each feeding consisting of 2-3 pellets or flakes
    [SerializeField] private float _starvationRate;
    [SerializeField] private float _oxygenConsumptionPPM = 1f;

    [Header("Output")]
    [SerializeField] private float _ammoniaProducedPPM;

    [Header("Nitrogen Componds Tolerences")]
    [SerializeField] private float _ammoniaPMMTolerance = 4.0f;
    [SerializeField] private float _nitritePPMTolerance;
    [SerializeField] private float _nitratePPMTolerance;

    [Header("Ph Tolerance")]
    [SerializeField] private float _maxPh = 7.5f; // Range from 0-14 -- For Betta fish, the ph should be : 6.5 -> 7.5 
    [SerializeField] private float _minPh = 6.5f; // Range from 0-14 -- For Betta fish, the ph should be : 6.5 -> 7.5 

    [Header("Tempurature Tolerance")]
    [SerializeField] private float _temperatureConfortFallOff = 6; // celsius -- Number of degrees outside of the min/max that confort will be 0
    [SerializeField] private float _maxTemperature = 28; // celsius -- For Betta fish, the temp should be 24C-28C
    [SerializeField] private float _minTemperature = 24; // celsius -- For Betta fish, the temp should be 24C-28C

    [Header("ComfortWeights")]
    [SerializeField] private float _temperatureWeight = 1f;
    [SerializeField] private float _hungerWeight = 1f;
    [SerializeField] private float _phWeight = 1f;

    private float _currentHealth;
    private float _currentHunger; // Zero is starving
    private float _comfortLevel;

    [Header("Debug")]
    public float currentHealthDisplay;

    [Header("Comfort")]
    public float comfortLevelDisplay;
    public float phComfortDisplay;
    public float hungerDisplay;
    public float temperatureDisplay;

    [Header("Water Quality")]
    public float AmmoniaDamage;
    public float NitriteDamage;
    public float NitrateDamage;

    enum comfortLevel
    {
        Dying = 0,
        Stressed = 1,
        Comfortable = 2,
        Thriving = 3
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _currentHunger = _maxHunger;
    }

    private float PhComfort(float ph)
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
            distanceToLimitOfPh = 14 - _maxPh;
        }
        else if (ph < _minPh)
        {
            amountOutsideTolerance = _minPh - ph;
            distanceToLimitOfPh = _minPh;
        }
        else
        {
            return MAX_COMFORT; //fully comfortable
        }


        // Calculate the fish comfort. Linear equation y = mx + b

        // the slope, or m
        float comfortDecayRate = (MAX_COMFORT / distanceToLimitOfPh);

        float phComfort = Mathf.Max(-comfortDecayRate * amountOutsideTolerance + MAX_COMFORT, 0f);
        phComfortDisplay = phComfort;
        return phComfort;

    }
    private float TemperatureComfort(float temperature)
    {
        float amountOutsideTolerance;

        float MAX_COMFORT = 100f;

        // Check if the water ph is outside the tolerance zone of the fish
        if (temperature > _maxTemperature)
        {
            amountOutsideTolerance = temperature - _maxTemperature;
        }
        else if (temperature < _minTemperature)
        {
            amountOutsideTolerance = _minTemperature - temperature;
        }
        else
        {
            return MAX_COMFORT; //fully comfortable
        }

        // Calculate the fish comfort. Linear equation y = mx + b

        // the slope, or m
        float comfortDecayRate = (MAX_COMFORT / _temperatureConfortFallOff);

        float temperatureComfort = -comfortDecayRate * amountOutsideTolerance + MAX_COMFORT;
        return temperatureComfort;
    }

    public override void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float avalibleOxygenPPM;
        float availableAmmoniaPPM;
        float availableNitritePPM;
        float availableNitratePPM;
        float aquariumPh;
        float aquariumTemperature;
        float availableFishFood;

        float actualOxygenConsumption;
        float actualFoodConsumption = 0f;

        float damageTaken = 0f;

        if (!TryToGetParameter(parameters, Parameter.Ammonia, out availableAmmoniaPPM)) return;
        if (!TryToGetParameter(parameters, Parameter.Nitrite, out availableNitritePPM)) return;
        if (!TryToGetParameter(parameters, Parameter.Nitrate, out availableNitratePPM)) return;
        if (!TryToGetParameter(parameters, Parameter.Oxygen, out avalibleOxygenPPM)) return;
        if (!TryToGetParameter(parameters, Parameter.Ph, out aquariumPh)) return;
        if (!TryToGetParameter(parameters, Parameter.Temperature, out aquariumTemperature)) return;
        if (!TryToGetParameter(parameters, Parameter.FishFood, out availableFishFood)) return;

        // Check if there is enough oxygen
        if (avalibleOxygenPPM > _oxygenConsumptionPPM)
        {
            actualOxygenConsumption = _oxygenConsumptionPPM;
        }
        else
        {
            actualOxygenConsumption = _oxygenConsumptionPPM - avalibleOxygenPPM;
        }


        // Hunger and food
        if (_currentHunger < _maxHunger)
        {
            actualFoodConsumption = Mathf.Min(_foodConsumption, availableFishFood);
            _currentHunger += actualFoodConsumption;
        }
        _currentHunger = Mathf.Max(_currentHunger - _metabolism, 0f);

        // Starvation
        if (_currentHunger <= 0f)
        {
            damageTaken = damageTaken + _starvationRate;
        }

        // Water Quality Damage
        if (availableAmmoniaPPM > _ammoniaPMMTolerance)
        {
            damageTaken = damageTaken + Mathf.Pow(availableAmmoniaPPM - _ammoniaPMMTolerance, 2);
            AmmoniaDamage = Mathf.Pow(availableAmmoniaPPM - _ammoniaPMMTolerance, 2);
        }
        if (availableNitritePPM > _nitritePPMTolerance)
        {
            damageTaken = damageTaken + Mathf.Pow(availableNitritePPM - _nitritePPMTolerance, 2);
            NitriteDamage = Mathf.Pow(availableNitritePPM - _nitritePPMTolerance, 2);
        }
        if (availableNitratePPM > _nitratePPMTolerance)
        {
            damageTaken = damageTaken + Mathf.Pow(availableNitratePPM - _nitratePPMTolerance, 2);
            NitrateDamage = Mathf.Pow(availableNitritePPM - _nitratePPMTolerance, 2);
        }


        // calculate comfort
        float hungerComfort = _currentHunger;
        float phComfort = PhComfort(aquariumPh);
        float temperatureComfort = TemperatureComfort(aquariumTemperature);

        // Weigh all the comfort values
        float weightedHunger = hungerComfort * _hungerWeight;
        float weightedPH = phComfort * _phWeight;
        float weightedTemperature = temperatureComfort * _temperatureWeight;

        // calculate the total weight that will act as the denominator
        float totalWeight = _hungerWeight + _phWeight + _temperatureWeight;

        // calculate the total comfort. Not sure why the math works but is calculating a weight average -> sum / total# of values
        _comfortLevel = (weightedHunger + weightedPH + weightedTemperature) / totalWeight;

        _currentHealth -= damageTaken;

        _healthIndicator.AdjustGradient(_currentHealth);

        // Debug print
        comfortLevelDisplay = _comfortLevel;
        currentHealthDisplay = _currentHealth;
        hungerDisplay = _currentHunger;
        temperatureDisplay = temperatureComfort;
        phComfortDisplay = phComfort;

        // Change Parameters
        parameters[Parameter.Ammonia] = availableAmmoniaPPM + _ammoniaProducedPPM;
        parameters[Parameter.Oxygen] = avalibleOxygenPPM - _oxygenConsumptionPPM;
        parameters[Parameter.FishFood] = availableFishFood - actualFoodConsumption;


    }
}
