using System.Collections.Generic;
using UnityEngine;

/* Based on the formula on wikipeda here
 * https://en.wikipedia.org/wiki/Nitrifying_bacteria N02 + H2O -> NO3 + 2H + 2e
 */
public class NitriteEatingBacteria : AquariumObject
{
    [Header("Bacteria")]
    [SerializeField] private float _biomass;
    [SerializeField] private float _minimumBiomass;
    [SerializeField] private float _growthRate;
    [SerializeField] private float _starvationFactor;
    [SerializeField] private float _processPerBiomassFactor;

    [Header("Input")]
    [SerializeField] private float _nitriteConsumptionPPM;

    [Header("Output")]
    [SerializeField] private float _nitrateProducedPPM;

    public override void DoProcess(AquariumParameterData parameters)
    {
        float requiredNitritePPM = _biomass * _nitriteConsumptionPPM * _processPerBiomassFactor;
        float expectedNitrateProducedPPM = _biomass * _nitrateProducedPPM * _processPerBiomassFactor;

        float actualNitriteConsumptionPPM;
        float actualNitrateProduced;

        float availableNitritePPM = GetParameter(Parameter.Nitrite, parameters);
        float availableNitratePPM = GetParameter(Parameter.Nitrate, parameters);

        bool hasLimitingFactor = false;
        float processEfficiency = 1;

        // Determine limiting factor for conversion

        // Not enough Nitrite
        if (requiredNitritePPM > availableNitritePPM)
        {
            hasLimitingFactor = true;
            float calculatedEfficiency = availableNitritePPM / requiredNitritePPM;
            if (calculatedEfficiency < processEfficiency)
            {
                processEfficiency = calculatedEfficiency;
            }
        }

        // Calculate the Growth of the bacteria               
        if (hasLimitingFactor) // if there is a limiting factor then kill the bacteria that is in excess;
        {
            // Only calculate bacteria die off if the value is over 0.1f
            if (_biomass > _minimumBiomass)
            {
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
        actualNitriteConsumptionPPM = requiredNitritePPM * processEfficiency;
        actualNitrateProduced = expectedNitrateProducedPPM * processEfficiency;

        parameters.SubtractFromParameter(Parameter.Nitrite, actualNitriteConsumptionPPM);
        parameters.AddToParameter(Parameter.Nitrate, actualNitrateProduced);

        //parameters[Parameter.Nitrite] = availableNitritePPM - actualNitriteConsumptionPPM;
        //parameters[Parameter.Nitrate] = availableNitratePPM + actualNitrateProduced;
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