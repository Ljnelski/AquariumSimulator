/*
 * Abstract Class for Tool managment
 * 
 */
using System;
using UnityEngine;

public abstract class ManagementTool : MonoBehaviour
{
    [Header("Management Tool")]
    [SerializeField] protected AquariumParameterData _aquariumParameterData;
    [SerializeField] private HighlightMesh _highlighter;
    [SerializeField] protected float _costPerUse;

    public Action OnToolFinished;
    public bool InUse => _inUse;

    protected ActionTimer toolCompletedTimer = new ActionTimer();
    protected bool _inUse;

    // Logic for what the tool does
    public virtual void Use()
    {
        _inUse = true;
    }

    // Logic for when the tool is 'selected' right now that means being pointed by the mouse
    public virtual void Select()
    {
        _highlighter.ApplyPositiveHighlight();
    }

    protected void Update()
    {
        toolCompletedTimer.Tick(Time.deltaTime);
    }

    // Logic for when the tool is 'deselected' right now that means stop being pointed at by the mouse
    public virtual void Deselect()
    {
        _highlighter.RemoveHighlight();
    }    

    protected virtual void Finish()
    {
        _inUse = false;
        OnToolFinished?.Invoke();
    }
}

