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
        
    }

    IEnumerator Tick()
    {
        while (_tickTank)
        {
            _oxygenPPM = Mathf.Min(_oxygenPPM + _oxygenExchangePPM, _oxygenMaxDiffusePPM);
            _ammoniaPPM = _ammoniaPPM + _ammoniaAdditionPPM;

            _nitratePPM = _nitratePPM - _nitrateDrainPPM;

            yield return new WaitForSeconds(_tickTime);
        }
    }
}
