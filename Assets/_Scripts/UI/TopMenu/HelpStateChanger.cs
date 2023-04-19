using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HelpStateChanger : MonoBehaviour
{
    public void ChangeState()
    {
        GameState.Instance.HelpOpen = !GameState.Instance.HelpOpen;
    }
}
