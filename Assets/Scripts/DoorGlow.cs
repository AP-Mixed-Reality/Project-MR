using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGlow : MonoBehaviour
{
    [SerializeField] private Material glowMaterial; // The glow material
    private Material originalMaterial; // To store the original material
    private Renderer doorRenderer;

    private void Awake()
    {
        // Get the Renderer of the door and store the original material
        doorRenderer = GetComponent<Renderer>();
        if (doorRenderer != null)
        {
            originalMaterial = doorRenderer.material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Doorglow triggered by: " + other.name);
        // Check if the right-hand ray is pointing at the door
        if (other.CompareTag("RightHandRay"))
        {
            ApplyGlow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the glow when the ray stops pointing at the door
        if (other.CompareTag("RightHandRay"))
        {
            RemoveGlow();
        }
    }

    private void ApplyGlow()
    {
        if (doorRenderer != null && glowMaterial != null)
        {
            doorRenderer.material = glowMaterial; // Apply the glow material
        }
    }

    private void RemoveGlow()
    {
        if (doorRenderer != null && originalMaterial != null)
        {
            doorRenderer.material = originalMaterial; // Restore the original material
        }
    }
}
