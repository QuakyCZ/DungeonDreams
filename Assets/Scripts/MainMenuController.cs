using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenuController : MonoBehaviour {
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private TextMeshProUGUI progress;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    #region MainMenuButtons
    public void ButtonPlayGame() {
        Debug.Log( "Button: Play Game" );
        playButton.interactable = false;
        exitButton.interactable = false;
        string sceneName = "Floor_0";
        StartCoroutine( LoadLevelAsync( sceneName ) );
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
        Debug.Log( "Loading Level '" + sceneName + "'" );
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync( sceneName, LoadSceneMode.Single );
        progress.enabled = true;
        while (gameLevel.progress < 1) {
            progress.text = Mathf.FloorToInt( gameLevel.progress * 100 ).ToString() + "%";
            yield return new WaitForEndOfFrame();
        }

    }
}
