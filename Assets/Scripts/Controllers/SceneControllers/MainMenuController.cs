using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour{
    [SerializeField] private Button playButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button optionsButton = null;
    [SerializeField] private Button feedbackButton = null;
    [SerializeField] private Button exitButton = null;

    [SerializeField] private TextMeshProUGUI progress = null;

    private void Awake() {
        ConfigFile.SetUp();
        Language.SetUp();
    }

    void Start() {
        playButton.gameObject.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "play");

        creditsButton.gameObject.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "credits");

        optionsButton.gameObject.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "options");

        feedbackButton.gameObject.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "feedback");

        exitButton.gameObject.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "exit");
    }

    #region MainMenuButtons

    public void ButtonPlayGame() {
        SceneManager.LoadScene("Levels");
    }

    public void ButtonCredits() {
        SceneManager.LoadScene("Credits");
    }

    public void ButtonOptions() {
        SceneManager.LoadScene("Options");
    }

    public void ButtonBugReport() {
        SceneManager.LoadScene("BugReport");
    }

    public void ButtonExit() {
        Application.Quit();
    }

    #endregion
}