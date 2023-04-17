using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syphon : ManagementTool
{
    [Header("Syphon")]
    [SerializeField] private Transform _activePosition;
    [SerializeField] private ParticleSystem _bubbleEffect;
    [Range(0, 1)]
    [SerializeField] private float _waterRemoved; // range of amount of water drained from tank. 0 = no water, 1 = all water

    [Header("Properties of incoming water")]
    [SerializeField] private float _waterPh;
    [SerializeField] private float _waterChangeTime;
    [SerializeField] private float _waterTempurature;

    public override void Use()
    {
        base.Use();

        toolCompletedTimer.SetCompleteCallback(Finish);
        toolCompletedTimer.Start(_waterChangeTime);

        // remove a percentage the parameters of the water
        
        // pay the price of using the equipment
        GameState.Instance.Purchase(_costPerUse);

        // move
        transform.position = _activePosition.position;
        transform.rotation = _activePosition.rotation;

        // start particles
        _bubbleEffect.Play();
    }

    private void ChangeWater()
    {
        float avaliableAmmonia = _aquariumParameterData.AccessParameterValue(Parameter.Ammonia);
        float avaliableNitrite = _aquariumParameterData.AccessParameterValue(Parameter.Nitrite);
        float avaliableNitrate = _aquariumParameterData.AccessParameterValue(Parameter.Nitrate);
        float aquariumPh = _aquariumParameterData.AccessParameterValue(Parameter.Ph);
        float aquariumTemperature = _aquariumParameterData.AccessParameterValue(Parameter.Temperature);

        float removedAmmonia = avaliableAmmonia * _waterRemoved;
        float removedNitrite = avaliableNitrite * _waterRemoved;
        float removedNitrate = avaliableNitrate * _waterRemoved;

        // The Ph and Temperature calculated by weighing current values and income values of the water using _water Removed

        // Calculate the change in Ph
        float newPh = aquariumPh *  (1 - _waterRemoved) + _waterPh * _waterRemoved;
        float phChange = newPh - aquariumPh;

        // Calculate the change in Tempurature
        float newTemperature = aquariumTemperature * (1 - _waterRemoved) + _waterTempurature * _waterRemoved;
        float temperatureChange = newTemperature - aquariumTemperature;

        _aquariumParameterData.SubtractFromParameter(Parameter.Ammonia, removedAmmonia);
        _aquariumParameterData.SubtractFromParameter(Parameter.Nitrite, removedNitrite);
        _aquariumParameterData.SubtractFromParameter(Parameter.Nitrate, removedNitrate);

        _aquariumParameterData.AddToParameter(Parameter.Temperature, temperatureChange);
        _aquariumParameterData.AddToParameter(Parameter.Ph, phChange, 0f, 14f);
    }

    protected override void Finish()
    {
        ChangeWater();
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 13);
        _bubbleEffect.Stop();
        base.Finish();
    }
}
