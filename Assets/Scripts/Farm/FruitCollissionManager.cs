using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm
{
    public class FruitCollissionManager : MonoBehaviour
    {
        public bool CanBeEaten = false;
        public bool hasBeenInTrigger = false;
        public Vector3 newTomatoSpawnLocation;
        public GameObject tomatoPrefab;
        
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EdibleArea"))
            {
                CanBeEaten = true;

                if (!hasBeenInTrigger)
                {
                    hasBeenInTrigger = true;
                    Instantiate(tomatoPrefab, newTomatoSpawnLocation, Quaternion.identity);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EdibleArea"))
            {
                CanBeEaten = false;
            }
        }
    } 
}

