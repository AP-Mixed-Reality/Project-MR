using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<Animator> animators = new();
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

        yield return new WaitForSeconds(0.2f);
        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
