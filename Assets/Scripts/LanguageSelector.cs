using System;
using System.Collections;
using System.Collections.Generic;
using Models.Files;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageSelector : MonoBehaviour
{
    public void Start() {
        ConfigFile.SetUp();
    }

    public void SelectLanguage(string language) {
        Debug.Log("Selected language: " + language);
        
        ConfigFile.Get().language = language;
        ConfigFile.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
