using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUIController : MonoBehaviour {

    public Label timerLabel;
    public Label clickLabel;

    private int time;
    private int clickCount;

    void Start () {
        time = 0;
        clickCount = 0;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        timerLabel = root.Q<Label>("timer");
        clickLabel = root.Q<Label>("click-count");

        timerLabel.text = time + "s";
        clickLabel.text = "Clicks: " + clickCount;

        StartCoroutine(GameTimer());
    }

    public void OnClickUpdated () {
        clickCount++;
        clickLabel.text = "Clicks: " + clickCount;
    }

    IEnumerator GameTimer () {
        while (true) {
            yield return new WaitForSeconds(1f);
            time++;
            timerLabel.text = time + "s";
        }
    }
}
