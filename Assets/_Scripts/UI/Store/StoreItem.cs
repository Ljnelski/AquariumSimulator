using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Aquarium _aquarium;

    [SerializeField] private TMP_Text _objectName;
    [SerializeField] private TMP_Text _objectDescription;
    [SerializeField] private TMP_Text _objectCost;

    [SerializeField] private Image _objectIcon;

    // This is the gameobject in the scene that is paired to the store Item Data scriptable object
    private GameObject _aquariumObjectPrefab;
    private GameObject _instantiatedAquariumObject;

    private float _cost;

    public void LoadData(AquariumObjectData data)
    {
        _objectName.text = data.Name;
        _objectDescription.text = data.Description;
        _objectCost.text = "$" + data.Cost;


        _cost = data.Cost;
        _aquariumObjectPrefab = data.AquariumObjectPrefab;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _instantiatedAquariumObject = Instantiate(_aquariumObjectPrefab);
        if (GameState.Instance.AvalalibleFunds > _cost)
        {
            _instantiatedAquariumObject.GetComponent<HighlightMesh>().ApplyPositiveHighlight();
        } else
        {
            _instantiatedAquariumObject.GetComponent<HighlightMesh>().ApplyNegativeHighlight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {       
        Destroy(_instantiatedAquariumObject);        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // See if it can be purchased Purchase        
        if(GameState.Instance.AvalalibleFunds < _cost)
        {
            return;
        }

        GameState.Instance.Purchase(_cost);        

        AquariumObject aquariumObject = _instantiatedAquariumObject.GetComponent<AquariumObject>();
        aquariumObject.RemoveHighlight();
        aquariumObject.InjectStoreItem(this);

        _aquarium.AddAquariumObject(_instantiatedAquariumObject);
        gameObject.SetActive(false);
    }
}
