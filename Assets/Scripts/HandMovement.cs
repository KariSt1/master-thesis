using UnityEngine;
using Tilia.Interactions.Interactables.Interactors;

public class HandMovement : MonoBehaviour
{
    public InteractorFacade controller;
    public float movementRatio = 1f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the difference in position between the controller and the hand
        Vector3 controllerPosition = controller.transform.position;
        Vector3 handPosition = transform.position;
        Vector3 positionDifference = controllerPosition - initialPosition;

        // Apply the movement ratio to the position difference and add it to the hand position
        Vector3 modifiedPositionDifference = positionDifference * movementRatio;
        transform.position = handPosition + modifiedPositionDifference;
    }
}