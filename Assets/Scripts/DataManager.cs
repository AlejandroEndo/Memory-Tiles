using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager {

    public static string dirName = "Assets/Resources/";
    public static string fileName = "results.json";

    public static void SaveResult (SaveFile file) {
        if (!Directory.Exists(dirName)) {
            Directory.CreateDirectory(dirName);
        }

        string json = JsonUtility.ToJson(file);
        File.WriteAllText(dirName + fileName, json);
    }

    public static Blocks LoadGridData () {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("blocks_data");

        Blocks blocks = JsonUtility.FromJson<Blocks>(jsonTextFile.ToString());

        Debug.Log(jsonTextFile);

        return blocks;
    }

    //public static SaveFile Load () {
    //    string fullPath = dirName + fileName;
    //    SaveFile sf = new SaveFile();

    //    if (File.Exists(fullPath)) {
    //        Debug.Log(fullPath);
    //        string json = File.ReadAllText(fullPath);
    //        sf = JsonUtility.FromJson<SaveFile>(json);
    //    } else {
    //        Debug.Log("Safe File does not exist.");
    //    }

    //    return sf;
    //}
}