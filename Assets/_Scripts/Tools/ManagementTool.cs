/*
 * Abstract Class for Tool managment
 * 
 */
using UnityEngine;

public abstract class ManagementTool : MonoBehaviour
{
    [SerializeField] private HighlightMesh _highlighter;

    // Logic for what the tool does
    public abstract void UseTool();

    // Logic for when the tool is 'selected' right now that means being pointed by the mouse
    public virtual void SelectTool()
    {
        _highlighter.ApplyPositiveHighlight();
    }

    // Logic for when the tool is 'deselected' right now that means stop being pointed at by the mouse
    public virtual void DeselectTool()
    {
        _highlighter.RemoveHighlight();
    }
}

