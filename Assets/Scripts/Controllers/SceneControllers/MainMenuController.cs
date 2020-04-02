using System.Collections;
using Models.Files;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers.SceneControllers {
    public class MainMenuController : MonoBehaviour {
        [SerializeField] private Button playButton = null;
        [SerializeField] private Button creditsButton = null;
        [SerializeField] private Button optionsButton = null;
        [SerializeField] private Button feedbackButton = null;
        [SerializeField] private Button exitButton = null;
    
        [SerializeField] private TextMeshProUGUI progress = null;

        // Start is called before the first frame update
        void Start() {
            ConfigFile.SetUp();
            Language.SetUp();
        
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

        // Update is called once per frame
        void Update() { }

        #region MainMenuButtons

        public void ButtonPlayGame() {
            Debug.Log("Button: Play Game");
            playButton.interactable = false;
            creditsButton.interactable = false;
            optionsButton.interactable = false;
            feedbackButton.interactable = false;
            exitButton.interactable = false;
            string sceneName = "Floor_0";
            StartCoroutine(LoadLevelAsync(sceneName));

            Debug.Log("Loading of '" + sceneName + "' Completed.");
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

        IEnumerator LoadLevelAsync(string sceneName) {
            Debug.Log("Loading Level '" + sceneName + "'");
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            progress.enabled = true;
            while (gameLevel.progress < 1) {
                progress.text = Mathf.FloorToInt(gameLevel.progress * 100).ToString() + "%";
                yield return new WaitForEndOfFrame();
            }
        }
    }
}