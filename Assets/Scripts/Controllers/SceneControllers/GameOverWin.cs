﻿using UnityEngine;
using UnityEngine.UI;

public class GameOverWin : SceneController{
    [SerializeField] private Text content = null;
    [SerializeField] private Button mainMenuButton = null;

    protected override void Translate() {
        base.Translate();
        content.text = Language.GetString(GameDictionaryType.other, "gameOverWinContent");
        mainMenuButton.GetComponentInChildren<Text>().text =
            Language.GetString(GameDictionaryType.buttons, "mainMenu");
    }
}