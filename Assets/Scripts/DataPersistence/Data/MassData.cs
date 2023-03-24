using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MassData
{
    public int mass;
    public int dropCount;
    public int pickUpCount;

    public MassData(int mass, int dropCount, int pickUpCount) {
        this.mass = mass;
        this.dropCount = dropCount;
        this.pickUpCount = pickUpCount;
    }
}
