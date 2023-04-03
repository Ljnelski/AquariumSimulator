using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Aquarium _aquarium;

    [SerializeField] private TMP_Text ItemName;
    [SerializeField] private TMP_Text ItemDescription;
    [SerializeField] private TMP_Text ItemCost;

    [SerializeField] private Material itemMaterial;
    [SerializeField] private Material highlightMaterial;

    [SerializeField] private Image ItemIcon;

    // This is the gameobject in the scene that is paired to the store Item Data scriptable object
    private GameObject pairedItem;

    public void LoadData(StoreItemData data)
    {
        ItemName.text = data.ItemName;
        ItemDescription.text = data.ItemDescription;
        ItemCost.text = "$" + data.ItemCost;
       
        Transform itemInAquarium = _aquarium.FindObject(data.ItemName);
        if (itemInAquarium == null)
        {
            Debug.LogError("StoreItem ERROR: No Aquarium Item Found with name: " + data.ItemName);
        }
        else
        {
            pairedItem = _aquarium.FindObject(data.ItemName).gameObject;
            itemMaterial = pairedItem.GetComponent<Material>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pairedItem.SetActive(true);
        pairedItem.GetComponent<MeshRenderer>().materials[0] = highlightMaterial;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pairedItem.GetComponent<MeshRenderer>().materials[0] = itemMaterial;
        pairedItem.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pairedItem.SetActive(true);
        gameObject.SetActive(false);
    }
}
