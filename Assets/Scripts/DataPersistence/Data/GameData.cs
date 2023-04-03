using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string hand;
    public List<ConditionData> conditionDataList;
    private ConditionData currentConditionData;
    private int testNumber = 1;
    private string scenarioName = "Initial";

    public GameData() {
        this.hand = "";
        conditionDataList = new List<ConditionData>();
        currentConditionData = new ConditionData(testNumber++, scenarioName);
    }

    public void SaveDataToDictionary() {
        FinalizeConditionData();
    }

    public void SetStartTime() {
        currentConditionData.SetStartTime();
    }

    public void AddMassData(int initialPosition, int mass, int dropCount, int pickUpCount, int finalPosition, List<TrajectoryData> trajectoryDataList) {
        currentConditionData.AddMassData(initialPosition, mass, dropCount, pickUpCount, finalPosition, trajectoryDataList);
    }

    public void SetScenarioName(string scenarioName) {
        this.scenarioName = scenarioName;
        currentConditionData.SetScenarioName(scenarioName);
    }

    public void SetHand(string hand) {
        this.hand = hand;
    }

    private void FinalizeConditionData() {
        currentConditionData.FinalizeData();
        conditionDataList.Add(currentConditionData);
        currentConditionData = new ConditionData(testNumber++, scenarioName);
        currentConditionData.SetStartTime();
    }

}
