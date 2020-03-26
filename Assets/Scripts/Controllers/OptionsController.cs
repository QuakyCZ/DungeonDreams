using System.Collections.Generic;
using Models.Files;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers {
    public class OptionsController : MonoBehaviour {
        [SerializeField] private Toggle debugConsoleToggle;
        [SerializeField] private Toggle debugToggle;
        [SerializeField] private Toggle debugDialogueFileContentToggle;
        [SerializeField] private Toggle debugPathLinesToggle;
        
        [SerializeField] private Toggle openDoorsToggle;
        [SerializeField] private Toggle immuneToggle;
        
        private Dictionary<string, Toggle> _toggles;
        // Start is called before the first frame update
        void Start() {
            _toggles = new Dictionary<string, Toggle>();
            _toggles.Add("d_all", debugToggle);            
            _toggles.Add("d_console",debugConsoleToggle);
            _toggles.Add("d_dialogue_file_content",debugDialogueFileContentToggle);
            _toggles.Add("d_path_lines",debugPathLinesToggle);
            
            _toggles.Add("open_doors", openDoorsToggle);
            _toggles.Add("immune",immuneToggle);
            
            ConfigFile.SetUp();
            
            debugToggle.isOn = ConfigFile.Get().HasDebug("all");
            debugConsoleToggle.isOn = ConfigFile.Get().HasExactDebug("console");
            debugDialogueFileContentToggle.isOn = ConfigFile.Get().HasExactDebug("dialogue_file_content");
            debugPathLinesToggle.isOn = ConfigFile.Get().HasExactDebug("path_lines");
            
            openDoorsToggle.isOn = ConfigFile.Get().HasOption("open_doors");
            immuneToggle.isOn = ConfigFile.Get().HasOption("immune");
            
            
        }

        public void OnValueChanged(string option) {
            if (_toggles.ContainsKey(option) == false) {
                if (ConfigFile.Get().HasDebug("all")) {
                    Debug.Log("Toggle " + option + " is not in the dictionary!");
                }
            }
            else {
                var value = _toggles[option].isOn;
        
                ConfigFile.Get().SetValue(option,_toggles[option].isOn);
        
                ConfigFile.Reload();
                if (ConfigFile.Get().HasDebug("all")) {
                    Debug.Log("Toggle " + option + " " + value);
                }
            }
            
        }

        public void MainMenu() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}