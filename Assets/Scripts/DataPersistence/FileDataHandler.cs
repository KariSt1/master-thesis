using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public void Save(GameData data) {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object to JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e.Message);
        }
    }
}
