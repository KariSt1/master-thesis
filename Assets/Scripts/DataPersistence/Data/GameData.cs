using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<ConditionData> conditionDataList;
    private ConditionData currentConditionData;

    public GameData() {
        conditionDataList = new List<ConditionData>();
        currentConditionData = new ConditionData();
    }

    public void SaveDataToDictionary() {
        FinalizeConditionData();
    }

    public void SetStartTime() {
        currentConditionData.SetStartTime();
    }

    public void AddMassData(int initialPosition, int mass, int dropCount, int pickUpCount, int finalPosition) {
        currentConditionData.AddMassData(initialPosition, mass, dropCount, pickUpCount, finalPosition);
    }

    private void FinalizeConditionData() {
        currentConditionData.FinalizeData();
        conditionDataList.Add(currentConditionData);
        currentConditionData = new ConditionData();
        currentConditionData.SetStartTime();
    }
}
