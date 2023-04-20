using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiGroup;

    [SerializeField] private GameObject _aquariumToolTab;
    [SerializeField] private GameObject _bettaCareTab;
    [SerializeField] private GameObject _nitrogenCycleTab;


    private void OnEnable()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.OnHelpOpen += SetActive;
        }
    }
    private void Start()
    {
        GameState.Instance.OnHelpOpen += SetActive;
    }

    private void SetActive(bool active)
    {
        _uiGroup.SetActive(active);
    }

    public void ShowAquariumToolTab()
    {
        Debug.Log("CALLED");
        _aquariumToolTab.SetActive(true);
        _bettaCareTab.SetActive(false);
        _nitrogenCycleTab.SetActive(false);
    }
    public void ShowBettaCareTab()
    {
        _aquariumToolTab.SetActive(false);
        _bettaCareTab.SetActive(true);
        _nitrogenCycleTab.SetActive(false);
    }
    public void ShowNitrogenCycleTab()
    {
        _aquariumToolTab.SetActive(false);
        _bettaCareTab.SetActive(false);
        _nitrogenCycleTab.SetActive(true);
    }

    private void OnDisable()
    {
        GameState.Instance.OnHelpOpen -= SetActive;
    }
}
