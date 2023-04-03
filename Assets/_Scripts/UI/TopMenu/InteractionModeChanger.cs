using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModeChanger : MonoBehaviour
{
    [SerializeField] private InteractionMode interaction;
    // Start is called before the first frame update

    public void ChangeMode()
    {
        Debug.Log("Changing Mode");
        GameState.Instance.CurrentInteractionMode = interaction;
        Debug.Log(GameState.Instance.CurrentInteractionMode);
    }
    
}