using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    public string conditionName;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private bool finalDataSaved = false;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager in the scene!");
        }
        instance = this;
    }

    private void Start()
    {
        DateTime dateTime = DateTime.Now;
        string stringDateTime = dateTime.ToString("yyyy-MM-dd_HH.mm.ss");
        fileName = stringDateTime + "_" + fileName;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        NewGame();
    }

    public void SetConditionName(string conditionName)
    {
        this.conditionName = conditionName;
        gameData.SetScenarioName(conditionName);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.SetStartTime();
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        gameData.SaveDataToDictionary();
    }

    public void NextTest()
    {
        gameData.SaveDataToDictionary();
    }

    private void OnApplicationQuit()
    {
        if (!finalDataSaved)
        {
            // save data to a file using the data handler
            SaveGame();
            dataHandler.Save(gameData);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();
            dataHandler.Save(gameData);
        }
    }

    public void SaveData()
    {
        SaveGame();
    }

    public void SaveDataFinal()
    {
        SaveGame();
        dataHandler.Save(gameData);
        finalDataSaved = true;
    }

    public void UpdateWeightObjects()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        Debug.Log("Found " + dataPersistenceObjects.Count() + " data persistence objects");
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void SetHand(string hand)
    {
        gameData.SetHand(hand);
    }
}
