using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionData
{
    public List<int> initialOrder;
    public List<int> finalPlacement;
    public List<MassData> massDataList;
    public float startTime;
    public float endTime;
    public float timeToComplete;


    public ConditionData() {
        initialOrder = new List<int>();
        finalPlacement = new List<int>();
        massDataList = new List<MassData>();
        timeToComplete = 0f;
    }

    public void AddMassData(int mass, int dropCount, int pickUpCount) {
        massDataList.Add(new MassData(mass, dropCount, pickUpCount));
    }

    public void SetStartTime() {
        startTime = Time.time;
    }

    public void FinalizeData() {
        endTime = Time.time;
        timeToComplete = endTime - startTime;
    }
}
