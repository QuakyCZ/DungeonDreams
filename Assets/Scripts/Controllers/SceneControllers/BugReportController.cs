﻿using Models.Files;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.SceneControllers {
    public class BugReportController : SceneController {
        [SerializeField] private Button sendButton = null;
        [SerializeField] private Button mainMenuButton = null;
        protected override void Translate() {
            base.Translate();
            mainMenuButton.GetComponentInChildren<Text>().text = Language.GetString(
                GameDictionaryType.buttons, 
                "mainMenu"
                );
            sendButton.GetComponentInChildren<Text>().text = Language.GetString(
                GameDictionaryType.buttons,
                "send"
                );
        }
    }
}
