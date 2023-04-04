using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIList : MonoBehaviour
{
    [SerializeField] private Aquarium _aquarium;
    [SerializeField] private GameObject ListPrefab;
    [SerializeField] private List<AquariumObjectData> storeItemData = new List<AquariumObjectData>();

    private void Start()
    {
        StoreItem storeItem;
        foreach (AquariumObjectData itemData in storeItemData)
        {
            storeItem = CreateListItem().GetComponent<StoreItem>();
            storeItem._aquarium = _aquarium;
            storeItem.LoadData(itemData);                  
        }
    }

    public GameObject CreateListItem()
    {
        return Instantiate(ListPrefab, transform);
    }
}
