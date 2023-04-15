using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagementToolSelector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private DefaultInputActions input;
    private Camera _raycastSourceCam;
    private ManagementTool targetedManagementTool = null;

    void Awake()
    {
        input = new DefaultInputActions();
        _raycastSourceCam = Camera.main;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Look.performed += RayCast;
        input.UI.Click.performed += UseTool;
    }
    private void RayCast(InputAction.CallbackContext context)
    {
        Debug.Log("RayCasting");

        Ray ray = _raycastSourceCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _layerMask))
        {
            ManagementTool newManagementTool = FindManagementTool(hit.transform.gameObject);

            // If the cursor moved off the targetedObject, then remove the highlight
            if (targetedManagementTool != null && targetedManagementTool != newManagementTool)
            {
                targetedManagementTool.DeselectTool();
                targetedManagementTool = null;
            }

            // If there isn't new object then return
            if (newManagementTool == null) { return; }

            targetedManagementTool = newManagementTool;
            targetedManagementTool.SelectTool();
        }
    }
    private void UseTool(InputAction.CallbackContext context)
    {
        if (targetedManagementTool == null) { return; }

        targetedManagementTool.UseTool();
    }
    private ManagementTool FindManagementTool(GameObject gameObject)
    {
        ManagementTool aquariumObject;

        // Check if the script is on the Gameobject passed in
        if (gameObject.TryGetComponent<ManagementTool>(out aquariumObject))
        {
            return aquariumObject;
        }

        // Check if the script is contained in the parent of the gameobject
        if (gameObject.transform.parent.TryGetComponent<ManagementTool>(out aquariumObject))
        {
            return aquariumObject;
        }

        // If no object is found, return null
        return null;
    }
    private void OnDisable()
    {
        input.Disable();
    }
}
