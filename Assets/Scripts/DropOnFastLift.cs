using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;
using Tilia.Interactions.Interactables.Interactors;
// using System;
// using System.Collections;
using System.Collections.Generic;
using Zinnia.Tracking.Velocity;

public class DropOnFastLift : MonoBehaviour
{
    public float maxVelocity = 2.0f;

    private InteractableFacade interactable;
    private Rigidbody interactableRigidbody;

    private bool startPositionSet = false;
    private Vector3 startPosition;
    private bool isOutsideOriginLimit = false;

    void Awake()
    {
        interactable = GetComponentInChildren<InteractableFacade>();
        Debug.Log("interactabler: " + interactable);
        interactableRigidbody = gameObject.GetComponent<Rigidbody>();
        Debug.Log("rigidbody: " + interactableRigidbody);
        CalculateMaxVelocity();
    }

    void FixedUpdate()
    {

        if (interactable && interactable.IsGrabbed)
        {
            if (!startPositionSet) {
                Debug.Log("Setting start position...");
                startPosition = transform.position;
                Debug.Log("Start position: " + startPosition);
                startPositionSet = true;
            }

            // Check if the weight is outside the origin position limits where the weight cannot be dropped
            //if (isOutsideOriginLimit) {
                // Get the list of grabbing interactors
                IReadOnlyList<InteractorFacade> grabbingInteractors = interactable.GrabbingInteractors;
                // Iterate through the grabbing interactors to see if any of them are moving too fast
                foreach (InteractorFacade interactor in grabbingInteractors)
                {
                    // Get the VelocityTrackerProcessor from the interactor's parent game object
                    VelocityTrackerProcessor velocityTrackerProcessor = interactor.transform.parent.gameObject.GetComponent<VelocityTrackerProcessor>();
                    // Get the current velocity from the VelocityTrackerProcessor
                    Vector3 currentVelocity = velocityTrackerProcessor.GetVelocity();
                    // Get the current velocity magnitude from the current velocity
                    float currentVelocityMagnitude = currentVelocity.magnitude;
                    Debug.Log("current velocity magnitude: " + currentVelocityMagnitude);
                    // If the current velocity magnitude is greater than the max velocity, ungrab the interactable
                    if (currentVelocityMagnitude > maxVelocity) {
                        // interactable.Ungrab(interactor);
                        interactor.Ungrab();
                        startPositionSet = false;
                        isOutsideOriginLimit = false;
                        break;
                    }
                }
            // } else {
            //     Debug.Log("IN ELSE: checking distance from origin...");
            //     CheckDistanceFromOrigin();
            // }

        }
    }

    private void CalculateMaxVelocity() {
        if (interactableRigidbody.mass > 9.83f) {
              maxVelocity = (1.9f / interactableRigidbody.mass + 0.06f);
        } else if (interactableRigidbody.mass > 2.36f) {
            // speedLimit = ((2 / (interactableRigidbody.mass))+0.1f); works good
            //speedLimit = ((1.5f / (interactableRigidbody.mass))+0.07f); // works good as well. A bit frustrating but I managed to get the first four scenes correct
            //speedLimit = ((2.3f / (interactableRigidbody.mass))+0.07f); // used for user study1
            //speedLimit = (4/(interactableRigidbody.mass+0.45f)-0.1f); //wip
            // 4/(x+0.9)-0.2 //wip
            //speedLimit = (1.9f / interactableRigidbody.mass + 0.06f); // prototype 3A
            //speedLimit = (-0.08f * interactableRigidbody.mass + 1.1f); // combining linear
            maxVelocity = (-0.08f * interactableRigidbody.mass + 1.05f); // combining linear
        } else {
            maxVelocity = (-0.8f * interactableRigidbody.mass + 2.75f); // prototype 3A or cobining linear
            //speedLimit = (-1.8f * interactableRigidbody.mass + 3.6f);
        }
        Debug.Log("max velocity: " + maxVelocity);
    }

    private void CalculateAngularDrag() {
        interactableRigidbody.angularDrag = (0.5f * (interactableRigidbody.mass*interactableRigidbody.mass));
    }

    private void CheckDistanceFromOrigin() {
        Vector3 distance = transform.position - startPosition;
        Debug.Log("distance: " + distance);
        if (distance.y > 0.5f && Mathf.Abs(distance.x) < 0.1f && Mathf.Abs(distance.z) < 0.1f) {
            Debug.Log("outside origin limit");
            isOutsideOriginLimit = true;
        }
    }
}