using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MassData
{
    public int initialPosition;
    public int mass;
    public int dropCount;
    public int pickUpCount;
    public int finalPosition = -1;
    public List<TrajectoryData> trajectoryDataList;

    public MassData(int initialPosition, int mass, int dropCount, int pickUpCount, int finalPosition, List<TrajectoryData> trajectoryDataList) {
        this.initialPosition = initialPosition;
        this.mass = mass;
        this.dropCount = dropCount;
        this.pickUpCount = pickUpCount;
        this.finalPosition = finalPosition;
        this.trajectoryDataList = trajectoryDataList;
    }
}
