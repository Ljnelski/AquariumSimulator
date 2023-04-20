using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AquariumToolDescription : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _icon;

    [SerializeField] private AquariumToolDescriptionData _data;
    
    private void Awake()
    {
        _name.text = _data.Name;
        _description.text = _data.Description;
        _icon.sprite = _data.Icon;
    }
}
