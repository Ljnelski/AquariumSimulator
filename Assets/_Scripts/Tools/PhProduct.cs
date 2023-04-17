using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhProduct : ManagementTool
{
    [Header("Ph Product")]
    [SerializeField] private Transform _activePosition;
    [SerializeField] private ParticleSystem _powderEffect;
    [SerializeField] private float _phChange;

    public override void Use()
    {
        base.Use();

        toolCompletedTimer.SetCompleteCallback(Finish);
        toolCompletedTimer.Start(_powderEffect.main.duration);

        float currentPh = _aquariumParameterData.AccessParameterValue(Parameter.Ph);

        // Add food to fish tank Directly
        _aquariumParameterData.AddToParameter(Parameter.Ph, _phChange, 0, 14);

        // Pay the bill for the food
        GameState.Instance.Purchase(_costPerUse);

        // Move
        transform.position = _activePosition.position;
        transform.rotation = _activePosition.rotation;

        // start particles
        _powderEffect.Play();
    }

    protected override void Finish()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, -45, 0f);
        base.Finish();
    }
}
