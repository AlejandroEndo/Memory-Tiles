using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block {
    public int column;
    public int row;
    public int value;
}

[System.Serializable]
public class SaveFile {
    public Results results;
}

[System.Serializable]
public class Results {
    public int total_clicks;
    public float total_time;
}