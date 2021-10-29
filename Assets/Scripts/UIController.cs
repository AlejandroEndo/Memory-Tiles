using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour {

    public Button startButton;
    public Label scoreLabel;

    void Start () {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("start-button");
        scoreLabel = root.Q<Label>("score-label");

        startButton.clicked += OnStartBottonPressed;

        scoreLabel.text = LoadScore();
    }

    public void OnStartBottonPressed () {
        SceneManager.LoadScene("MainScene");
    }

    private string LoadScore () {
        try {
            string jsonResultsFile = DataManager.LoadResult(); //Resources.Load<TextAsset>("results");

            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(jsonResultsFile);

            return "Time: " + saveFile.results.total_time + "s | Clicks: " + saveFile.results.total_clicks;
        } catch {
            return "Time: 0s | Clicks: 0";
        }

    }
}
