using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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
        TextAsset jsonResultsFile = Resources.Load<TextAsset>("results");

        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(jsonResultsFile.ToString());

        return "Time: " + saveFile.results.total_time + "s | Clicks: " + saveFile.results.total_clicks;
    }
}
