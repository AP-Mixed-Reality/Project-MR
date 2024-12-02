using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public List<AudioClip> AudioClips = new();
    public float playChance = 0.1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10000) < playChance * 100)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (!audioSource.isPlaying) 
            {
                audioSource.clip = AudioClips[Random.Range(0, AudioClips.Count)];
                audioSource.Play();    
            }
        }
    }
}
