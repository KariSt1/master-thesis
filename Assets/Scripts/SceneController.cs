using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneController : MonoBehaviour
{
    private string currentScenario = "SixWeights";
    private int currentTestNumber = 1;
    [SerializeField] GameObject handSelectionEnvironment;
    [SerializeField] GameObject heightCalibrationEnvironment;
    [SerializeField] GameObject liftTutorialEnvironment;
    [SerializeField] GameObject sixWeightsEnvironment;
    [SerializeField] GameObject twoWeightsEnvironment;
    [SerializeField] GameObject milkCartonEnvironment;

    [SerializeField] GameObject leftInteractor;
    [SerializeField] GameObject rightInteractor;

    [SerializeField] Transform cameraRig;

    [SerializeField] TwoWeightUICounter twoWeightUICounter;

    // Whether hand selection has been confirmed
    private bool handSelected = true;

    // Which hand is selcted
    private string selectedHand = "Right";

    // Array for six weights scenario
    public GameObject[] sixWeights;

    // Array for the placement cubes in the 6-weight scenario
    public GameObject[] sixWeightsPlacementCubes;

    // Array for number canvases for the 6-weight scenario
    public TextMeshProUGUI[] sixWeightsNumberCanvases;

    // Correct, incorrect materials
    public Material correctMaterial;
    public Material incorrectMaterial;

    // Normal material
    public Material normalMaterial;

    // Array for two weights scenario
    public GameObject[] twoWeights;

    // Array for milk carton scenario
    public GameObject[] milkCartons;

    // Array of possible masses for the weights
    private float[] massesArray = { 1f, 3f, 5f, 7f, 9f, 11f };
    private float[,] massesArrayPairs = { { 1f, 11f }, { 3f, 9f }, { 5f, 7f } };

    // Array for the order that the 2 weights will be randomized
    private int[] randomizeOrder = new int[30];
    private int twoWeightTestIndex = 0;

    // public DataPersistenceManager dataPersistenceManager;

    // Start is called before the first frame update
    void Start()
    {
        // // Randomize the masses of the weights
        // RandomizeMasses();
        // Set the condition name in the data persistence manager
        DataPersistenceManager.instance.SetConditionName(currentScenario);
        DataPersistenceManager.instance.SetHand("Right");
        DataPersistenceManager.instance.UpdateWeightObjects();
        foreach (GameObject weight in sixWeights)
        {
            weight.GetComponent<RespawnWeight>().Respawn();
        }
        // Randomize the masses of the weights
        RandomizeMasses();
    }

    public void RandomizeMasses()
    {
        // Randomize weights depending on current scenario
        if (currentScenario == "SixWeights")
        {
            // Randomize the mass of the six weights
            RandomizeSixWeights(sixWeights);
        }
        else if (currentScenario == "TwoWeights")
        {
            // Randomize the mass of the two weights
            RandomizeTwoWeights();
        }
        else if (currentScenario == "MilkCarton")
        {
            // Randomize the mass of the milk cartons
            RandomizeSixWeights(milkCartons);
        }
        // For each weight, get the DropOnFastLift script and set the max velocity
        if (currentScenario == "SixWeights")
        {
            foreach (GameObject weight in sixWeights)
            {
                DropOnFastLift dropOnFastLift = weight.GetComponent<DropOnFastLift>();
                dropOnFastLift.ResetData();
                dropOnFastLift.CalculateMaxVelocity();
            }
        }
        else if (currentScenario == "TwoWeights")
        {
            foreach (GameObject weight in twoWeights)
            {
                DropOnFastLift dropOnFastLift = weight.GetComponent<DropOnFastLift>();
                dropOnFastLift.ResetData();
                dropOnFastLift.CalculateMaxVelocity();
            }
        }
        else if (currentScenario == "MilkCarton")
        {
            foreach (GameObject weight in milkCartons)
            {
                DropOnFastLift dropOnFastLift = weight.GetComponent<DropOnFastLift>();
                dropOnFastLift.ResetData();
                dropOnFastLift.CalculateMaxVelocity();
            }
        }
    }

    // Function to randomize the 6 weights
    private void RandomizeSixWeights(GameObject[] weightArray)
    {
        // Temporary array with the possible masses
        float[] possibleMasses = massesArray;
        // Randomize the mass of the weights without repeating the same mass
        for (int i = 0; i < weightArray.Length; i++)
        {
            int randomIndex = Random.Range(0, possibleMasses.Length);
            weightArray[i].GetComponent<Rigidbody>().mass = possibleMasses[randomIndex];
            float[] tempArray = possibleMasses;
            possibleMasses = new float[possibleMasses.Length - 1];
            int j = 0;
            for (int k = 0; k < tempArray.Length; k++)
            {
                if (k != randomIndex)
                {
                    possibleMasses[j] = tempArray[k];
                    j++;
                }
            }
        }
    }

    // Function to randomize 2 weights
    private void RandomizeTwoWeights()
    {
        if (twoWeightTestIndex == 0)
        {
            RandomizePairOrder();
        }
        // Give the two weights a random mass from one of the pairs in the massesArrayPairs array
        int randomIndex = randomizeOrder[twoWeightTestIndex] % 3;
        twoWeightTestIndex++;
        int randomIndex2 = Random.Range(0, 2);
        twoWeights[0].GetComponent<Rigidbody>().mass = massesArrayPairs[randomIndex, randomIndex2];
        twoWeights[1].GetComponent<Rigidbody>().mass = massesArrayPairs[randomIndex, 1 - randomIndex2];
    }

    // A function that creates an array of length 30 with numbers from 0 to 2 where each number appears in a random order 10 times
    private void RandomizePairOrder()
    {
        // Create a 30 element array with numbers from 0 to 29
        int[] tempArray = new int[30];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = i;
        }
        // Randomize the order of the numbers in the randomizeOrder array
        for (int i = 0; i < 30; i++)
        {
            int randomIndex = Random.Range(0, tempArray.Length);
            randomizeOrder[i] = tempArray[randomIndex];
            int[] tempArray2 = tempArray;
            tempArray = new int[tempArray.Length - 1];
            int j = 0;
            for (int k = 0; k < tempArray2.Length; k++)
            {
                if (k != randomIndex)
                {
                    tempArray[j] = tempArray2[k];
                    j++;
                }
            }
        }
    }

    public void LeftControllerContinuePressed()
    {
        if (selectedHand == "Right")
        {
            ContinueToNextTest();
        }
    }

    public void RightControllerContinuePressed()
    {
        if (selectedHand == "Left")
        {
            ContinueToNextTest();
        }
    }

    // Function to reset the weights and randomize their masses
    public void ContinueToNextTest()
    {
        if (currentScenario == "HandSelection")
        {
            currentScenario = "HeightCalibration";
            // Disable the hand selection environment
            handSelectionEnvironment.SetActive(false);
            // Enable the height calibration environment
            heightCalibrationEnvironment.SetActive(true);
            DataPersistenceManager.instance.SetConditionName(currentScenario);
            handSelected = true;
        }
        else if (currentScenario == "HeightCalibration")
        {
            float cameraRigHeight = cameraRig.localPosition.y;
            // Set the height of all environments to 0.3 below the camera rig height
            handSelectionEnvironment.transform.position = new Vector3(handSelectionEnvironment.transform.position.x, cameraRigHeight + 0.35f, handSelectionEnvironment.transform.position.z);
            liftTutorialEnvironment.transform.position = new Vector3(liftTutorialEnvironment.transform.position.x, cameraRigHeight + 0.35f, liftTutorialEnvironment.transform.position.z);
            sixWeightsEnvironment.transform.position = new Vector3(sixWeightsEnvironment.transform.position.x, cameraRigHeight + 0.35f, sixWeightsEnvironment.transform.position.z);
            // two weights environment is 0.05 below the others
            twoWeightsEnvironment.transform.position = new Vector3(twoWeightsEnvironment.transform.position.x, cameraRigHeight + 0.355f, twoWeightsEnvironment.transform.position.z);
            milkCartonEnvironment.transform.position = new Vector3(milkCartonEnvironment.transform.position.x, cameraRigHeight + 0.35f, milkCartonEnvironment.transform.position.z);
            currentScenario = "LiftTutorial";
            // Disable the HeightCalibrationEnvironment game object
            heightCalibrationEnvironment.SetActive(false);
            // Enable the LiftTutorialEnvironment game object
            liftTutorialEnvironment.SetActive(true);
            DataPersistenceManager.instance.SetConditionName(currentScenario);
        }
        else if (currentScenario == "LiftTutorial")
        {
            currentScenario = "SixWeights";
            // Disable the LiftTutorialEnvironment game object
            liftTutorialEnvironment.SetActive(false);
            // Enable the SixWeightsEnvironment game object
            sixWeightsEnvironment.SetActive(true);
            DataPersistenceManager.instance.SetConditionName(currentScenario);
            DataPersistenceManager.instance.UpdateWeightObjects();
        }
        else if (currentScenario == "SixWeights")
        {
            DataPersistenceManager.instance.SetConditionName(currentScenario);
            DataPersistenceManager.instance.SaveData();
            // Six weights test is done
            currentScenario = "CheckingIfCorrect";
            // Set the condition name in the data persistence manager
            // Disable the SixWeightsEnvironment game object
            // sixWeightsEnvironment.SetActive(false);
            // Enable the TwoWeightsEnvironment game object
            // twoWeightsEnvironment.SetActive(true);

            // Check if each weight is in the correct position
            for (int i = 0; i < sixWeights.Length; i++)
            {
                // check CheckIfCorrectPlacement method in DropOnFastLift script
                (int correct, int placement) checkTuple = (sixWeights[i].GetComponent<DropOnFastLift>().CheckIfCorrectPlacement());
                Debug.Log("!!!!!!!!!!!!!! " + checkTuple.correct + " " + checkTuple.placement + " !!!!!!!!!!!!!");
                // if correct, change material to green on the placement object
                if (checkTuple.placement != -1) {
                    if (checkTuple.correct == checkTuple.placement) {
                        sixWeightsPlacementCubes[checkTuple.placement - 1].GetComponent<Renderer>().material = correctMaterial;
                    } else {
                        sixWeightsPlacementCubes[checkTuple.placement - 1].GetComponent<Renderer>().material = incorrectMaterial;
                    }
                    if (checkTuple.correct != -1) {
                        sixWeightsNumberCanvases[checkTuple.placement - 1].text = checkTuple.correct.ToString();
                    }
                }
            }
            
        }
        else if (currentScenario == "CheckingIfCorrect") {
            currentScenario = "SixWeights";
            for (int i=0; i< sixWeightsPlacementCubes.Length; i++) {
                sixWeightsPlacementCubes[i].GetComponent<Renderer>().material = normalMaterial;
                sixWeightsNumberCanvases[i].text = "";
            }
            DataPersistenceManager.instance.SetConditionName(currentScenario);
            DataPersistenceManager.instance.UpdateWeightObjects();
            currentTestNumber++;
        }
        else if (currentScenario == "TwoWeights")
        {
            currentTestNumber++;
            DataPersistenceManager.instance.SaveData();
            twoWeightUICounter.AddOne();
            // Check if two weights test is done
            if (twoWeightTestIndex == 30)
            {
                // two weight test is done
                twoWeightTestIndex = 0;
                currentScenario = "MilkCarton";
                // Disable the TwoWeightsEnvironment game object
                twoWeightsEnvironment.SetActive(false);
                // Enable the MilkCartonEnvironment game object
                milkCartonEnvironment.SetActive(true);
                // Set the condition name in the data persistence manager
                DataPersistenceManager.instance.SetConditionName(currentScenario);
                DataPersistenceManager.instance.UpdateWeightObjects();
            }
        }
        else if (currentScenario == "MilkCarton")
        {
            // DataPersistenceManager.instance.SetConditionName(currentScenario);
            // Milk carton test is done and the experiment is done
#if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
#else
            DataPersistenceManager.instance.SaveDataFinal();
            Application.Quit();
#endif
            return;
        }
        // For each weight, reset its position and rotation
        GameObject[] weights;
        if (currentScenario == "SixWeights")
        {
            weights = sixWeights;
        }
        else if (currentScenario == "TwoWeights")
        {
            weights = twoWeights;
        }
        else if (currentScenario == "MilkCarton")
        {
            weights = milkCartons;
        }
        else
        {
            weights = new GameObject[0];
        }
        foreach (GameObject weight in weights)
        {
            weight.GetComponent<RespawnWeight>().Respawn();
        }
        // Randomize the masses of the weights
        RandomizeMasses();
    }

    public void SelectLeftHand()
    {
        if (!handSelected)
        {
            DataPersistenceManager.instance.SetHand("Left");
            // Disable the game object
            leftInteractor.SetActive(true);
            rightInteractor.SetActive(false);
            selectedHand = "Left";
        }
    }

    public void SelectRightHand()
    {
        if (!handSelected)
        {
            DataPersistenceManager.instance.SetHand("Right");
            rightInteractor.SetActive(true);
            leftInteractor.SetActive(false);
            selectedHand = "Right";
        }
    }
}
