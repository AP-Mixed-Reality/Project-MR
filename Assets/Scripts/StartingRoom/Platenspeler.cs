using UnityEngine;

public class PlatenspelerScript : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Geen AudioClip toegewezen aan de AudioSource.");
        }
    }
}
