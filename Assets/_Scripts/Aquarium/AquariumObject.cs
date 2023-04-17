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

    protected float GetParameter(Parameter targetParameter, AquariumParameterData parameters)
    {
        return parameters.AccessParameterValue(targetParameter);
    }

    public abstract void DoProcess(AquariumParameterData parameters);   

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