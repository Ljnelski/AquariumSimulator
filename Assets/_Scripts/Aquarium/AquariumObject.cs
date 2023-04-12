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
    [Header("Aquarium Object")]
    [SerializeField] private HighlightMesh _meshHighlighter;

    private StoreItem _storeItem;

    protected bool TryToGetParameter(Dictionary<Parameter, float> parameters, Parameter targetParameter, out float value)
    {
        if (!parameters.TryGetValue(targetParameter, out value))
        {
            Debug.LogError(GetType().ToString() + " ERROR: Failed to get parameter '" + targetParameter + "' from Aquarium");
            return false;
        }
        return true;
    }

    public abstract void DoProcess(Dictionary<Parameter, float> parameters);   

    public virtual void HightlightValid()
    {
        _meshHighlighter.ApplyPositiveHighlight();
    }

    public virtual void HightLightInvalid()
    {
        _meshHighlighter.ApplyNegativeHighlight();
    }

    public virtual void RemoveHighlight()
    {
        _meshHighlighter.RemoveHighlight();
    }

    public void Remove()
    {
        _storeItem.gameObject.SetActive(true);
        Destroy(gameObject);
    }
    public void InjectStoreItem(StoreItem storeItem)
    {
        _storeItem = storeItem;
    }

}