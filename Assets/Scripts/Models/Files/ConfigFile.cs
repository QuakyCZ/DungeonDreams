using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using YamlDotNet;
using YamlDotNet.RepresentationModel;

namespace Models.Files {
    public class ConfigFile {
        private static string configJson;
        private static Config config;
        private static string path;
        public static Config SetUp() {
            path = Application.streamingAssetsPath + "/config.json";
            configJson = File.ReadAllText(path);
            config = JsonUtility.FromJson<Config>(configJson);
            if (config.HasDebug("all")) {
                foreach (var debug in config.debug) {
                    Debug.Log(debug);
                }

                foreach (var option in config.options) {
                    Debug.Log(option);
                }
            }

            return config;
        }

        public static Config Get() {
            return config;
        }

        /// <summary>
        /// Saves and reloads the file.
        /// </summary>
        public static void Save() {
            configJson = JsonUtility.ToJson(config);
            Debug.Log("ConfigFile:Save(): " + configJson);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(configJson);
            sw.Close();
            Reload();
        }

        public static void Reload() {
            configJson = File.ReadAllText(path);
        }
    }

    [Serializable]
    public class Config {
        /// <summary>
        /// Do not use this
        /// </summary>
        public List<string> debug;

        /// <summary>
        /// Do not use this
        /// </summary>
        public List<string> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Write d_key for add to debug.</param>
        /// <param name="value"></param>
        public void SetValue(string key, bool value) {
            Debug.Log("Config set " + key + " " + value);
            key = key.ToLower();
            if (key.StartsWith("d_")) {
                Debug.Log("Is debug.");
                SetDebugOption(key, value);
            }
            else {
                Debug.Log("Is not debug");
                SetOption(key, value);
            }

            ConfigFile.Save();
        }

        private void SetOption(string key, bool value) {
            Debug.Log($"Config:SetOption({key},{value})");
            key = key.ToLower();

            switch (value) {
                case true: {
                    if (HasOption(key) == false) {
                        options.Add(key);
                    }

                    break;
                }
                case false: {
                    if (HasOption(key) == true) {
                        options.Remove(key);
                    }

                    break;
                }
            }
        }

        public bool HasOption(string key) {
            return options.Contains(key);
        }

        /// <summary>
        /// Sets the value in debug
        /// </summary>
        /// <param name="key">The name of the debug mod</param>
        /// <param name="value">true - add, false - remove</param>
        private void SetDebugOption(string key, bool value) {
            key = key.ToLower();
            if (key.StartsWith("d_")) {
                key = key.Replace("d_", "");
            }

            if (value) {
                if (!HasExactDebug(key)) {
                    debug.Add(key);
                }
            }
            else {
                if (HasExactDebug(key)) {
                    debug.Remove(key);
                }
            }
        }

        public bool HasDebug(string name) {
            if (debug.Contains("all")) {
                return true;
            }

            name = name.ToLower();
            if (name.StartsWith("d_")) {
                name = name.Replace("d_", "");
            }

            return debug.Contains(name);
        }

        public bool HasExactDebug(string name) {
            name = name.ToLower();
            if (name.StartsWith("d_")) {
                name = name.Replace("d_", "");
            }

            return debug.Contains(name);
        }
    }
}