using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Based on the formula on wikipeda here
 * https://en.wikipedia.org/wiki/Nitrifying_bacteria NH3 + O2 -> NO2 + 3H + 2e
 */
public class AmmoniaEatingBacteria : AquariumObject
{
    [Header("Bacteria")]
    [SerializeField] private float _biomass;
    [SerializeField] private float _minimumBiomass;
    [SerializeField] private float _growthRate;
    [SerializeField] private float _starvationFactor;
    [SerializeField] private float _processPerBiomassFactor;

    [Header("Input")]
    [SerializeField] private float _ammoniaConsumptionPPM;
    [SerializeField] private float _oxygenConsumptionPPM;

    [Header("Output")]
    [SerializeField] private float _nitrateProducedPPM;

    public override void DoProcess(AquariumParameterData parameters)
    {
        float requiredAmmoniaPPM = _biomass * _ammoniaConsumptionPPM * _processPerBiomassFactor;
        float requiredOxygenPPM = _biomass * _oxygenConsumptionPPM * _processPerBiomassFactor;
        float expectedNitriteProducedPPM = _biomass * _nitrateProducedPPM * _processPerBiomassFactor;

        float actualAmmoniaConsumptionPPM;
        float actualOxygenConsumptionPPM;
        float actualNitriteProduced;

        float availableAmmoniaPPM = GetParameter(Parameter.Ammonia, parameters);
        float availableNitritePPM = GetParameter(Parameter.Nitrate, parameters);
        float availableOxygenPPM = GetParameter(Parameter.Oxygen, parameters);

        bool hasLimitingFactor = false;
        float processEfficiency = 1;

        // Determine limiting factor for conversion

        // Not enough oxygen
        if (requiredOxygenPPM > availableOxygenPPM)
        {
            hasLimitingFactor = true;
            float calculatedEfficiency = availableOxygenPPM / requiredOxygenPPM;
            if (calculatedEfficiency < processEfficiency)
            {
                processEfficiency = calculatedEfficiency;
            }
        }

        // Not enough ammonia
        if (requiredAmmoniaPPM > availableAmmoniaPPM)
        {
            hasLimitingFactor = true;
            float calculatedEfficiency = availableAmmoniaPPM / requiredAmmoniaPPM;
            if (calculatedEfficiency < processEfficiency)
            {
                processEfficiency = calculatedEfficiency;
            }
        }

        // Log the effiency
        //if (hasLimitingFactor)
        //{
        //    Debug.Log("Missing Inputs can only complete process with  a efficiency of " + processEfficiency);
        //}     


        // Calculate the Growth of the bacteria               
        if (hasLimitingFactor) // if there is a limiting factor then kill the bacteria that is in excess;
        {
            // Only calculate bacteria die off if the value is over 0.1f
            if (_biomass > _minimumBiomass)
            {
                //          biomass * (excess bacteria) * (the amount of it that dies per tick as a value 0 - 1)
                Debug.Log("Process Efficiency: " + processEfficiency);
                // calculate amount of bacteria that don't have enough 'food'
                float excessBacteria = _biomass * (1 - processEfficiency);

                // kill the excess bacteria
                _biomass = _biomass - excessBacteria * _starvationFactor;
            }
        }
        else // Grow bacteria
        {
            _biomass = _biomass * _growthRate;
        }

        // Calculate the input and outputs to the aquarium system
        actualAmmoniaConsumptionPPM = requiredAmmoniaPPM * processEfficiency;
        actualOxygenConsumptionPPM = requiredOxygenPPM * processEfficiency;
        actualNitriteProduced = expectedNitriteProducedPPM * processEfficiency;

        //Debug.Log("---------");
        //Debug.Log("---------");
        //Debug.Log("---------");
        //Debug.Log("Process Efficiency: " + processEfficiency);
        //Debug.Log("Ammonia Consumed: " + actualAmmoniaConsumptionPPM);
        //Debug.Log("Oxygetn Consumed: " + actualOxygenConsumptionPPM);
        //Debug.Log("Nitrite Produced: " + actualNitriteProduced);
        //Debug.Log("---------");
        //Debug.Log("---------");
        //Debug.Log("---------");

        parameters.DecreaseParameter(Parameter.Oxygen, actualOxygenConsumptionPPM, 0f);
        parameters.DecreaseParameter(Parameter.Ammonia, actualAmmoniaConsumptionPPM, 0f);
        parameters.IncreaseParameter(Parameter.Nitrite, actualNitriteProduced);


        //parameters[Parameter.Ammonia] = Mathf.Max(availableAmmoniaPPM - actualAmmoniaConsumptionPPM, 0f);
        //parameters[Parameter.Nitrite] = availableNitritePPM + actualNitriteProduced;
        //parameters[Parameter.Oxygen] = Mathf.Max(availableOxygenPPM - actualOxygenConsumptionPPM * _oxygenConsumptionPPM, 0f);
    }

    public override void HightlightValid()
    {
        ;
    }
    public override void HightLightInvalid()
    {
        ;
    }
    public override void RemoveHighlight()
    {
        ;
    }
}
