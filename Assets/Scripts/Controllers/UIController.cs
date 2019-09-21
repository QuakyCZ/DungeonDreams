using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject menu;
    public MainController mainController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenu(bool enable = true) {
        mainController.PauseGameTime( enable );
        menu.SetActive( enable );
    }

    public void ButtonPlayGame() {
        SceneManager.LoadScene( "SampleScene", LoadSceneMode.Single );
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene( "MainMenu", LoadSceneMode.Single );
    }

    public void ButtonExit() {
        Application.Quit();
    }

}
