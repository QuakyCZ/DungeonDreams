using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject menu;
    public MainController mainController;
    public TextMeshProUGUI progress;
    public Button playButton;
    public Button exitButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown( KeyCode.Escape )) {
            ShowMenu( mainController.doUpdate );
        }
    }

    public void ShowMenu(bool enable = true) {
        mainController.PauseGameTime( enable );
        menu.SetActive( enable );
    }

    public void ButtonPlayGame() {
        
        playButton.interactable = false;
        exitButton.interactable = false;
        StartCoroutine( LoadAsyncOperation() );
    }

    

    public void ExitToMainMenu() {
        SceneManager.LoadScene( "MainMenu", LoadSceneMode.Single );
    }

    public void ButtonExit() {
        Application.Quit();
    }




    IEnumerator LoadAsyncOperation() {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync( "SampleScene", LoadSceneMode.Single );
        progress.enabled = true;
        while (gameLevel.progress < 1) {
            progress.text = Mathf.FloorToInt(gameLevel.progress*100).ToString() + "%";
            yield return new WaitForEndOfFrame();
        }
    }
}
