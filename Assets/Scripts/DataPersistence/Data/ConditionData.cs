using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionData
{
    public int testNumber;
    public string scenarioName;
    public List<MassData> massDataList;
    public float startTime;
    public float endTime;
    public float timeToComplete;


    public ConditionData(int testNumber, string scenarioName) {
        this.testNumber = testNumber;
        this.scenarioName = scenarioName;
        massDataList = new List<MassData>();
        timeToComplete = 0f;
    }

    public void AddMassData(int initialPosition, int mass, int dropCount, int pickUpCount, int finalPosition, List<TrajectoryData> trajectoryDataList) {
        massDataList.Add(new MassData(initialPosition, mass, dropCount, pickUpCount, finalPosition, trajectoryDataList));
    }

    public void SetStartTime() {
        startTime = Time.time;
    }

    public void SetScenarioName(string scenarioName) {
        this.scenarioName = scenarioName;
    }

    public void FinalizeData() {
        endTime = Time.time;
        timeToComplete = endTime - startTime;
    }
}
