using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Use the new Input System namespace

public class DoorInteraction : MonoBehaviour, ICustomInteractable
{
    [SerializeField] private string sceneToLoad;

    // Input Action for Grab (configured in Unity's Input System)
    [SerializeField] private InputAction grabAction;

    private void OnEnable()
    {
        grabAction.Enable(); // Enable the action when the object is enabled
    }

    private void OnDisable()
    {
        grabAction.Disable(); // Disable the action when the object is disabled
    }

    public void OnInteract()
    {
        Debug.Log($"{gameObject.name} interacted with!");

        // Validate and load the scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            if (SceneExists(sceneToLoad))
            {
                Debug.Log("Scene exists");

                if (grabAction.WasPressedThisFrame()) // Replacing XRInputManager logic
                {
                    Debug.Log($"Grab button pressed, loading scene: {sceneToLoad}");
                    StartCoroutine(LoadSceneAsync(sceneToLoad));
                }
            }
            else
            {
                Debug.LogError($"Scene '{sceneToLoad}' does not exist in build settings!");
            }
        }
        else
        {
            Debug.LogWarning("Scene to load is not set!");
        }
    }

    // Check if the scene name exists in build settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameFromPath == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    // Avoid freezing the game during scene loading
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
