using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterContainer : MonoBehaviour
{

    public string waterTag = "Water"; // Tag assigned to the water object
    public AudioClip pouringSound; // Sound to play when pouring

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a water object
        if (other.CompareTag(waterTag))
        {
            // Play the pouring sound effect
            if (pouringSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pouringSound);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // You can add further actions if needed when the water object exits the container
    }
}
