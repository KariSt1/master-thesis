using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;
using Tilia.Interactions.Interactables.Interactors;
using Zinnia.Tracking.Velocity;
using System.Collections.Generic;

public class HandMovement : MonoBehaviour
{
    // The transform of the real life controller
    public Transform controllerTransform;
    // The transform of the virtual hand
    public Transform handTransform;
    // The C/D ratio
    public float movementRatio = 1f;
    // Velocity tracker processor to get the velocity of the controller
    public VelocityTrackerProcessor velocityTrackerProcessor;
    // Interactor facade to get the grabbed objects
    public InteractorFacade interactorFacade;

    // The initial position of the hand
    private Vector3 initialPosition;
    // The last velocity of the controller
    private Vector3 lastVelocity;
    // The current position of the virtual hand
    private Vector3 handPosition;
    // Whether the weight is grabbed or not

    // Is C/D ratio modification emabled
    public bool CDRatioEnabled = false;

    private bool weightGrabbed = false;


    public void UpdateLocation()
    {
        if (CDRatioEnabled && weightGrabbed)
        {
            handTransform.position = handPosition;
        }
    }
    void Update()
    {
        if (CDRatioEnabled && weightGrabbed)
        {
            // Debug.Log("In update weight grabbed");
            // Hand position with C/D ratio that that takes into account the velocity of the controller
            handPosition = initialPosition + Vector3.Scale(lastVelocity, new Vector3(1f, movementRatio, 1f)) * Time.deltaTime;
            // Hand position with C/D ratio that that takes into account the position of the controller according to the formula used in https://ieeexplore.ieee.org/document/9669918
            // handPosition = initialPosition + Vector3.Scale((controllerTransform.position - initialPosition), new Vector3(1.0f, movementRatio, 1.0f));
            initialPosition = handPosition;
            lastVelocity = velocityTrackerProcessor.GetVelocity();
        }
    }

    public void WeightGrabbed()
    {
        if (CDRatioEnabled) {
            // Get the interactable that is grabbed
            IReadOnlyList<GameObject> GrabbedObjects = interactorFacade.GrabbedObjects;
            foreach (GameObject grabbedObject in GrabbedObjects)
            {
                // get the mass of the grabbed object
                float mass = grabbedObject.GetComponent<Rigidbody>().mass;
                // update the movement ratio based on the mass of the grabbed object. 
                // The highest mass of 11 should have a movement ratio of 0.7 
                // and the lowest mass of 1 should have a ratio of 1.0
                // movementRatio = 0.7f + 0.3f * (1f - ((mass - 1f) / 10f));
                // update the movement ratio based on the mass of the grabbed object. 
                // The highest mass of 11 should have a movement ratio of 0.7 
                // and the lowest mass of 1 should have a ratio of 1.3
                movementRatio = 0.7f + 0.6f * (1f - ((mass - 1f) / 10f));

                // update the movement ratio based on the mass of the grabbed object. 
                // The highest mass of 11 should have a movement ratio of 0.05 
                // and the lowest mass of 1 should have a ratio of 0.3
                // movementRatio = 0.05f + 0.25f * (1f - ((mass - 1f) / 10f));

                // movementRatio = 0.05f + 0.95f * (1f - ((mass - 1f) / 10f));
                Debug.Log("Weight grabbed with mass " + mass + " and movement ratio " + movementRatio);
            }
            initialPosition = controllerTransform.position;
            lastVelocity = velocityTrackerProcessor.GetVelocity();
            weightGrabbed = true;
        }
    }

    public void WeightUngrabbed()
    {
        Debug.Log("Weight ungrabbed");
        weightGrabbed = false;
    }
}