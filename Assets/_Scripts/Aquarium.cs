using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquarium : MonoBehaviour
{
    [Header("Parameters")]

    [Range(0, 14)][SerializeField] private int _pH;

    [Min(0)][SerializeField] private float _ammoniaPPM;
    [Min(0)][SerializeField] private float _nitratePPM;
    [Min(0)][SerializeField] private float _nitritePPM;
    [Min(0)][SerializeField] private float _oxygenPPM;

    [Header("Bacteria")]
    [SerializeField] private float _bacteriaBiomass;
    [SerializeField] private float _bacteriaConversionRatePPM;
    [SerializeField] private float _bacteriaOxygenUseRatePPM;
    [SerializeField] private float _bacteriaGrowthRate;
    [SerializeField] private float _bacteriaStarvationFactor;
    [SerializeField] private float _bacteriaConsumptionRatePPM;

    [Header("DebugControls")]
    [SerializeField] private float _oxygenExchangePPM;
    [SerializeField] private float _oxygenMaxDiffusePPM;
    [SerializeField] private float _nitrateDrainPPM;
    [SerializeField] private float _nitriteDrainPPM;
    [SerializeField] private float _ammoniaAdditionPPM;

    [SerializeField] private bool _tickTank;
    [SerializeField] private float _tickTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tick());
    }

    private void CalculateAmmoniaConsumingBacteria()
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

    IEnumerator Tick()
    {
        while (_tickTank)
        {
            _oxygenPPM = Mathf.Min(_oxygenPPM + _oxygenExchangePPM, _oxygenMaxDiffusePPM);
            _ammoniaPPM = _ammoniaPPM + _ammoniaAdditionPPM;

            CalculateAmmoniaConsumingBacteria();

            _nitratePPM = _nitratePPM - _nitrateDrainPPM;

            yield return new WaitForSeconds(_tickTime);
        }
    }
}
