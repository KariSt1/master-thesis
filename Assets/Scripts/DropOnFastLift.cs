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

    void Start()
    {
        interactable = GetComponentInChildren<InteractableFacade>();
        Debug.Log("interactabler: " + interactable);
    }

    void FixedUpdate()
    {
        if (interactable && interactable.IsGrabbed)
        {
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
                    interactable.Ungrab(interactor);
                }
            }
        }
    }
}