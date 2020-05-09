using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour {
    [SerializeField] private Text loadingInfo = null;
    [SerializeField] private Button[] _buttons = null;
    public void Start() {
        try {
            ConfigFile.SetUp();
        }
        catch (Exception e) {
            loadingInfo.text = e.Message;
        }
    }

    public void SelectLanguage(string language) {
        loadingInfo.gameObject.SetActive(true);
        loadingInfo.text = "Saving language " + language;
        foreach (var btn in _buttons) {
            btn.interactable = false;
        }
        Debug.Log("Selected language: " + language);
        ConfigFile.Get().language = language;
        ConfigFile.Save();
        LoadScene();
    }

    public void LoadScene() {
        SceneManager.LoadScene("MainMenu");
    }
}
