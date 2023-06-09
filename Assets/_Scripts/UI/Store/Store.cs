using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Move Store to a Scriptable Object
public class Store : MonoBehaviour
{
    [SerializeField] private GameObject _storeUI;
    // Start is called before the first frame update    

    private void OnEnable()
    {
        if (GameState.Instance != null) {
            GameState.Instance.OnInteractionModeChange += SetStoreVisable;
        }
    }
    void Start()
    {
        GameState.Instance.OnInteractionModeChange += SetStoreVisable;
    }

    private void SetStoreVisable(InteractionMode mode)
    {
        switch (mode)
        {
            case InteractionMode.Edit:
                _storeUI.SetActive(true);
                break;
            case InteractionMode.Manage:
                _storeUI.SetActive(false);
                break;
        }
    }

    private void OnDisable()
    {
        GameState.Instance.OnInteractionModeChange -= SetStoreVisable;
    }
}
