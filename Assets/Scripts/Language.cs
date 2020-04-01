using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using BayatGames.Serialization.Formatters.Json;
using Models.Files;

public enum GameDictionaryType {
    buttons,
    credits,
    titles,
    controls
}


public class Language {
    private static GameDictionary dictionary;
    // Start is called before the first frame update
    
    public static void SetUp() {
        Debug.Log("Language SetUp");
        string fileName = ConfigFile.Get().language + ".json";
        string path = Application.streamingAssetsPath + "/" + fileName;
        string json = File.ReadAllText(path);
        Debug.Log(json);
        dictionary = (GameDictionary)JsonFormatter.DeserializeObject(json,typeof(GameDictionary));
        Debug.Log(dictionary.language);
    }

    public static string GetString(GameDictionaryType type ,string key) {
        return dictionary.GetString(type,key);
    }
}

[Serializable]
public class GameDictionary {
    public string language;
    
    public Dictionary<string, Dictionary<string,string>>dics;
    public string GetString(GameDictionaryType type, string key) {

        Debug.Log($"Get {key}");
        if (dics.ContainsKey(type.ToString())&&dics[type.ToString()].ContainsKey(key)) {
            return dics[type.ToString()][key];
        }
        Debug.LogWarning($"{type.ToString()} dictionary doesn't contain {key}.");
        return key;
    }
}
