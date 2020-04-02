using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Interaction.Objects;
using Models.Files;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum Action {
    None,
    Award,
    ToggleDoor,
    SpawnEnemies,
    GameOverWin,
    GameOverLose
}

public class Dialogue : Collectable {
    [Header("UI")]
    [SerializeField] private GameObject dialogGo = null;
    [SerializeField] private float typingSpeed = 0.02f; // the typing delay between chars
    [SerializeField] private Text continueText = null;
    
    [Header("Settings")]
    [Tooltip("Has to be same as file name without suffix.")] 
    [SerializeField] private string dialogueName = null; // the name of the dialogue file without suffix
    [SerializeField] protected Vector2 spawnPosition;
    [SerializeField] EnemyPrefabs enemyPrefabs = null;

    private UnityEngine.Object _file; // current dialogue file 
    private List<string> _dialogueList; // the list of sentences
    private int _index = 0; // index of current dialogue sentence
    
    // Texts
    private Text[] _textFields;
    private Text _nameText;
    private Text _sentenceText;
    
    // Actions
    private List<GameObject> _enemiesToSpawn;
    private Queue<string> _actionsToPrepare; // the queue of actions that need to be prepared
    private Queue<Action> _actionsToDo; // the queue of actions that should be committed

    protected override void Start() {
        base.Start();
        continueText.text = Language.GetString(GameDictionaryType.titles, "continue") + ":\n" +
                            Language.GetString(GameDictionaryType.controls, "dialogue_continue");
        string language = Language.language;
        _file = Resources.Load("Dialogues/" + language + "/" + dialogueName);
        if (_file == null) {
            Debug.LogError($"{gameObject.name}: Dialog file {dialogueName}.txt doesn't exist!");
            Destroy(this.gameObject);
            return;
        }

        InstantiateAttributes();

        ReadFile();
        while (_actionsToPrepare.Count > 0) {
            PrepareAction(_actionsToPrepare.Dequeue());
        }
    }

    private void InstantiateAttributes() {
        _enemiesToSpawn = new List<GameObject>();
        _actionsToPrepare = new Queue<string>();
        _actionsToDo = new Queue<Action>();
        _textFields = dialogGo.GetComponentsInChildren<Text>(); // 0 - name, 1 - text
        _nameText = _textFields[0];
        _sentenceText = _textFields[1];
        _dialogueList = new List<string>();
    }


    protected override void Update() {
        base.Update();
        // If the sentence was written and player pressed space.
        if (_nameText.text + '>' + _sentenceText.text == _dialogueList[_index] && Input.GetKeyDown(KeyCode.Space)) {
            typingSpeed = 0.02f;
            NextSentence();
        }
        // If the sentence was not written yet and player pressed space.
        else if (_nameText.text + '>' + _sentenceText.text != _dialogueList[_index] &&
                 Input.GetKeyDown(KeyCode.Space)) {
            typingSpeed = 0;
        }
    }

    protected override void OnCollect() {
        base.OnCollect();
        Door door = GetComponent<Door>();
        if (door != null) {
            if (door.IsLocked()) {
                return;
            }
            else {
                ShowDialog();
            }
        }
        else {
            ShowDialog();
        }
    }

    private void ShowDialog() {
        FindObjectOfType<MainController>().PauseGameTime();

        uiController.ToggleGUI(false);
        dialogGo.SetActive(true);

        string[] sentence = _dialogueList[0].Split('>');
        StartCoroutine(Type(sentence[0], true));
        StartCoroutine(Type(sentence[1], false));
    }

    /// <summary>
    /// Reads the file.
    /// </summary>
    private void ReadFile() {
        Debug.Log($"Reading file {dialogueName}.txt");
        if (_file == null) {
            Debug.LogError("File doesn't exist.");
            return;
        }

        if (ConfigFile.Get().GetDebug("dialogue_file_content"))
            Debug.Log(_file.ToString());

        string[] lines = _file.ToString().Split('\r', '\n');
        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i];
            if (line != "" && line.StartsWith("$", StringComparison.Ordinal) == false) {
                _dialogueList.Add(line);
                if (ConfigFile.Get().GetDebug("dialogue_file_content"))
                    Debug.Log($"{dialogueName}: Dialog line found: {line}");
            }
            else if (line.StartsWith("$", StringComparison.Ordinal)) {
                _actionsToPrepare.Enqueue(line.Substring(1));
                if (ConfigFile.Get().GetDebug("dialogue_file_content"))
                    Debug.Log($"{dialogueName}: Action found: {line}");
            }
        }
    }

    /// <summary>
    /// Starts typing new sentence.
    /// </summary>
    private void NextSentence() {
        if (_index < _dialogueList.Count - 1) {
            _index++;

            string[] sentence = _dialogueList[_index].Split('>');
            StartCoroutine(Type(sentence[0], true));
            StartCoroutine(Type(sentence[1], false));
        }
        else {
            // There are not other lines -> close the dialog.
            dialogGo.SetActive(false);
            uiController.ToggleGUI(true);
            FindObjectOfType<MainController>().PauseGameTime(false);
            foreach (Action action in _actionsToDo) {
                DoAction(action);
            }
        }
    }

    /// <summary>
    /// Starts typing the text.
    /// If the given text is name, use true.
    /// </summary>
    /// <param name="text">Text to type</param>
    /// <param name="name">Whether text is name or not</param>
    /// <returns></returns>
    private IEnumerator Type(string text, bool name) {
        Text field;

        if (name) {
            field = _nameText;
        }
        else {
            field = _sentenceText;
        }

        field.text = "";
        foreach (char letter in text) {
            field.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    /// Actions after dialogue
    /// <summary>
    /// Prepares the action into the actionsToDo queue.
    /// </summary>
    /// <param name="actionSentence">The string with action</param>
    private void PrepareAction(string actionSentence) {
        string[] actionString = actionSentence.Split('>'); // 0 - action, 1 - itemtype, 2 - count
        Action action = (Action) Enum.Parse(typeof(Action), actionString[0]);
        switch (action) {
            case Action.ToggleDoor:
                _actionsToDo.Enqueue(Action.ToggleDoor);
                Debug.Log("ToggleDoor. This is not implemented yet.");
                break;
            case Action.Award:
                _actionsToDo.Enqueue(Action.Award);
                Debug.Log("Award - " + actionString[1] + " " + actionString[2] + "x");
                break;
            case Action.SpawnEnemies:
                _actionsToDo.Enqueue(Action.SpawnEnemies);
                EnemyType type = (EnemyType) Enum.Parse(typeof(EnemyType), actionString[1]);
                int spawnCount = Int32.Parse(actionString[2]);
                Debug.Log("Spawn " + type + " " + spawnCount + "x");
                InstantiateEnemy(type, spawnCount);
                break;
            default:
                _actionsToDo.Enqueue(action);
                break;
        }
    }

    /// <summary>
    /// Does given action.
    /// </summary>
    /// <param name="action"></param>
    private void DoAction(Action action) {
        switch (action) {
            case Action.SpawnEnemies:
                ShowEnemies();
                break;
            case Action.GameOverWin:
                SceneManager.LoadScene("GameOverWin");
                break;
            case Action.GameOverLose:
                SceneManager.LoadScene("GameOverLose");
                break;
            default:
                Debug.LogWarning($"Action {action.ToString()} is not implemented!");
                break;
        }
    }

    /// <summary>
    /// Instantiates enemy on start as inactive. Use void ShowEnemies to show them.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    private void InstantiateEnemy(EnemyType type, int count = 1) {
        for (int i = 0; i < count; i++) {
            if (enemyPrefabs.ContainsKey(type)) {
                GameObject enemyGO = Instantiate(enemyPrefabs[type]);
                _enemiesToSpawn.Add(enemyGO);
                enemyGO.transform.SetParent(transform);
                enemyGO.transform.position = spawnPosition;
                enemyGO.SetActive(false);
            }
            else {
                Debug.LogWarning("InstantiateEnemy: " + type + " is not in dictionary");
            }
        }
    }

    /// <summary>
    /// Shows instantiated enemies.
    /// </summary>
    private void ShowEnemies() {
        foreach (GameObject enemyGO in _enemiesToSpawn) {
            if (enemyGO != null)
                enemyGO.SetActive(true);
        }
    }

    /// <summary>
    /// This is just for graphic debug.
    /// </summary>
    protected void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPosition, 1);
    }
}