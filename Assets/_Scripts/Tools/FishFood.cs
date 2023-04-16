using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFood : ManagementTool
{

    [SerializeField] private Transform _activePosition;
    [SerializeField] private ParticleSystem _foodPelletEffect;

   
    public override void UseTool()
    {
        Debug.Log("USE FISH FOOD");

        toolCompletedTimer.Start(_foodPelletEffect.main.duration, FinishTool, null);

        // Move,
        transform.position = _activePosition.position;
        transform.rotation = _activePosition.rotation;

        // start particles
        _foodPelletEffect.Play();
    }

    private void Update()
    {
        toolCompletedTimer.Tick(Time.deltaTime);
    }

    protected override void FinishTool()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, -45, 0f);
        Debug.Log("Particles Done Complete");
        base.FinishTool();
    }
}
