using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIController : MainController
{
    [Header("UI Objects")]
    public GameObject menu;

    [Header("Loading")]
    [Tooltip("This is lable for loading progress.")]
    public TextMeshProUGUI progress;

    [Header("Buttons")]
    public Button playButton;
    public Button exitButton;

    [Header("Messages for user")]
    [Tooltip("This text is for user. E.g. Press F when something.")]
    public Text logText;


    // Start is called before the first frame update
    void Start()
    {
        InstantiateVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown( KeyCode.Escape )) {
            ShowMenu( doUpdate );
        }
    }

    protected override void InstantiateVariables() {
        base.InstantiateVariables();
        uiController = this;
    }

    #region inGameMenu
    public void ShowMenu(bool enable = true) { // Also Resume Game button uses this method.
        PauseGameTime( enable );
        menu.SetActive( enable );
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene( "MainMenu", LoadSceneMode.Single );
    }
    #endregion


    #region MainMenuButtons
    public void ButtonPlayGame() {
        Debug.Log( "Button: Play Game" );
        playButton.interactable = false;
        exitButton.interactable = false;
        string sceneName = "Floor_0";
        StartCoroutine( LoadLevelAsync(sceneName) );        
        Debug.Log( "Loading of '" + sceneName + "' Completed." );
    }

    public void ButtonCredits() {
        SceneManager.LoadScene( "Credits" );
    }

    public void ButtonExit() {
        Application.Quit();
    }
    #endregion


    IEnumerator LoadLevelAsync(string sceneName) {
        Debug.Log( "Loading Level '" + sceneName + "'");
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync( sceneName, LoadSceneMode.Single );
        progress.enabled = true;
        while (gameLevel.progress < 1) {
            progress.text = Mathf.FloorToInt(gameLevel.progress*100).ToString() + "%";
            yield return new WaitForEndOfFrame();
        }
        
    }

    public void Log(string message ) {
        logText.text = message;
    }


}
