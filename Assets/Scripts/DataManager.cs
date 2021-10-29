using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager {

    public static string dirName = Application.dataPath + "/Data/";
    public static string fileName = "results.json";


    public static void SaveResult (int time, int clicks) {
        if (!Directory.Exists(dirName)) {
            Directory.CreateDirectory(dirName);
        }

        TextAsset jsonTextFile = Resources.Load<TextAsset>("results");
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(jsonTextFile.ToString());

        saveFile.results.total_clicks = clicks;
        saveFile.results.total_time = time;

        string json = JsonUtility.ToJson(saveFile);
        Debug.Log(dirName);
        File.WriteAllText(dirName+fileName, json);
    }

    public static string LoadResult() {
        return File.ReadAllText(dirName+fileName);
    }

    public static Blocks LoadGridData () {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("blocks_data");

        Blocks blocks = JsonUtility.FromJson<Blocks>(jsonTextFile.ToString());

        return blocks;
    }
}