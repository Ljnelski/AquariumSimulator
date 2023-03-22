using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoniaEatingBacteria : MonoBehaviour
{
    [Header("Bacteria")]
    [SerializeField] private float _bacteriaBiomass;
    [SerializeField] private float _bacteriaConversionRatePPM;
    [SerializeField] private float _bacteriaOxygenUseRatePPM;
    [SerializeField] private float _bacteriaGrowthRate;
    [SerializeField] private float _bacteriaStarvationFactor;
    [SerializeField] private float _bacteriaConsumptionRatePPM;

    // THIS IS BROKEN, I COPIED IT OVER FROM AQUARIUM SCRIPT DIRECTLY, NEED TO DO MORE WEEK
    void DoProcess(float _ammoniaPPM, float _oxygenPPM, float _nitratePPM)
    {
        float actualConsumptionPPM;
        float requiredConsumptionPPM = _bacteriaBiomass * _bacteriaConsumptionRatePPM;

        // If enough Ammonia for consumption, grow bacteria 
        if (_ammoniaPPM > requiredConsumptionPPM)
        {
            _bacteriaBiomass = _bacteriaBiomass * _bacteriaGrowthRate;
            actualConsumptionPPM = requiredConsumptionPPM;

        }
        // Bacteria Converts what ammonia is still avalible and dies off
        else
        {
            actualConsumptionPPM = _ammoniaPPM;

            // Only calculate bacteria die off if the value is over 0.1f
            if (_bacteriaBiomass > 0.1f)
            {
                _bacteriaBiomass = _bacteriaBiomass - ((requiredConsumptionPPM - actualConsumptionPPM) / _bacteriaConsumptionRatePPM) * _bacteriaStarvationFactor;
            }
        }
        Debug.Log("Ammonia Consumed: " + actualConsumptionPPM);
        _ammoniaPPM = Mathf.Max(_ammoniaPPM - actualConsumptionPPM, 0f);
        _nitratePPM = _nitratePPM + (actualConsumptionPPM * _bacteriaConversionRatePPM);
        _oxygenPPM = Mathf.Max(_oxygenPPM - actualConsumptionPPM * _bacteriaOxygenUseRatePPM, 0f);
    }
}
