using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Controllers.SceneControllers {
    public class LevelsController: SceneController {
        [SerializeField] private Button mainMenuButton = null;
        private Text _currentButtonText = null;
        protected override void Translate() {
            base.Translate();
            mainMenuButton.GetComponentInChildren<Text>().text = Language.GetString(GameDictionaryType.buttons, "mainMenu");
        }

        public void LoadLevel(int level) {
            Debug.Log("Loading Level '" + level + "'");
            mainMenuButton.interactable = false;
            _currentButtonText = GameObject.Find("Level" + level + " Button").GetComponentInChildren<Text>();
            
            StartCoroutine(LoadLevelAsync("Floor_" + level));
            Debug.Log("Loading of '" + level + "' Completed.");
            
        }
        
        IEnumerator LoadLevelAsync(string sceneName) {
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (gameLevel.progress < 1) {
                _currentButtonText.text = Mathf.FloorToInt(gameLevel.progress * 100).ToString() + " %";
                yield return new WaitForEndOfFrame();
            }
        }
    }
}