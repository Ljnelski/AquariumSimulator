using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StoreItemData", menuName = "ScriptableObjects/StoreItemData", order = 1)]
public class StoreItemData : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public float ItemCost;
    public Image ItemIcon;
}
