using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    public float speed = 1;
    public float turnSpeed = 10;
    public float maxSpeed = 2;
    public bool hasBeenFast = false;
    public float lastTurnAt = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // We want to just move the animal to some direction, if it hasn't moved from its distance for 3 seconds
        // then we should turn around and go somewhere else, this is a failsafe in case it gets stuck
        // We will just apply a small force forwards and to the left or right
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.velocity.magnitude < 0.1f && hasBeenFast && Time.deltaTime - lastTurnAt > 3) 
        {
            // We are stuck, turn around by modifying our rotation by 180 degrees
            transform.Rotate(0, Random.Range(90, 270), 0);
            hasBeenFast = false;
            lastTurnAt = Time.deltaTime;
        }
        else
        {
            hasBeenFast = true;
        }
        
        rb.AddForce(transform.forward * speed);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
        // Random chance to move
        if (Random.Range(0, 100) < 5)
        {
            transform.Rotate(0, Random.Range(-turnSpeed, turnSpeed), 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Terrain"))
        {
            transform.Rotate(0, Random.Range(90, 270), 0);
        }
    }
}
