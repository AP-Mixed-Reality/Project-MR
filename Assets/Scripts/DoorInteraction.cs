using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorInteraction : MonoBehaviour, ICustomInteractable
{
    public void OnInteract()
    {
        Debug.Log($"{gameObject.name} interacted with!");
        // Add additional interaction logic here
    }
}