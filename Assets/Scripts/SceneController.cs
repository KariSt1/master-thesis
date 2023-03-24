using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    // Array of the different weight game objects
    public GameObject[] weights;

    // Array of possible masses for the weights
    private float[] massesArray = { 1f, 3f, 5f, 7f, 9f, 11f };
    // The same array as above expect it is a 2D array with pairs
    private float[,] massesArrayPairs = { { 1f, 11f }, { 3f, 9f }, { 5f, 7f }};

    // public DataPersistenceManager dataPersistenceManager;

    // Start is called before the first frame update
    void Start()
    {    
        // Randomize the masses of the weights
        RandomizeMasses();
    }

    public void RandomizeMasses() {
        // If there are 6 weights, randomize the mass of all of them
        if (weights.Length == 6) {
            RandomizeSixWeights();
        // If there are 2 weights, randomize the mass of only two of them
        } else if (weights.Length == 2) {
            RandomizeTwoWeights();
        }
        // For each weight, get the DropOnFastLift script and set the max velocity
        foreach (GameObject weight in weights) {
            DropOnFastLift dropOnFastLift = weight.GetComponent<DropOnFastLift>();
            dropOnFastLift.ResetData();
            dropOnFastLift.CalculateMaxVelocity();
        }
    }

    // Function to randomize the 6 weights
    private void RandomizeSixWeights() {
        // Temporary array with the possible masses
        float[] possibleMasses = massesArray;
        // Randomize the mass of the weights without repeating the same mass
        for (int i = 0; i < weights.Length; i++) {
            int randomIndex = Random.Range(0, possibleMasses.Length);
            weights[i].GetComponent<Rigidbody>().mass = possibleMasses[randomIndex];
            float[] tempArray = possibleMasses;
            possibleMasses = new float[possibleMasses.Length - 1];
            int j = 0;
            for (int k = 0; k < tempArray.Length; k++) {
                if (k != randomIndex) {
                    possibleMasses[j] = tempArray[k];
                    j++;
                }
            }
        }
    }

    // Function to randomize 2 weights
    private void RandomizeTwoWeights() {
        // Give the two weights a random mass from one of the pairs in the massesArrayPairs array
        int randomIndex = Random.Range(0, 3);
        int randomIndex2 = Random.Range(0, 2);
        weights[0].GetComponent<Rigidbody>().mass = massesArrayPairs[randomIndex, randomIndex2];
        weights[1].GetComponent<Rigidbody>().mass = massesArrayPairs[randomIndex, 1 - randomIndex2];
    }

    // Function to reset the weights and randomize their masses
    public void ResetWeights() {
        DataPersistenceManager.instance.SaveData();
        // For each weight, reset its position and rotation
        foreach (GameObject weight in weights) {
            weight.GetComponent<RespawnWeight>().Respawn();
        }
        // Randomize the masses of the weights
        RandomizeMasses();
    }
}
