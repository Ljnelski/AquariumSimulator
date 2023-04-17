using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFood : ManagementTool
{
    [Header("Fish Food")]
    [SerializeField] private Transform _activePosition;
    [SerializeField] private ParticleSystem _foodPelletEffect;
    [SerializeField] private float _foodPerUse;

    public override void Use()
    {
        base.Use();

        toolCompletedTimer.Start(_foodPelletEffect.main.duration, Finish, null);

        // Add food to fish tank Directly
        _aquariumParameterData.IncreaseParameter(Parameter.FishFood, _foodPerUse);

        // Pay the bill for the food
        GameState.Instance.Purchase(_costPerUse);

        // Move
        transform.position = _activePosition.position;
        transform.rotation = _activePosition.rotation;

        // start particles
        _foodPelletEffect.Play();
    }

    protected override void Finish()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, -45, 0f);
        base.Finish();
    }
}
