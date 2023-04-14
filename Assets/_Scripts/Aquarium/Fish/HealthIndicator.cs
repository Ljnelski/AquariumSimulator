using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private Gradient _indicator;
    [SerializeField] private MeshRenderer _fishMesh;

    public void AdjustGradient(float value)
    {
        _fishMesh.material.color = _indicator.Evaluate(1 - value/100);
    }
}
