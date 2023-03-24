using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Condition name")]
    [SerializeField] private string conditionName;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    
    public static DataPersistenceManager instance {get; private set;}

    private void Awake() {
        if (instance != null ){
            Debug.LogError("More than one DataPersistenceManager in the scene!");
        }
        instance = this;
    }

    private void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        NewGame();
    }

    public void NewGame() {
        this.gameData = new GameData();
        this.gameData.SetStartTime();
    }

    public void SaveGame() {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(ref gameData);
        }
        gameData.SaveDataToDictionary();
        // save data to a file using the data handler
        dataHandler.Save(gameData);
    }

    public void NextTest() {
        gameData.SaveDataToDictionary();
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    public void SaveData() {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
