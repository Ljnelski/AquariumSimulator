using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aquarium : MonoBehaviour
{
    [Header("Aquarium Parameter Data")]
    [SerializeField] private AquariumParameterData _parameters;

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

   
    private List<AquariumObject> _aquariumObjects = new List<AquariumObject>();  

    private void Awake()
    {
        // Initalize Parameters that don't start at zero
        _parameters.AddToParameter(Parameter.Ph, 7.0f);

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
            _parameters.AddToParameter(Parameter.SupportedBiomass, bioMedia.SupportedBiomass);
        }

        _aquariumObjects.Add(newAquariumObject);
    }

    public void AddAquariumObject(GameObject AquariumObjectGameObject)
    {
        AquariumObject newAquariumObject = AquariumObjectGameObject.GetComponent<AquariumObject>();
        AddAquariumObject(newAquariumObject);
    }   
    
    public void RemoveAquariumObject(AquariumObject aquariumObject)
    {
        _aquariumObjects.Remove(aquariumObject);
    }

    IEnumerator Tick()
    {
        while (_tickTank)
        {
            _parameters.AddToParameter(Parameter.Ammonia, _ammoniaAdditionPPM);
            _parameters.SubtractFromParameter(Parameter.Temperature, _heatDissapation, _roomTemperature);
            _parameters.AddToParameter(Parameter.Oxygen, _oxygenMaxDiffusePPM, _oxygenExchangePPM);

            foreach (AquariumObject process in _aquariumObjects)
            {
                process.DoProcess(_parameters);
            }

            _parameters.OnParameterUpdate?.Invoke();
            yield return new WaitForSeconds(_tickTime);
        }
    }
}
