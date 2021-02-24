using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OptionsController : MonoBehaviour{
    [Header("Title")] [SerializeField] private Text title = null;

    [Header("Debug Options")] [SerializeField]
    private Toggle debugConsoleToggle = null;

    [SerializeField] private Toggle debugToggle = null;
    [SerializeField] private Toggle debugDialogueFileContentToggle = null;
    [SerializeField] private Toggle debugPathLinesToggle = null;

    [Header("Other options")] [SerializeField]
    private Toggle openDoorsToggle = null;

    [SerializeField] private Toggle immuneToggle = null;

    [Header("Language")] [SerializeField] private Dropdown dropdown = null;

    [Header("Button")] [SerializeField] private Button mainMenuButton = null;

    private Dictionary<string, Toggle> _toggles;

    // Start is called before the first frame update
    void Start() {
        InitializeToggles();

        ConfigFile.SetUp();
        Language.SetUp();

        TranslateLanguage();
        ToggleOptions();

        List<string> options = new List<string>();
        options.Add(ConfigFile.Get().languages[Language.language]);
        foreach (var language in ConfigFile.Get().languages) {
            if (!options.Contains(language.Value))
                options.Add(language.Value);
        }

        dropdown.AddOptions(options);
    }

    private void InitializeToggles() {
        _toggles = new Dictionary<string, Toggle>();
        _toggles.Add("d_all", debugToggle);
        _toggles.Add("d_console", debugConsoleToggle);
        _toggles.Add("d_dialogue_file_content", debugDialogueFileContentToggle);
        _toggles.Add("d_path_lines", debugPathLinesToggle);

        _toggles.Add("open_doors", openDoorsToggle);
        _toggles.Add("immune", immuneToggle);
    }

    private void TranslateLanguage() {
        // Title
        title.text = Language.GetString(GameDictionaryType.titles, "options");

        // Toggles
        debugToggle.GetComponentInChildren<Text>().text = Language.GetString(GameDictionaryType.options, "d_all");
        debugConsoleToggle.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.options, "d_console");
        debugDialogueFileContentToggle.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.options, "d_dialogue_file_content");
        debugPathLinesToggle.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.options, "d_path_lines");
        openDoorsToggle.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.options, "open_doors");
        immuneToggle.GetComponentInChildren<Text>().text = Language.GetString(GameDictionaryType.options, "immune");

        // Button
        mainMenuButton.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "mainMenu");
    }

    private void ToggleOptions() {
        debugToggle.isOn = ConfigFile.Get().GetDebug("all");
        debugConsoleToggle.isOn = ConfigFile.Get().GetExactDebug("console");
        debugDialogueFileContentToggle.isOn = ConfigFile.Get().GetExactDebug("dialogue_file_content");
        debugPathLinesToggle.isOn = ConfigFile.Get().GetExactDebug("path_lines");
        openDoorsToggle.isOn = ConfigFile.Get().GetOption("open_doors");
        immuneToggle.isOn = ConfigFile.Get().GetOption("immune");
    }

    public void OnValueChanged(string option) {
        if (_toggles.ContainsKey(option) == false) {
            if (ConfigFile.Get().GetDebug("all")) {
                Debug.Log("Toggle " + option + " is not in the dictionary!");
            }
        }
        else {
            var value = _toggles[option].isOn;

            ConfigFile.Get().SetValue(option, _toggles[option].isOn);

            ConfigFile.Reload();
            if (ConfigFile.Get().GetDebug("all")) {
                Debug.Log("Toggle " + option + " " + value);
            }
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeLanguage(Dropdown change) {
        string value = change.captionText.text;
        string key = "";
        foreach (var language in ConfigFile.Get().languages) {
            if (language.Value == value) {
                key = language.Key;
                break;
            }
        }

        ConfigFile.Get().language = key;
        ConfigFile.Save();
        SceneManager.LoadScene("Options");
    }

    public void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}