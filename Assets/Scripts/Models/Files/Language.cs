using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public static class Language{
    private static GameDictionary dictionary;

    public static string language { get; private set; }
    // Start is called before the first frame update

    public static void SetUp() {
        Debug.Log("Language SetUp");
        string fileName = ConfigFile.Get().language + ".json";
        if (ConfigFile.Get().language.Equals("")) {
            Debug.LogWarning("Language was not specified. Loading en");
            fileName = "en.json";
        }

        string path = Application.streamingAssetsPath + "/" + fileName;
        if (File.Exists(path) == false) {
            var file = (TextAsset) Resources.Load(ConfigFile.Get().language);
            var content = file.text;
            File.WriteAllText(path,content);
        }
        string json = File.ReadAllText(path);
        Debug.Log(json);
        dictionary = (GameDictionary) JsonConvert.DeserializeObject(json, typeof(GameDictionary));
        language = dictionary.language;
        Debug.Log(dictionary.language);
    }

    public static string GetString(GameDictionaryType type, string key) {
        return dictionary.GetString(type, key);
    }
}

[Serializable]
public class GameDictionary{
    public string language;

    public Dictionary<string, Dictionary<string, string>> dics;

    public string GetString(GameDictionaryType type, string key) {
        //Debug.Log($"Get {key}");
        if (dics.ContainsKey(type.ToString()) && dics[type.ToString()].ContainsKey(key)) {
            return dics[type.ToString()][key];
        }

        Debug.LogWarning($"{type.ToString()} dictionary doesn't contain {key}.");
        return key;
    }
}