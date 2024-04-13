using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWater : MonoBehaviour
{
    public GameObject prefabToCreate; // Reference to the prefab you want to instantiate
    public Vector3 spawnPosition; // Position where you want to spawn the object

    public float generationInterval = 1.0f;
    private bool generateObjects = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (
            OVRInput.Get( OVRInput.Button.Three )                           // Check that the A button is pressed
            && OVRInput.Get( OVRInput.Button.Four )                         // Check that the B button is pressed
        ) {
            StartCoroutine(GenerateObjectsCoroutine());
        }    
    }

    IEnumerator GenerateObjectsCoroutine()
    {
        // Continuously generate objects while the boolean variable is true
        while (generateObjects)
        {
            // Instantiate the object prefab
            Instantiate(prefabToCreate, spawnPosition, Quaternion.identity);

            // Wait for the specified interval before generating the next object
            yield return new WaitForSeconds(generationInterval);
        }
    }
}
