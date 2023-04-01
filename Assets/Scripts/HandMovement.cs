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

    // Test variables
    private Vector3 controllerInitialPosition;
    private Vector3 handInitialPosition;

    // Set in the Modified EventData of the Position ProperyModifer in Internal on the tracked alias
    public void UpdateLocation()
    {
        if (CDRatioEnabled && weightGrabbed)
        {
            Debug.Log("TESTING: Updating hand position");
            handTransform.position = handPosition;
            Debug.Log("Hand transform position in UpdateLocation: " + handPosition);
            Debug.Log("Controller transform position in UpdateLocation: " + controllerTransform.position);
        }
    }
    void LateUpdate()
    {
        if (CDRatioEnabled && weightGrabbed)
        {
            /** OLD WAY
            Debug.Log("TESTING: In Update");
            // Debug.Log("In update weight grabbed");
            // Hand position with C/D ratio that that takes into account the velocity of the controller
            Debug.Log("Hand position before changing: " + handPosition);
            Debug.Log("Handtransform position: " + handTransform.position);
            lastVelocity = velocityTrackerProcessor.GetVelocity();
            handPosition = initialPosition + Vector3.Scale(lastVelocity, new Vector3(1f, movementRatio, 1f)) * Time.deltaTime;
            Debug.Log("Hand position after changing: " + handPosition);
            // Hand position with C/D ratio that that takes into account the position of the controller according to the formula used in https://ieeexplore.ieee.org/document/9669918
            // handPosition = initialPosition + Vector3.Scale((controllerTransform.position - initialPosition), new Vector3(1.0f, movementRatio, 1.0f));
            initialPosition = handPosition;
            lastVelocity = velocityTrackerProcessor.GetVelocity();
            **/
            // new position s2 = s1 + v_avg * t
            // where s2 is the new position, s1 is the old position, v_avg is the average velocity, and t is the time
            
            
            // lastVelocity = velocityTrackerProcessor.GetVelocity();
            // Vector3 s2 = initialPosition + lastVelocity * Time.deltaTime;
            // handPosition = Vector3.Scale((s2 - initialPosition), new Vector3(1f, movementRatio, 1f)) + initialPosition;
            // initialPosition = handPosition;

            handPosition = Vector3.Scale((controllerTransform.position - initialPosition), new Vector3(1f, movementRatio, 1f)) + initialPosition;
        }
    }

    // Called on the Grabbed action InteractableFacade of the Interactions.Interactor
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
                movementRatio = 0.7f + 0.3f * (1f - ((mass - 1f) / 10f));
                // update the movement ratio based on the mass of the grabbed object. 
                // The highest mass of 11 should have a movement ratio of 0.7 
                // and the lowest mass of 1 should have a ratio of 1.3
                // movementRatio = 0.7f + 0.6f * (1f - ((mass - 1f) / 10f));

                // update the movement ratio based on the mass of the grabbed object. 
                // The highest mass of 11 should have a movement ratio of 0.05 
                // and the lowest mass of 1 should have a ratio of 0.3
                // movementRatio = 0.05f + 0.25f * (1f - ((mass - 1f) / 10f));

                // movementRatio = 0.05f + 0.95f * (1f - ((mass - 1f) / 10f));
                Debug.Log("Weight grabbed with mass " + mass + " and movement ratio " + movementRatio);

                controllerInitialPosition = controllerTransform.position;
                handInitialPosition = handTransform.position;
            }
            initialPosition = controllerTransform.position;
            lastVelocity = velocityTrackerProcessor.GetVelocity();
            weightGrabbed = true;
        }
    }

    // Called on the Ungrabbed action InteractableFacade of the Interactions.Interactor
    public void WeightUngrabbed()
    {
        Debug.Log("Weight ungrabbed");
        weightGrabbed = false;
        Debug.Log("Controller initial position: " + controllerInitialPosition);
        Debug.Log("Hand initial position: " + handInitialPosition);
        Debug.Log("Controller final position: " + controllerTransform.position);
        Debug.Log("Hand final position: " + handTransform.position);
        Debug.Log("Ratio of controller movement to hand movement: " + (handTransform.position.y - handInitialPosition.y) / (controllerTransform.position.y - controllerInitialPosition.y));
        Debug.Log("Total controller movement: " + (controllerTransform.position.y - controllerInitialPosition.y));
        Debug.Log("Total hand movement: " + (handTransform.position.y - handInitialPosition.y));
    }
}