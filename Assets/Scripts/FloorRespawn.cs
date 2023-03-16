using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRespawn : MonoBehaviour
{
    // The original position of the game object
    private Vector3 originalPosition;

    // The original rotation of the game object
    private Quaternion originalRotation;

    // The BoxCollider of the floor
    private BoxCollider floorCollider;

    // The BoxCollider of the game object

    // Start is called before the first frame update
    void Start()
    {  
        // Set the original position of the game object
        originalPosition = transform.position;
        // Set the original rotation of the game object
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game object collider is hitting the floor and respawn it if it is
        if (transform.position.y < 1.2f) {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}


// TODO: Fix the rotation of the weight when it is respawned