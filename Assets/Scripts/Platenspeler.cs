using UnityEngine;

public class PlatenspelerScript : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Haal de AudioSource-component op
        audioSource = GetComponent<AudioSource>();

        // Controleer of er een AudioClip is toegewezen
        if (audioSource.clip != null)
        {
            // Speel de muziek af
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Geen AudioClip toegewezen aan de AudioSource.");
        }
    }
}
