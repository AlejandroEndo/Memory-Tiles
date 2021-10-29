using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour {

    [Header("UI GamePlay")]
    public Label timerLabel;
    public Label clickLabel;

    [Header("UI GameOver")]
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

    #region GameOver
    public void GameOver() {
        gameOver = true;

        DataManager.SaveResult(time, clickCount);

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
    #endregion
}
