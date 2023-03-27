using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionData
{
    public int testNumber;
    public List<MassData> massDataList;
    public float startTime;
    public float endTime;
    public float timeToComplete;


    public ConditionData(int testNumber) {
        this.testNumber = testNumber;
        massDataList = new List<MassData>();
        timeToComplete = 0f;
    }

    public void AddMassData(int initialPosition, int mass, int dropCount, int pickUpCount, int finalPosition) {
        massDataList.Add(new MassData(initialPosition, mass, dropCount, pickUpCount, finalPosition));
    }

    public void SetStartTime() {
        startTime = Time.time;
    }

    public void FinalizeData() {
        endTime = Time.time;
        timeToComplete = endTime - startTime;
    }
}
