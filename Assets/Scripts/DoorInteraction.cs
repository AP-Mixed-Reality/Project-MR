using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorInteraction : MonoBehaviour, ICustomInteractable
{
    [SerializeField] private string sceneToLoad;

    public void OnInteract()
    {
        Debug.Log($"{gameObject.name} interacted with!");

        // Load a scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log($"Loading scene: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not set!");
        }

    }
}