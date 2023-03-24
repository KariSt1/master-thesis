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

    public void AddInitialOrder(List<int> initialOrder) {
        currentConditionData.initialOrder = initialOrder;
    }

    public void AddFinalPlacement(List<int> finalPlacement) {
        currentConditionData.finalPlacement = finalPlacement;
    }

    public void AddMassData(int mass, int dropCount, int pickUpCount) {
        currentConditionData.AddMassData(mass, dropCount, pickUpCount);
    }

    private void FinalizeConditionData() {
        currentConditionData.FinalizeData();
        conditionDataList.Add(currentConditionData);
        currentConditionData = new ConditionData();
        currentConditionData.SetStartTime();
    }
}
