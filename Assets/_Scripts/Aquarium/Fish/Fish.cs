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
    [SerializeField] private float _baseHealAmount;

    [Header("Inputs")]
    [SerializeField] private float _oxygenConsumptionPPM = 1f;

    [Header("Food")]
    [SerializeField] private float _maxHunger;
    [SerializeField] private float _foodConsumption = 2f;
    [SerializeField] private float _metabolism; // By what rate the fish gets hungry -- should be fed once or twice a day, with each feeding consisting of 2-3 pellets or flakes
    [SerializeField] private float _starvationDamage;

    [Header("Outputs")]
    [SerializeField] private float _ammoniaProducedPPM;

    [Header("Nitrogen Componds Tolerences")]
    [SerializeField] private float _ammoniaPMMTolerance = 4.0f;
    [SerializeField] private float _nitritePPMTolerance;
    [SerializeField] private float _nitratePPMTolerance;

    [Header("Ph Tolerance")]
    [Min(0)][SerializeField] private float _basePhDamage = 0.5f;
    [SerializeField] private float _maxPh = 7.5f; // Range from 0-14 -- For Betta fish, the ph should be : 6.5 -> 7.5 
    [SerializeField] private float _minPh = 6.5f; // Range from 0-14 -- For Betta fish, the ph should be : 6.5 -> 7.5 

    [Header("Tempurature Tolerance")]
    [Min(0)][SerializeField] private float _baseTemperatureDamage = 0.5f;
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
    private bool _dead = false;

    [Header("Debug")]
    public float currentHealthDisplay;

    [Header("Comfort")]
    public float comfortLevelDisplay;
    public float phComfortDisplay;
    public float hungerDisplay;
    public float temperatureDisplay;

    [Header("DamagingFish")]
    public float AmmoniaDamage;
    public float NitriteDamage;
    public float NitrateDamage;
    public float TemperatureDamage;
    public float PhDamage;
    public float HungerDamage;

    public Action OnFishDeath;

    private float _processCycleDamage = 0f;
    private float _processFoodConsumed = 0f;

    private const float MAX_COMFORT = 100f;

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

    private float PhReaction(float ph)
    {
        float amountOutsideTolerance;
        float distanceToLimitOfPh;

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

        // Calculate damage from ph
        float damageFromPh = Mathf.Pow(_basePhDamage + 1, amountOutsideTolerance) - 1f;
        PhDamage = damageFromPh;
        _processCycleDamage = _processCycleDamage + damageFromPh;

        // Calculate the fish comfort. Linear equation y = mx + b

        // the slope, or m
        float comfortDecayRate = (MAX_COMFORT / distanceToLimitOfPh);

        float phComfort = Mathf.Max(-comfortDecayRate * amountOutsideTolerance + MAX_COMFORT, 0f);
        phComfortDisplay = phComfort;
        return phComfort;

    }
    private float TemperatureReaction(float temperature)
    {
        float amountOutsideTolerance;

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

        // Calcualte damage recvied from being outside temperature range
        float damageFromTemperature = Mathf.Pow(_baseTemperatureDamage + 1, amountOutsideTolerance) - 1f;
        TemperatureDamage = damageFromTemperature;
        _processCycleDamage = _processCycleDamage + damageFromTemperature;


        // Calculate the fish comfort. Linear equation y = mx + b

        // the slope, or m
        float comfortDecayRate = (MAX_COMFORT / _temperatureConfortFallOff);

        float temperatureComfort = -comfortDecayRate * amountOutsideTolerance + MAX_COMFORT;
        return temperatureComfort;
    }
    private float HungerReaction(float availableFishFood)
    {
        float consumedFood = 0f;
        // Hunger and food
        if (_currentHunger < _maxHunger)
        {
            float remainingHunger = _maxHunger - _currentHunger;
            consumedFood = Mathf.Min(_foodConsumption, availableFishFood, remainingHunger);
            _currentHunger += consumedFood;
            _processFoodConsumed = consumedFood;
        }
        _currentHunger = Mathf.Max(_currentHunger - _metabolism, 0f);

        // Starvation
        if (_currentHunger <= 0f)
        {
            HungerDamage = _starvationDamage;
            _processCycleDamage = _processCycleDamage + _starvationDamage;
        }

        float _hungerAsComfort = (_currentHunger / _maxHunger) * MAX_COMFORT;

        return _hungerAsComfort;
    }

    public override void DoProcess(AquariumParameterData parameters)
    {
        if (_dead) return;

        // Exposes Damages to the UI
        AmmoniaDamage = 0f;
        NitriteDamage = 0f;
        NitrateDamage = 0f;
        TemperatureDamage = 0f;
        PhDamage = 0f;
        HungerDamage = 0f;

        _processCycleDamage = 0;
        _processFoodConsumed = 0;

        float avalibleOxygenPPM = GetParameter(Parameter.Oxygen, parameters);
        float availableAmmoniaPPM = GetParameter(Parameter.Ammonia, parameters);
        float availableNitritePPM = GetParameter(Parameter.Nitrite, parameters);
        float availableNitratePPM = GetParameter(Parameter.Nitrate, parameters);
        float aquariumPh = GetParameter(Parameter.Ph, parameters);
        float aquariumTemperature = GetParameter(Parameter.Temperature, parameters);
        float availableFishFood = GetParameter(Parameter.FishFood, parameters);

        float actualOxygenConsumption;
        float actualFoodConsumption = 0f;

        // Check if there is enough oxygen
        if (avalibleOxygenPPM > _oxygenConsumptionPPM)
        {
            actualOxygenConsumption = _oxygenConsumptionPPM;
        }
        else
        {
            actualOxygenConsumption = _oxygenConsumptionPPM - avalibleOxygenPPM;
        }

        // Water Quality Damage
        if (availableAmmoniaPPM > _ammoniaPMMTolerance)
        {
            _processCycleDamage = _processCycleDamage + Mathf.Pow(availableAmmoniaPPM - _ammoniaPMMTolerance, 2);
            AmmoniaDamage = Mathf.Pow(availableAmmoniaPPM - _ammoniaPMMTolerance, 2);
        }
        if (availableNitritePPM > _nitritePPMTolerance)
        {
            _processCycleDamage = _processCycleDamage + Mathf.Pow(availableNitritePPM - _nitritePPMTolerance, 2);
            NitriteDamage = Mathf.Pow(availableNitritePPM - _nitritePPMTolerance, 2);
        }
        if (availableNitratePPM > _nitratePPMTolerance)
        {
            _processCycleDamage = _processCycleDamage + Mathf.Pow(availableNitratePPM - _nitratePPMTolerance, 2);
            NitrateDamage = Mathf.Pow(availableNitritePPM - _nitratePPMTolerance, 2);
        }

        // Run Fish Reactions and store the comfort value returned
        float hungerComfort = HungerReaction(availableFishFood);
        float phComfort = PhReaction(aquariumPh);
        float temperatureComfort = TemperatureReaction(aquariumTemperature);

        // Weigh all the comfort values
        float weightedHunger = hungerComfort * _hungerWeight;
        float weightedPH = phComfort * _phWeight;
        float weightedTemperature = temperatureComfort * _temperatureWeight;

        // calculate the total weight that will act as the denominator
        float totalWeight = _hungerWeight + _phWeight + _temperatureWeight;

        // calculate the total comfort. Not sure why the math works but is calculating a weight average -> sum / total# of values
        _comfortLevel = (weightedHunger + weightedPH + weightedTemperature) / totalWeight;


        if (_processCycleDamage > 0)
        {
            // Do damage
            _currentHealth = Mathf.Max(_currentHealth - _processCycleDamage, 0f);
            if (_currentHealth <= 0f)
            {
                _dead = true;
                OnFishDeath?.Invoke();
                _processCycleDamage = 0;
            }
        }
        else if (_currentHealth < _maxHealth)
        {

            float healFactor = (_comfortLevel / MAX_COMFORT) * _baseHealAmount;
            _currentHealth = Mathf.Min(_currentHealth + healFactor, _maxHealth);
        }



        _healthIndicator.AdjustGradient(_currentHealth);

        // Debug print
        comfortLevelDisplay = _comfortLevel;
        currentHealthDisplay = _currentHealth;
        hungerDisplay = _currentHunger;
        temperatureDisplay = temperatureComfort;
        phComfortDisplay = phComfort;

        // Change Parameters
        parameters.AddToParameter(Parameter.Ammonia, _ammoniaProducedPPM);
        parameters.SubtractFromParameter(Parameter.Oxygen, _oxygenConsumptionPPM);
        parameters.SubtractFromParameter(Parameter.FishFood, actualFoodConsumption);
    }
}
