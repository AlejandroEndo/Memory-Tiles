using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public SaveFile sf;

    void Update () {
        // Save Data
        if (Input.GetKeyDown(KeyCode.Space)) {
            DataManager.SaveResult(sf);
        }

        // Load Data
        if (Input.GetKeyDown(KeyCode.S)) {
            //sf = DataManager.Load();
            DataManager.LoadGridData();
        }
    }
}
