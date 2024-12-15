using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class CustomRayInteractor : MonoBehaviour
{
    [SerializeField] public float rayLength = 10f; // Length of the ray
    [SerializeField] public LayerMask interactionLayer; // Layers to interact with
    [SerializeField] public Transform rayOrigin; // Optional: Set to the controller or object emitting the ray

    [SerializeField] private Material highlightMaterial; // Material to apply on hit
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    public List<GameObject> controllers = new();

    private void Update()
    {
        // Default ray origin to this object's position if none is set
        Vector3 origin = rayOrigin ? rayOrigin.position : transform.position;
        Vector3 direction = rayOrigin ? rayOrigin.forward : transform.forward;

        //todo delete this: Visualize the ray
        Debug.DrawRay(origin, direction * rayLength, Color.green);

        // Perform raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength, interactionLayer))
        {
            // Handle interaction
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);

            // Change the Material of the target GameObject
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!originalMaterials.ContainsKey(renderer))
                {
                    originalMaterials[renderer] = renderer.material; // Store the original material
                }
                renderer.material = highlightMaterial; // Apply the highlight material
                StartCoroutine(PlayHapticFeedback(12, 12.0f));
            }

            // Check for custom interaction script
            //todo change to XR Input Manager's fire/grab/every other button
            var interactable = hit.collider.GetComponent<ICustomInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
        else
        {
            // Restore materials when the ray is not hitting anything
            RestoreMaterials();
        }
    }

    // Method to restore original materials
    private void RestoreMaterials()
    {
        foreach (var entry in originalMaterials)
        {
            entry.Key.material = entry.Value;
        }
        originalMaterials.Clear();
    }
    IEnumerator PlayHapticFeedback(int pulses, float duration)
    {
        // Decide how much time is between each pulse
        float timeBetweenPulse = duration / pulses;
        for (int i = 0; i < pulses; i++)
        {
            foreach (var controller in controllers)
            {
                HapticImpulsePlayer haptic = controller.GetComponent<HapticImpulsePlayer>();
                bool success = haptic.SendHapticImpulse(1, 0.1f);
                Debug.Log("sending pulse, success = " + success);
            }

            yield return new WaitForSeconds(timeBetweenPulse);
        }
    }
}