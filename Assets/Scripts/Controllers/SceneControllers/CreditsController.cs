using Models.Files;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers.SceneControllers {
    public class CreditsController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI title = null;
        [SerializeField] private TextMeshProUGUI mainDeveloper = null;
        [SerializeField] private TextMeshProUGUI graphics = null;
        [SerializeField] private Button mainMenuButton = null;

        // Start is called before the first frame update
        void Start() {
            ConfigFile.SetUp();
            Language.SetUp();
            title.text = Language.GetString(GameDictionaryType.titles, "credits");
            mainDeveloper.text = Language.GetString(GameDictionaryType.credits, "developer");
            graphics.text = Language.GetString(GameDictionaryType.credits, "graphics");
            mainMenuButton.gameObject.GetComponentInChildren<Text>().text =
                Language.GetString(GameDictionaryType.buttons, "mainMenu");
        }

        public void MainMenuButton() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}