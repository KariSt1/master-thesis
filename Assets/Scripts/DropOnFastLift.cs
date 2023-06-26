using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;
using Tilia.Interactions.Interactables.Interactors;
// using System;
// using System.Collections;
using System.Collections.Generic;
using Zinnia.Tracking.Velocity;

public class DropOnFastLift : MonoBehaviour, IDataPersistence
{
    public float maxVelocity = 2.0f;

    private InteractableFacade interactable;
    private Rigidbody interactableRigidbody;

    private bool startPositionSet = false;
    private Vector3 startPosition;
    private bool isOutsideOriginLimit = false;

    // this is a toggle between my values for maxVelocity and the values from Emma's paper
    public bool usePaperValues = false;

    // Is velocity limiting enabled
    public bool velocityLimitEnabled = false;

    // *** DATA COLLECTION ***
    // How often this weight has been dropped
    private int dropCount = 0;
    // How often this weight has been picked up
    private int pickUpCount = 0;
    // The mass of this weight
    private int mass = 0;
    // The initial position of this weight
    public int weightNumber = 0;
    // The index of the platform this weight is on
    private int platformIndex = -1;
    // Trajectory data list
    private List<TrajectoryData> trajectoryData = new List<TrajectoryData>();
    // Current trajectory data
    private TrajectoryData currentTrajectoryData;


    void Awake()
    {
        interactable = GetComponentInChildren<InteractableFacade>();
        interactableRigidbody = gameObject.GetComponent<Rigidbody>();
        CalculateMaxVelocity();
    }

    void FixedUpdate()
    {

        if (velocityLimitEnabled && interactable && interactable.IsGrabbed)
        {
            if (!startPositionSet)
            {
                startPosition = transform.position;
                startPositionSet = true;
            }

            // Check if the weight is outside the origin position limits where the weight cannot be dropped
            if (isOutsideOriginLimit)
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
                    // Get the current velocity magnitude y value from the current velocity
                    float currentVelocityYMagnitude = currentVelocity.y;
                    // If the current velocity y magnitude is greater than the max velocity, ungrab the interactable
                    // Positive velocity only happens on the way up, so you cannot drop it while going down
                    if (currentVelocityYMagnitude > maxVelocity)
                    {
                        // interactable.Ungrab(interactor);
                        interactor.Ungrab();
                        IncrementWeightDroppedCounter();
                        startPositionSet = false;
                        isOutsideOriginLimit = false;
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("IN ELSE: checking distance from origin...");
                CheckDistanceFromOrigin();
            }

        }
        // Add the current position to the trajectory data
        if (currentTrajectoryData != null && interactable && interactable.IsGrabbed)
        {
            currentTrajectoryData.AddTrajectoryPoint(transform.position);
        }
    }

    public void CalculateMaxVelocity()
    {
        if (usePaperValues) // Emma Bäckström's values
        {
            if (interactableRigidbody.mass > 9.83f)
            {
                maxVelocity = (1.9f / interactableRigidbody.mass + 0.06f);
            }
            else if (interactableRigidbody.mass > 2.36f)
            {
                maxVelocity = (-0.08f * interactableRigidbody.mass + 1.05f); // combining linear
            }
            else
            {
                maxVelocity = (-0.8f * interactableRigidbody.mass + 2.75f); // prototype 3A or cobining linear
            }
        }
        else // My values
        {
            maxVelocity = (1 / (Mathf.Log(interactableRigidbody.mass + 1, 10))) - 0.5f;
        }
    }

    private void CalculateAngularDrag()
    {
        if (velocityLimitEnabled)
        {
            interactableRigidbody.angularDrag = (0.5f * (interactableRigidbody.mass * interactableRigidbody.mass));
        }
    }

    private void CheckDistanceFromOrigin()
    {
        if (velocityLimitEnabled)
        {
            Vector3 distance = transform.position - startPosition;
            if (Mathf.Abs(distance.y) > 0.057f || Mathf.Abs(distance.x) > 0.057f || Mathf.Abs(distance.z) > 0.057f)
            {
                isOutsideOriginLimit = true;
            }
        }
    }

    public void ResetData()
    {
        dropCount = 0;
        pickUpCount = 0;
        platformIndex = -1;
        Debug.Log("resetting data, rigidbody mass: " + interactableRigidbody.mass);
        mass = Mathf.RoundToInt(interactableRigidbody.mass);
        Debug.Log("resetting data, mass: " + mass);
    }

    private void IncrementWeightDroppedCounter()
    {
        dropCount++;
    }

    public int GetWeightDroppedCounter()
    {
        return dropCount;
    }

    public void IncrementWeightPickedUpCounter()
    {
        pickUpCount++;
    }

    public int GetWeightPickedUpCounter()
    {
        return pickUpCount;
    }

    public void SaveData(ref GameData data)
    {
        data.AddMassData(weightNumber, mass, dropCount, pickUpCount, platformIndex, trajectoryData);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WeightPlacementCollider")
        {
            // Get the platform index from the WeightPlacementCollider script
            platformIndex = other.gameObject.GetComponent<WeightPlacementCollider>().GetPlatformIndex();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "WeightPlacementCollider")
        {
            platformIndex = -1;
        }
    }

    public void CreateTrajectoryData()
    {
        currentTrajectoryData = new TrajectoryData();
        trajectoryData.Add(currentTrajectoryData);
    }

    public void AddCurrentTrajectoryDataToList()
    {
        trajectoryData.Add(currentTrajectoryData);
    }
}