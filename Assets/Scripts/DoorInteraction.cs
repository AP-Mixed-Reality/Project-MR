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

        // Validate and load the scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            if (SceneExists(sceneToLoad))
            {
                Debug.Log($"Loading scene: {sceneToLoad}");
                // load the scene asynchronously for performance (does not help with Quest Link leaving game when scene changed)
                StartCoroutine(LoadSceneAsync(sceneToLoad));
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
    
    //check if name exists in build settings
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

    //avoid freezing the game during scene loading
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}