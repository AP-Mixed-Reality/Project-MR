using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteHandleAnchor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrab()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void OnRelease()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
