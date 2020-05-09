using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class VersionController : MonoBehaviour{
    [Header("General")] [SerializeField] private Text versionText = null;

    [SerializeField] private Button updateButton = null;

    [Header("Update Panel")] [SerializeField]
    private GameObject updatePanel = null;

    [SerializeField] private Text updateTitleText = null;
    [SerializeField] private Text newVersionText = null;
    [SerializeField] private Text updateButtonText = null;
    [SerializeField] private Text skipButtonText = null;

    private Version _version;

    void Start() {
        versionText.text = $"{Language.GetString(GameDictionaryType.titles, "version")}: {ConfigFile.Get().version}";

        StartCoroutine(CheckUpdate());
    }

    IEnumerator CheckUpdate() {
        UnityWebRequest webRequest = UnityWebRequest.Get(ConfigFile.Get().versionUrl);
        yield return webRequest.SendWebRequest();

        Debug.Log(webRequest.downloadHandler.text);
        _version = (Version) JsonConvert.DeserializeObject(webRequest.downloadHandler.text, typeof(Version));
        string newestVersion = _version.version;
        Debug.Log(newestVersion);
        if (!ConfigFile.Get().version.Equals(newestVersion) && !Application.version.Contains("dev")) {
            //if (ConfigFile.Get().version.Contains("Dev")) yield break;

            Tranlate();

            updatePanel.SetActive(true);
        }
    }

    private void Tranlate() {
        versionText.text += $" ({Language.GetString(GameDictionaryType.titles, "outdated")})";

        // Update Panel Language
        updateTitleText.text = Language.GetString(GameDictionaryType.titles, "newVersionAvailable"); // Title
        newVersionText.text = $"{ConfigFile.Get().version} > {_version.version}"; // Version
        updateButtonText.text = Language.GetString(GameDictionaryType.buttons, "update"); // Update Button
        skipButtonText.text = Language.GetString(GameDictionaryType.buttons, "skip"); // Skip Button
    }

    public void UpdateGame() {
        Application.OpenURL(_version.devlogurl);
    }

    public void CloseUpdatePanel() {
        updatePanel.SetActive(false);
        updateButton.GetComponentInChildren<Text>().text = Language.GetString(GameDictionaryType.buttons, "update");
        updateButton.gameObject.SetActive(true);
    }
}

public class Version{
    public string version;
    public string devlogurl;
}