using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{

    [SerializeField] private TMP_Text ItemName;
    [SerializeField] private TMP_Text ItemDescription;
    [SerializeField] private TMP_Text ItemCost;

    [SerializeField] private Image ItemIcon;

    public void LoadData(StoreItemData data)
    {
        ItemName.text = data.ItemName;
        ItemDescription.text = data.ItemDescription;
        ItemCost.text =  "$" + data.ItemCost;
    }
}
