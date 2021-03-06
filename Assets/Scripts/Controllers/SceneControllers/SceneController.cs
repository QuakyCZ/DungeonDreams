﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour{
    [SerializeField] private Text title = null;
    [SerializeField] private string titleName;

    protected virtual void Start() {
        ConfigFile.SetUp();
        Language.SetUp();
        Translate();
    }

    protected virtual void Translate() {
        title.text = Language.GetString(GameDictionaryType.titles, titleName);
    }

    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }
}