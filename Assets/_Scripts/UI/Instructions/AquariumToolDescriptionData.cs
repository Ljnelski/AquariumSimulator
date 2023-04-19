using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AquariumParameterData", menuName = "ScriptableObjects/AquariumToolDescriptionData", order = 2)]

public class AquariumToolDescriptionData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
}
