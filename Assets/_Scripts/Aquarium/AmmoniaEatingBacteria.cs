using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoniaEatingBacteria : MonoBehaviour, IAquariumObject
{
    [Header("Bacteria")]
    [SerializeField] private float _biomass;
    [SerializeField] private float _conversionFactorPPM;
    [SerializeField] private float _oxygenConsumptionPPM;
    [SerializeField] private float _growthRate;
    [SerializeField] private float _starvationFactor;
    [SerializeField] private float _consumptionPerBiomassPPM;

    public void DoProcess(Dictionary<Parameter, float> parameters)
    {
        float actualConsumptionPPM;
        float requiredConsumptionPPM = _biomass * _consumptionPerBiomassPPM;

        float ammoniaPPM;
        float nitritePPM;
        float oxygenPPM;

        // Extract the parameter values from the tank, and check they are initalized
        if(!parameters.TryGetValue(Parameter.Ammonia, out ammoniaPPM))
        {
            Debug.LogError("AmmoniaEatingBacteria ERROR: failed to get Ammonia value from aquarium");
            return;
        }

        if (!parameters.TryGetValue(Parameter.Nitrite, out nitritePPM))
        {
            Debug.LogError("AmmoniaEatingBacteria ERROR: failed to get nitratePPM value from aquarium");
            return;
        }

        if(!parameters.TryGetValue(Parameter.Oxygen, out oxygenPPM))
        {
            Debug.LogError("AmmoniaEatingBacteria ERROR: failed to get oxygenPPM value from aquarium");
            return;
        }

        //Debug.Log("Ammonia: " + ammoniaPPM);
        //Debug.Log("Nitrite: " + nitritePPM);
        //Debug.Log("Oxygen: " + oxygenPPM);

        // If enough Ammonia for consumption, grow bacteria 
        if (ammoniaPPM > requiredConsumptionPPM)
        {
            _biomass = _biomass * _growthRate;
            actualConsumptionPPM = requiredConsumptionPPM;
        }
        // Bacteria Converts what ammonia is still avalible and dies off
        else
        {
            actualConsumptionPPM = ammoniaPPM;

            // Only calculate bacteria die off if the value is over 0.1f
            if (_biomass > 0.1f)
            {
                _biomass = _biomass - ((requiredConsumptionPPM - actualConsumptionPPM) / _consumptionPerBiomassPPM) * _starvationFactor;
            }
        }
        //Debug.Log("Ammonia Consumed: " + actualConsumptionPPM);
        parameters[Parameter.Ammonia] = Mathf.Max(ammoniaPPM - actualConsumptionPPM, 0f);
        parameters[Parameter.Nitrite] = nitritePPM + (actualConsumptionPPM * _conversionFactorPPM);
        parameters[Parameter.Oxygen] = Mathf.Max(oxygenPPM - actualConsumptionPPM * _oxygenConsumptionPPM, 0f);
    }
}
