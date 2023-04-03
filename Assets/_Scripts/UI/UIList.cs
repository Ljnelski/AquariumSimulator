using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIList : MonoBehaviour
{
    [SerializeField] private Aquarium _aquarium;
    [SerializeField] private GameObject ListPrefab;
    [SerializeField] private List<StoreItemData> storeItemData = new List<StoreItemData>();

    private void Start()
    {
        StoreItem storeItem;
        foreach (StoreItemData itemData in storeItemData)
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
