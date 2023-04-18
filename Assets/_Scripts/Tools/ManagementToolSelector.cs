using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagementToolSelector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private DefaultInputActions input;
    private Camera _raycastSourceCam;
    private ManagementTool selectedTool = null;

    private bool _locked;

    void Awake()
    {
        input = new DefaultInputActions();
        _raycastSourceCam = Camera.main;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Look.performed += OnClick;
        input.UI.Click.performed += UseTool;
    }
    private void OnClick(InputAction.CallbackContext context)
    {
        if (!_locked)
        {
            RayCast();
        }
    }
    private void RayCast()
    {
        Ray ray = _raycastSourceCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _layerMask))
        {
            ManagementTool hitTool = FindManagementTool(hit.transform.gameObject);

            // If the cursor moved off the targetedObject, then remove the highlight
            if (selectedTool != null && selectedTool != hitTool)
            {
                selectedTool.Deselect();
                selectedTool = null;
            }

            // If there isn't new object then return
            if (hitTool == null) { return; }

            // If the object is inUse then don't select it
            if (hitTool.InUse) { return; }

            selectedTool = hitTool;
            selectedTool.Select();
        }
    }
    private void UseTool(InputAction.CallbackContext context)
    {
        if (selectedTool == null) { return; }
        if (selectedTool.InUse) { return; }

        selectedTool.Use();
        selectedTool.Deselect();
        selectedTool.OnToolFinished += Unlock;

        _locked = true;

        // Update what the mouse is pointing at in the case the object moves
        RayCast();

    }

    // Locks the selector so another equipment cannot be selected
    private void Unlock()
    {       
        selectedTool.OnToolFinished -= Unlock;
        selectedTool = null;
        _locked = false;
    }

    private ManagementTool FindManagementTool(GameObject gameObject)
    {
        ManagementTool managementTool;

        // Check if the script is on the Gameobject passed in
        if (gameObject.TryGetComponent<ManagementTool>(out managementTool))
        {
            return managementTool;
        }

        // Check if the script is contained in the parent of the gameobject
        if (gameObject.transform.parent != null && gameObject.transform.parent.TryGetComponent<ManagementTool>(out managementTool))
        {
            return managementTool;
        }

        // If no object is found, return null
        return null;
    }
    private void OnDisable()
    {
        input.Disable();
    }
}
