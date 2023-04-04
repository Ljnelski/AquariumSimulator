using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AquariumObjectData", menuName = "ScriptableObjects/AquariumObjectData", order = 1)]
public class AquariumObjectData : ScriptableObject
{
    public string Name;
    public string Description;
    public float Cost;
    public Image Icon;

    [Header("Item Prefab")]
    public GameObject AquariumObjectPrefab;

    private void OnValidate()
    {

        if(AquariumObjectPrefab == null)
        {
            Debug.LogError("Aquarium Object Data ERROR: AquariumObjectData with Item Name '" +
                Name +
                "' no Prefab assigned");
            return;
        }

        IAquariumObject aquariumObjectScript = AquariumObjectPrefab.GetComponent<IAquariumObject>();

        if(aquariumObjectScript == null)
        {
            Debug.LogError("Aquarium Object Data ERROR: AquariumObjectData with Item Name '" +
                Name +
                "' has no Script that implements IAquariumObject on the assigned prefab");
        }
    }
}
