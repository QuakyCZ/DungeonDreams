using System;
using System.Collections;
using System.Collections.Generic;
using Models.Files;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour {
    [SerializeField] private Text text = null; 
    public void Start() {
        try {
            ConfigFile.SetUp();
        }
        catch (Exception e) {
            text.text = e.Message;
        }
    }

    public void SelectLanguage(string language) {
        Debug.Log("Selected language: " + language);
        
        ConfigFile.Get().language = language;
        ConfigFile.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
