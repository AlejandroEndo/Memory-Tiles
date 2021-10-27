using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockTile : MonoBehaviour {

    public int number;
    public GameObject tmpObject;

    private TextMeshProUGUI tmp;

    void Start () {
    }

    void Update () {

    }

    public void SetValues (int value) {
        number = value;

        tmp = tmpObject.GetComponent<TextMeshProUGUI>();

        tmp.text = number.ToString();
    }
}
