using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    public Material highlightMaterial; // Material for highlighting
    public LineRenderer lineRenderer;
    public float maxDistance = 10f; // Maximum distance for the raycast
    
    private Transform controllerTransform;
    private GameObject previousHitObject; // Previously highlighted object
    private Material previousMaterial; // Material of the previously highlighted object

    private GameObject grabbedObject;
    private Rigidbody grabbedRigidbody;
    private bool isRaycastHit = false;

    void HighlightObject(GameObject obj)
    {
        // Store the original material of the object
        previousMaterial = obj.GetComponent<Renderer>().material;

        // Apply the highlight material to the object
        obj.GetComponent<Renderer>().material = highlightMaterial;
    }

    void UnhighlightObject(GameObject obj)
    {
        // Revert the object's material to the original one
        obj.GetComponent<Renderer>().material = previousMaterial;
    }

    void GrabObject()
    {
        // Disable physics interactions while the object is grabbed
        grabbedRigidbody.isKinematic = true;
        grabbedObject.transform.SetParent(controllerTransform);
    }

    void ReleaseObject()
    {
        // Enable physics interactions and release the object
        grabbedRigidbody.isKinematic = false;
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
        grabbedRigidbody = null;
    }

    void UpdateGrabbedObject()
    {
        // TODO figure out the pulling thing
        // float distance = Vector3.Distance( controllerTransform.position, grabbedObject.transform.position );

        // Update the position and rotation of the grabbed object to match the controller
        grabbedObject.transform.position = controllerTransform.position + controllerTransform.forward * 1.5f;
        grabbedObject.transform.rotation = controllerTransform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        controllerTransform = GameObject.Find("RightControllerAnchor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get( OVRInput.Axis1D.SecondaryHandTrigger ) > 0.5 && OVRInput.Get( OVRInput.Axis1D.SecondaryIndexTrigger ) > 0.5)
        {
            // If an object is grabbed, update its position and rotation
            if (grabbedObject != null)
            {
                UpdateGrabbedObject();
            }
            else
            {
                // Grab the object if it has a Rigidbody component
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
                if (grabbedRigidbody != null)
                {
                    GrabObject();
                }
            }
            lineRenderer.enabled = false;
        }
        else {
            if (grabbedRigidbody != null) ReleaseObject();
        
            // Perform raycasting to detect objects the controller is hovering over
            RaycastHit hit;
            Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
            isRaycastHit = Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, maxDistance);
            if (isRaycastHit)
            {
                // Update the positions of the LineRenderer to match the ray
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, controllerTransform.position); // Start position
                lineRenderer.SetPosition(1, hit.point); // End position (where the ray hits)
                
                // TODO match for tag instead of matching name
                if (hit.transform.name != "Terrain") {
                    GameObject hitObject = hit.collider.gameObject;
                    grabbedObject = hitObject;

                    // Check if the hit object is different from the previously highlighted object
                    if (hitObject != previousHitObject)
                    {
                        // Unhighlight the previous object if it exists
                        if (previousHitObject != null)
                        {
                            UnhighlightObject(previousHitObject);
                        }

                        // Highlight the new hit object
                        HighlightObject(hitObject);

                        // Update the previously highlighted object
                        previousHitObject = hitObject;
                    }
                }
                else {
                    if (previousHitObject != null)
                    {
                        UnhighlightObject(previousHitObject);
                        previousHitObject = null;
                    }
                    grabbedObject = null;

                    // TODO for teleportaion

                }
            }
            else
            {
                // If no object is hit, unhighlight the previous object if it exists
                if (previousHitObject != null)
                {
                    UnhighlightObject(previousHitObject);
                    previousHitObject = null;
                }
                lineRenderer.enabled = false;
                grabbedObject = null;
            }
        }
    }
}
