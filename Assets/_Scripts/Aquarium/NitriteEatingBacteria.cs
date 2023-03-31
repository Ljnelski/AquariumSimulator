using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitriteEatingBacteria : MonoBehaviour, IAquariumProcess
{
    [Header("Bacteria")]
    [SerializeField] private float _biomass;
    [SerializeField] private float _conversionFactorPPM;
    [SerializeField] private float _growthRate;
    [SerializeField] private float _starvationFactor;
    [SerializeField] private float _consumptionPerBiomassPPM;

    public void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float actualConsumptionPPM;
        float requiredConsumptionPPM = _biomass * _consumptionPerBiomassPPM;

        float nitritePPM;
        float nitratePPM;

        // Extract the parameter values from the tank, and check they are initalized
        if (!parameters.TryGetValue(Parameter.Nitrite, out nitritePPM))
        {
            Debug.LogError("NitriteEatingBacteria ERROR: failed to get nitratePPM value from aquarium");
            return;
        }

        if(!parameters.TryGetValue(Parameter.Nitrate, out nitratePPM))
        {
            Debug.LogError("NitriteEatingBacteria ERROR: failed to get nitratePPM value from aquarium");
            return;
        }

        //Debug.Log("Ammonia: " + ammoniaPPM);
        //Debug.Log("Nitrite: " + nitritePPM);
        //Debug.Log("Oxygen: " + oxygenPPM);

        // If enough Nitrite for consumption, grow bacteria 
        if (nitritePPM > requiredConsumptionPPM)
        {
            _biomass = _biomass * _growthRate;
            actualConsumptionPPM = requiredConsumptionPPM;
        }
        // Bacteria Converts what ammonia is still avalible and dies off
        else
        {
            actualConsumptionPPM = nitritePPM;

            // Only calculate bacteria die off if the value is over 0.1f
            if (_biomass > 0.1f)
            {
                _biomass = _biomass - ((requiredConsumptionPPM - actualConsumptionPPM) / _consumptionPerBiomassPPM) * _starvationFactor;
            }
        }
        //Debug.Log("Ammonia Consumed: " + actualConsumptionPPM);
        parameters[Parameter.Nitrite] = Mathf.Max(nitritePPM - actualConsumptionPPM, 0f);
        parameters[Parameter.Nitrate] = nitratePPM + (actualConsumptionPPM * _conversionFactorPPM);
    }
}
