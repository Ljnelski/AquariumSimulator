using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aquarium : MonoBehaviour
{
    [Header("Debug Controls")]
    [SerializeField] private float _oxygenExchangePPM;
    [SerializeField] private float _oxygenMaxDiffusePPM;
    [SerializeField] private float _nitrateDrainPPM;
    [SerializeField] private float _nitriteDrainPPM;
    [SerializeField] private float _ammoniaAdditionPPM;

    [SerializeField] private bool _tickTank;
    [SerializeField] private float _tickTime;

    [SerializeField]

    private Dictionary<Parameter, float> _parameters = new Dictionary<Parameter, float>();
    private List<IAquariumObject> _aquariumProcesses;

    // events
    public Action OnParameterUpdate;

    private void Awake()
    {
        _aquariumProcesses = GetComponentsInChildren<IAquariumObject>().ToList();
        Debug.Log(_aquariumProcesses.Count);
    }
    // Start is called before the first frame update
    void Start()
    {
        _parameters.Add(Parameter.Oxygen, 0f);
        _parameters.Add(Parameter.Ammonia, 0f);
        _parameters.Add(Parameter.Nitrite, 0f);
        _parameters.Add(Parameter.Nitrate, 0f);
        _parameters.Add(Parameter.Ph, 7f);

        StartCoroutine(Tick());
    }

    public void AddAquariumProcess(IAquariumObject newAquariumObject)
    {
        _aquariumProcesses.Add(newAquariumObject);
    }

    public float AccessParameterValue(Parameter targetParameter)
    {
        float targetValue;
        if (!_parameters.TryGetValue(targetParameter, out targetValue))
        {
            Debug.LogError("Aquarium ERROR: failed to get " + targetParameter + " value from aquarium");
        }

        return targetValue;
    }

    public void AddAquariumObject(GameObject AquariumObjectGameObject)
    {
        IAquariumObject newAquariumObject = AquariumObjectGameObject.GetComponent<IAquariumObject>();
        AddAquariumProcess(newAquariumObject);
    }

    IEnumerator Tick()
    {
        while (_tickTank)
        {
            _parameters[Parameter.Ammonia] += _ammoniaAdditionPPM;

            foreach (IAquariumObject process in _aquariumProcesses)
            {
                process.DoProcess(_parameters);
            }

            OnParameterUpdate?.Invoke();
            yield return new WaitForSeconds(_tickTime);
        }
    }
}
