using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CreditsController : MonoBehaviour{
    [SerializeField] private Text title = null;
    [SerializeField] private Text mainDeveloperText = null;
    [SerializeField] private Text graphicsText = null;
    [SerializeField] private Text soundText = null;
    [SerializeField] private Button mainMenuButton = null;

    // Start is called before the first frame update
    void Start() {
        ConfigFile.SetUp();
        Language.SetUp();
        title.text = Language.GetString(GameDictionaryType.titles, "credits");
        mainDeveloperText.text = Language.GetString(GameDictionaryType.credits, "developer");
        graphicsText.text = Language.GetString(GameDictionaryType.credits, "graphics");
        soundText.text = Language.GetString(GameDictionaryType.credits, "sound");
        mainMenuButton.gameObject.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "mainMenu");
    }

    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }
}