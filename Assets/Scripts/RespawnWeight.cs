using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnWeight : MonoBehaviour
{
    // The original position of the game object
    private Vector3 originalPosition;

    // The original euler angles of the game object
    private Vector3 originalEulerAngles;

    // The BoxCollider of the floor
    private BoxCollider floorCollider;

    // The BoxCollider of the game object

    // Start is called before the first frame update
    void Awake()
    {  
        // Set the original position of the game object
        originalPosition = transform.localPosition;
        originalEulerAngles = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game object collider is hitting the floor and respawn it if it is
        if (transform.position.y < 1.2f) {
            Respawn();
        }
    }

    public void Respawn() {
        transform.localPosition = originalPosition;
        transform.eulerAngles = originalEulerAngles;
    }
}