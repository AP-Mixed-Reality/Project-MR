using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRayInteractor : MonoBehaviour
{
    [SerializeField] public float rayLength = 10f; // Length of the ray
    [SerializeField] public LayerMask interactionLayer; // Layers to interact with
    [SerializeField] public Transform rayOrigin; // Optional: Set to the controller or object emitting the ray

    private void Update()
    {
        // Default ray origin to this object's position if none is set
        Vector3 origin = rayOrigin ? rayOrigin.position : transform.position;
        Vector3 direction = rayOrigin ? rayOrigin.forward : transform.forward;

        // Visualize the ray
        Debug.DrawRay(origin, direction * rayLength, Color.green);

        // Perform raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength, interactionLayer))
        {
            // Handle interaction
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);

            // Check for custom interaction script
            var interactable = hit.collider.GetComponent<ICustomInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }
}
