using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LevelManager {
    private static Dictionary<int, bool> achievedLevels;
    private static bool initialized = false;
    
    public static void Init() {
        if (!initialized) {
            Debug.Log("Initializing Level Manager");
            LoadData();
            initialized = true;
        }
    }

    /// <summary>
    /// This will be used just to store data while not playing.
    /// </summary>
    private static void SaveJson() {
        string json = JsonConvert.SerializeObject(achievedLevels);
        Debug.Log(json);
        PlayerPrefs.SetString("achieved-levels", json);
    }

    private static void LoadData() {
        string json = PlayerPrefs.GetString("achieved-levels", "null");
        Debug.Log(json);
        if (json.Equals("null")) {
            achievedLevels = new Dictionary<int, bool> {{0, false}, {1, false}};
            SaveJson();
        }
        else {
            achievedLevels = (Dictionary<int, bool>) JsonConvert.DeserializeObject(json, typeof(Dictionary<int, bool>));
        }
    }

    public static void CompleteLevel(int level) {
        if(level < 0)
            return;
        if (achievedLevels[level] != true) {
            achievedLevels[level] = true; 
            SaveJson();
        }
    }
    
    public static List<int> GetUnlockedLevels() {
        List<int> unlocked = new List<int>();
        foreach (int x in achievedLevels.Keys)
            if (achievedLevels[x] == true)
                unlocked.Add(x);
        return unlocked;
    }
}