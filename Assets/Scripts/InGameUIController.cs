using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour {

    public Label timerLabel;
    public Label clickLabel;

    public VisualElement gameOverPanel;
    public Label gameOverScoreLabel;
    public Button replayButton;

    private int time;
    private int clickCount;

    private bool gameOver = false;

    void Start () {
        time = 0;
        clickCount = 0;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        timerLabel = root.Q<Label>("timer");
        clickLabel = root.Q<Label>("click-count");

        gameOverPanel = root.Q<VisualElement>("game-over-panel");
        gameOverScoreLabel = root.Q<Label>("score-label");
        replayButton = root.Q<Button>("replay-button");

        replayButton.clicked += Replay;

        gameOverPanel.style.display = DisplayStyle.None;

        timerLabel.text = time + "s";
        clickLabel.text = "Clicks: " + clickCount;

        StartCoroutine(GameTimer());
    }

    private void Replay() {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickUpdated () {
        clickCount++;
        clickLabel.text = "Clicks: " + clickCount;
    }

    public void GameOver() {
        gameOver = true;
        //StopCoroutine("GameTimer");

        TextAsset jsonTextFile = Resources.Load<TextAsset>("results");
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(jsonTextFile.ToString());
        saveFile.results.total_clicks = clickCount;
        saveFile.results.total_time = time;

        Debug.Log(saveFile);

        DataManager.SaveResult(saveFile);

        gameOverScoreLabel.text = "Time: " + time + "s | Clicks: " + clickCount;

        gameOverPanel.style.display = DisplayStyle.Flex;
    }

    IEnumerator GameTimer () {
        while (!gameOver) {
            yield return new WaitForSeconds(1f);
            time++;
            timerLabel.text = time + "s";
        }
    }
}
