using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class AnimationManager : MonoBehaviour
{
    public List<Animator> animators = new();
    public List<GameObject> controllers = new();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(0.5f);  
        foreach (var animator in animators)
        {
            animator.SetBool("started", true);
        }

        GetComponent<AudioSource>().Play();

        /*yield return new WaitForSeconds(28.143f);
        yield return PlayHapticFeedback(12, 12);
        yield return new WaitForSeconds(9);
        yield return PlayHapticFeedback(11, 11);*/
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
