using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerOld : MonoBehaviour
{

    // Array of the different weight game objects
    public GameObject[] weights;

    // Array of possible masses for the weights
    private float[] massesArray = { 1f, 3f, 5f, 7f, 9f, 11f };
    private float[,] massesArrayPairs = { { 1f, 11f }, { 3f, 9f }, { 5f, 7f }};

    // Array for the order that the 2 weights will be randomized
    private int[] randomizeOrder = new int[30];
    private int twoWeightTestIndex = 0;

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
        if (twoWeightTestIndex == 0) {
            RandomizePairOrder();
        }
        // Give the two weights a random mass from one of the pairs in the massesArrayPairs array
        int randomIndex = randomizeOrder[twoWeightTestIndex]%3;
        Debug.Log("Random index: " + randomIndex);
        twoWeightTestIndex++;
        int randomIndex2 = Random.Range(0, 2);
        weights[0].GetComponent<Rigidbody>().mass = massesArrayPairs[randomIndex, randomIndex2];
        weights[1].GetComponent<Rigidbody>().mass = massesArrayPairs[randomIndex, 1 - randomIndex2];
    }

    // A function that creates an array of length 30 with numbers from 0 to 2 where each number appears in a random order 10 times
    private void RandomizePairOrder() {
        // Create a 30 element array with numbers from 0 to 29
        int[] tempArray = new int[30];
        for (int i = 0; i < tempArray.Length; i++) {
            tempArray[i] = i;
        }
        // Randomize the order of the numbers in the randomizeOrder array
        for (int i = 0; i < 30; i++) {
            int randomIndex = Random.Range(0, tempArray.Length);
            randomizeOrder[i] = tempArray[randomIndex];
            int[] tempArray2 = tempArray;
            tempArray = new int[tempArray.Length - 1];
            int j = 0;
            for (int k = 0; k < tempArray2.Length; k++) {
                if (k != randomIndex) {
                    tempArray[j] = tempArray2[k];
                    j++;
                }
            }
        }
    }


    // Function to reset the weights and randomize their masses
    public void ResetWeights() {
        if (twoWeightTestIndex == 30) {
            // two weight test is done
            twoWeightTestIndex = 0;
            // Application.Quit(); // TODO: change this to go to the next scene
            #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
            return;
        }
        DataPersistenceManager.instance.SaveData();
        // For each weight, reset its position and rotation
        foreach (GameObject weight in weights) {
            weight.GetComponent<RespawnWeight>().Respawn();
        }
        // Randomize the masses of the weights
        RandomizeMasses();
    }
}
