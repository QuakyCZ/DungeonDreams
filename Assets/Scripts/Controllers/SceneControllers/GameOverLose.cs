using System;
using Models.Files;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers.SceneControllers {
    public class GameOverLose : MonoBehaviour {
        [SerializeField] private Text title = null;
        [SerializeField] private Button mainMenuButton;

        private void Start() {
            ConfigFile.SetUp();
            Language.SetUp();
            title.text = Language.GetString(GameDictionaryType.titles, "gameOverLose");
            mainMenuButton.GetComponentInChildren<Text>().text =
                Language.GetString(GameDictionaryType.buttons, "mainMenu");
        }

        public void MainMenu() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
