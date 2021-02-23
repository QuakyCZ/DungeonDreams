using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Controllers.SceneControllers {
    public class LevelsController: SceneController {
        [SerializeField] private Button mainMenuButton;
        [Header("Level management")]
        [SerializeField] private List<Button> levelButtons = new List<Button>();
        
        private Text _currentButtonText;
        protected override void Translate() {
            base.Translate();
            mainMenuButton.GetComponentInChildren<Text>().text = Language.GetString(GameDictionaryType.buttons, "mainMenu");
        }

        protected override void Start() {
            base.Start();
            
            List<int> achieved = LevelManager.GetUnlockedLevels();
            
            if (achieved.Count == 0) {
                levelButtons[0].interactable = true;
                return;
            }
            
            foreach (int x in achieved) {
                levelButtons[x].interactable = true;
            }
            
            if(levelButtons.Count > achieved.Max() + 1)
                levelButtons[achieved.Max() + 1].interactable = true;
        }

        public void LoadLevel(int level) {
            Debug.Log("Loading Level '" + level + "'");
            mainMenuButton.interactable = false;
            _currentButtonText = GameObject.Find("Level" + level + " Button").GetComponentInChildren<Text>();
            
            StartCoroutine(LoadLevelAsync("Floor_" + level));
            PlayerPrefs.SetInt("current-level", level);
            PlayerPrefs.Save();
            Debug.Log("Loading of '" + level + "' Completed.");
            
        }
        
        IEnumerator LoadLevelAsync(string sceneName) {
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (gameLevel.progress < 1) {
                _currentButtonText.text = Mathf.FloorToInt(gameLevel.progress * 100) + " %";
                yield return new WaitForEndOfFrame();
            }
        }
    }
}