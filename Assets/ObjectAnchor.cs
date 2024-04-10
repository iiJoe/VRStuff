using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnchor : MonoBehaviour
{
    [Header( "Grasping Properties" )]
    public float graspingRadius = 0.1f;

    public float get_grasping_radius () { return graspingRadius; }

    // Store the hand controller this object will be attached to
    protected HandController hand_controller = null;

    public bool is_available () { return hand_controller == null; }

    // Store initial transform parent
    protected Transform initial_transform_parent;

    public void attach_to ( HandController hand_controller ) {
        // Store the hand controller in memory
        this.hand_controller = hand_controller;

        // Set the object to be placed in the hand controller referential
        transform.SetParent( hand_controller.transform );
    }

    public void detach_from ( HandController hand_controller ) {
        // Make sure that the right hand controller ask for the release
        if ( this.hand_controller != hand_controller ) return;

        // Detach the hand controller
        this.hand_controller = null;

        // Set the object to be placed in the original transform parent
        transform.SetParent( initial_transform_parent );
    }

    // Start is called before the first frame update
    void Start()
    {
	    initial_transform_parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
