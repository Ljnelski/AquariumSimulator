using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AquariumObject : MonoBehaviour
{
    public abstract void DoProcess(Dictionary<Parameter, float> parameters);

    public bool TryToGetParameter(Dictionary<Parameter, float> parameters, Parameter targetParameter , out float value)
    {
        if (!parameters.TryGetValue(targetParameter, out value))
        {
            Debug.LogError(GetType().ToString() + " ERROR: Failed to get parameter '" + targetParameter + "' from Aquarium");
            return false;
        }
        return true;
    }
}