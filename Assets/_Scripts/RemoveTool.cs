using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemoveTool : MonoBehaviour
{
    [SerializeField] private LayerMask removeable;

    private DefaultInputActions input;
    private Camera cam;
    private AquariumObject targetedAquariumObject = null;

    void Awake()
    {
        input = new DefaultInputActions();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Look.performed += RayCast;
        input.UI.Click.performed += ReturnItem;
    }

    private void RayCast(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, removeable))
        {
            AquariumObject newAquariumObject = FindAquariumObject(hit.transform.gameObject);          

            // If the cursor moved off the targetedObject, then remove the highlight
            if(targetedAquariumObject != null && targetedAquariumObject != newAquariumObject)
            {
                targetedAquariumObject.RemoveHighlight();
                targetedAquariumObject = null;
            }


            // If there isn't new object then return
            if(newAquariumObject == null) { return; }
            

            targetedAquariumObject = newAquariumObject;
            targetedAquariumObject.HightLightInvalid();
        }
    }   

    private void ReturnItem(InputAction.CallbackContext context)
    {
        if(targetedAquariumObject == null) { return; }

        targetedAquariumObject.Remove();
    }


    private AquariumObject FindAquariumObject(GameObject gameObject)
    {
        AquariumObject aquariumObject;

        // Check if the script is on the Gameobject passed in
        if(gameObject.TryGetComponent<AquariumObject>(out aquariumObject))
        {
            return aquariumObject;
        }

        // Check if the script is contained in the parent of the gameobject
        if(gameObject.transform.parent.TryGetComponent<AquariumObject>(out aquariumObject))
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
