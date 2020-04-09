using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Console {
    public abstract class ConsoleCommand {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }

        public void AddCommandToConsole() { }

        public abstract void RunCommand();
    }

    public class DeveloperConsole : MonoBehaviour {
        [Header("UI")] public Canvas consoleCanvas;
        public ScrollRect scrollRect;
        [SerializeField] private Text consoleOutput = null;

        public static DeveloperConsole Instance { get; private set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; protected set; }

        private void Awake() {
            if (Instance != null) {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
            consoleCanvas.gameObject.SetActive(false);
            Log("Starting the console...");
            Application.logMessageReceived += HandleLog;
        }


        private void HandleLog(string logMessage, string stackTrace, LogType type) {
            string _message = $"[{type.ToString()}] {logMessage}";
            Log(_message);
        }

        private void CreateCommands() { }

        private void AddCommandToConsole(string _name, ConsoleCommand _command) {
            if (!Commands.ContainsKey(_name)) {
                Commands.Add(_name, _command);
            }
        }

        public void Log(string message) {
            if (consoleOutput != null) {
                consoleOutput.text += message + "\n";
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        public static void LogStatic(string message) {
            DeveloperConsole.Instance.consoleOutput.text += message + "\n";
            DeveloperConsole.Instance.scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}