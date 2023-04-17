using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionModeChanger : MonoBehaviour
{
    public void ChangeMode()
    {
        switch (GameState.Instance.CurrentInteractionMode)
        {
            case InteractionMode.Manage:
                GameState.Instance.CurrentInteractionMode = InteractionMode.Edit;
                break;
            case InteractionMode.Edit:
                GameState.Instance.CurrentInteractionMode = InteractionMode.Manage;
                break;
            default:
                break;
        }
    }
    
}
