using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aquarium : MonoBehaviour
{
    [Header("Debug Controls")]
    [SerializeField] private float _nitrateDrainPPM;
    [SerializeField] private float _nitriteDrainPPM;
    [SerializeField] private float _ammoniaAdditionPPM;

    [Header("Gameplay")]
    [SerializeField] private bool _tickTank;
    [SerializeField] private float _tickTime;

    [Header("Aquarium")]
    [SerializeField] private float _oxygenExchangePPM;
    [SerializeField] private float _oxygenMaxDiffusePPM;
    [SerializeField] private float _supportedBiomass;
    [SerializeField] private float _heatDissapation;
    [SerializeField] private float _roomTemperature = 19f;

    private Dictionary<Parameter, float> _parameters = new Dictionary<Parameter, float>();
    private List<AquariumObject> _aquariumObjects = new List<AquariumObject>();

    // events
    public Action OnParameterUpdate;

    private void Awake()
    {
        // Initalize Parameters
        _parameters.Add(Parameter.Oxygen, 0f);
        _parameters.Add(Parameter.Ammonia, 0f);
        _parameters.Add(Parameter.Nitrite, 0f);
        _parameters.Add(Parameter.Nitrate, 0f);
        _parameters.Add(Parameter.Ph, 7f);
        _parameters.Add(Parameter.SupportedBiomass, 0f);
        _parameters.Add(Parameter.Temperature, 18f);
        _parameters.Add(Parameter.FishFood, 2f);

        AquariumObject[] aquariumObjects = GetComponentsInChildren<AquariumObject>();

        for (int i = 0; i < aquariumObjects.Length; i++)
        {
            AddAquariumObject(aquariumObjects[i]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tick());
    }

    private void Update()
    {
        //Debug.Log("SupportedBiomass: " + _parameters[Parameter.SupportedBiomass]);
    }

    public void AddAquariumObject(AquariumObject newAquariumObject)
    {
        // Update Surface Area
        IBioMedia bioMedia;
        if(newAquariumObject.TryGetComponent<IBioMedia>(out bioMedia))
        {
            _parameters[Parameter.SupportedBiomass] += bioMedia.SupportedBiomass;
        }

        _aquariumObjects.Add(newAquariumObject);
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
        AquariumObject newAquariumObject = AquariumObjectGameObject.GetComponent<AquariumObject>();
        AddAquariumObject(newAquariumObject);
    }    

    IEnumerator Tick()
    {
        while (_tickTank)
        {
            _parameters[Parameter.Ammonia] += _ammoniaAdditionPPM;
            _parameters[Parameter.Temperature] = Mathf.Max(_parameters[Parameter.Temperature] - _heatDissapation, _roomTemperature);
            _parameters[Parameter.Oxygen] = Mathf.Min(_parameters[Parameter.Oxygen] + _oxygenExchangePPM, _oxygenMaxDiffusePPM);

            foreach (AquariumObject process in _aquariumObjects)
            {
                process.DoProcess(_parameters);
            }

            OnParameterUpdate?.Invoke();
            yield return new WaitForSeconds(_tickTime);
        }
    }
}
