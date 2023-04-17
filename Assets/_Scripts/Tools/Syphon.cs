using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syphon : ManagementTool
{
    [Header("Syphone")]
    [SerializeField] private Transform _activePosition;
    [SerializeField] private ParticleSystem _bubbleEffect;
    [SerializeField] private float _waterPh;
    [SerializeField] private float _waterChangeTime;

    public override void Use()
    {
        base.Use();

        toolCompletedTimer.Start(_waterChangeTime, Finish, null);

        // remove a percentage the parameters of the water

        // Pay the bill for the food
        GameState.Instance.Purchase(_costPerUse);

        // Move
        transform.position = _activePosition.position;
        transform.rotation = _activePosition.rotation;

        // start particles
        _bubbleEffect.Play();
    }

    protected override void Finish()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 13);
        _bubbleEffect.Stop();
        base.Finish();
    }
}
