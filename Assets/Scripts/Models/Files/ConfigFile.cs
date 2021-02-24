using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public static class ConfigFile{
    private static Config config;
    private static string configJson;
    private static string path;

    public static Config SetUp() {
        try {
            if (Directory.Exists(Application.streamingAssetsPath) == false) {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            path = Application.streamingAssetsPath + "/config.json";
            if (File.Exists(path) == false) {
                var file = (TextAsset) Resources.Load("config");
                var content = file.text;
                File.WriteAllText(path,content);
            }
            configJson = File.ReadAllText(path);
            config = (Config) JsonConvert.DeserializeObject(configJson, typeof(Config));
            config.version = Application.version;
            //Debug.Log(JsonFormatter.SerializeObject(config));
            Debug.Log("Selected language: " + config.language);
            if (config.GetDebug("all")) {
                foreach (var debug in config.debug) {
                    Debug.Log(debug.Key);
                }

                foreach (var option in config.options) {
                    Debug.Log(option.Key);
                }

                foreach (var language in config.languages) {
                    Debug.Log(language.Key);
                }
            }

            Save();
            return config;
        }
        catch (Exception e) {
            throw e;
        }
    }

    public static Config Get() {
        return config;
    }

    /// <summary>
    /// Saves and reloads the file.
    /// </summary>
    public static void Save() {
        JsonSerializerSettings settings = new JsonSerializerSettings {Formatting = Formatting.Indented};
        configJson = JsonConvert.SerializeObject(config, settings);
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
public class Config{
    public string version;
    public string versionUrl;
    public string language;
    public int healPotion;

    public Dictionary<string, string> languages;
    public Dictionary<string, bool> debug;
    public Dictionary<string, bool> options;

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
        if (HasOption(key)) {
            options[key] = value;
        }
        else {
            Debug.LogWarning("Options doesn't contain " + key);
        }
    }

    public bool GetOption(string key) {
        if (HasOption(key)) {
            return options[key];
        }

        Debug.LogWarning("Options doesn't contain " + key);
        return false;
    }

    public bool HasOption(string key) {
        if (options.ContainsKey(key)) {
            return true;
        }

        return false;
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

        if (debug.ContainsKey(key)) {
            debug[key] = value;
        }
        else {
            Debug.LogWarning("Debug doesn't contain " + key);
        }
    }

    public bool GetDebug(string name) {
        if (debug["all"] == true) {
            return true;
        }

        name = name.ToLower();
        if (name.StartsWith("d_")) {
            name = name.Replace("d_", "");
        }

        if (HasDebug(name))
            return debug[name];

        Debug.Log("Debug doesn't contain " + name);
        return false;
    }

    public bool GetExactDebug(string name) {
        name = name.ToLower();
        if (name.StartsWith("d_")) {
            name = name.Replace("d_", "");
        }

        if (HasDebug(name))
            return debug[name];
        Debug.Log("Debug doesn't contain " + name);
        return false;
    }

    public bool HasDebug(string key) {
        if (debug.ContainsKey(key)) {
            return true;
        }

        return false;
    }
}