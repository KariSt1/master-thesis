using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<int, int> weightDroppedCounterByMass;
    public SerializableDictionary<int, int> weightPickedUpCounterByMass;

    public GameData() {
        weightDroppedCounterByMass = new SerializableDictionary<int, int>();
        weightPickedUpCounterByMass = new SerializableDictionary<int, int>();
    }
}
