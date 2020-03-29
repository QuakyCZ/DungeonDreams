using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models.Files;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Action { 
    None, 
    Award, 
    ToggleDoor,
    SpawnEnemies,
    GameOverWin,
    GameOverLose
}

public class Dialogue : Collectable {
    
    [Header("Dialogue settings")]
    #region UI
    [SerializeField] protected GameObject dialogGO;
    protected Text[] textFields;
    protected Text nameText;
    protected Text sentenceText;
    #endregion
    #region dialogue settings
    [SerializeField] protected float typingSpeed = 0.02f; // the typing delay between chars 
    [Tooltip("Has to be same as file name.")]
    [SerializeField] protected string dialogueName; // the name of the dialogue file without suffix
    #endregion
    #region spawning characters and enemies
    [SerializeField] EnemyPrefabs enemyPrefabs = null;
    protected List<GameObject> enemiesToSpawn;
    [SerializeField] protected Vector2 spawnPosition;
    #endregion

    protected int index = 0; // index of current dialogue sentence

    protected UnityEngine.Object file; // current dialogue file 

    protected List<string> dialogueList; // the list of sentences

    protected Queue<string> actionsToPrepare; // the queue of actions that need to be prepared
    protected Queue<Action> actionsToDo; // the queue of actions that should be commited

    protected override void Start() {
        base.Start();
        file = Resources.Load( "Dialogues/"+dialogueName );
        if ( file == null ) {
            Debug.LogError( $"{gameObject.name}: Dialog file {dialogueName}.txt doesn't exist!" );
            Destroy( this.gameObject );            
            return;
        }
        InstantiateAttributes();

        ReadFile();
        while ( actionsToPrepare.Count > 0 ) {
            PrepareAction( actionsToPrepare.Dequeue() );
        }
    }

    protected void InstantiateAttributes() {
        enemiesToSpawn      = new List<GameObject>();
        actionsToPrepare    = new Queue<string>();
        actionsToDo         = new Queue<Action>();
        textFields          = dialogGO.GetComponentsInChildren<Text>(); // 0 - name, 1 - text
        nameText            = textFields[0];
        sentenceText        = textFields[1];
        dialogueList        = new List<string>();
    }

   

    protected override void Update() {
        base.Update();
        // If the sentence was written and player pressed space.
        if ( nameText.text + '>' + sentenceText.text == dialogueList[index] && Input.GetKeyDown( KeyCode.Space ) ) {
            typingSpeed = 0.02f;
            NextSentence();

        }
        // If the sentence was not written yet and player pressed space.
        else if ( nameText.text + '>' + sentenceText.text != dialogueList[index] && Input.GetKeyDown( KeyCode.Space ) ) {
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
    protected void ShowDialog() {
        FindObjectOfType<MainController>().PauseGameTime();

        uiController.ToggleGUI( false );
        dialogGO.SetActive( true );

        string[] sentence = dialogueList[0].Split( '>' );
        StartCoroutine( Type( sentence[0], true ) );
        StartCoroutine( Type( sentence[1], false ) );
    }
    /// <summary>
    /// Reads the file.
    /// </summary>
    protected void ReadFile() {
        Debug.Log( $"Reading file {dialogueName}.txt" );
        if ( file == null ) {
            Debug.LogError( "File doesn't exist." );
            return;
        }
        if(ConfigFile.Get().HasDebug("dialogue_file_content"))
            Debug.Log( file.ToString() );
        
        string[] lines = file.ToString().Split( '\r','\n' );
        for  ( int i = 0; i<lines.Length; i++) {
            string line = lines[i];
            if (line != "" && line.StartsWith( "$", StringComparison.Ordinal )==false) {
                dialogueList.Add( line );
                if(ConfigFile.Get().HasDebug("dialogue_file_content"))
                    Debug.Log( $"{dialogueName}: Dialog line found: {line}" );
            }
            else if (line.StartsWith( "$", StringComparison.Ordinal )) {
                actionsToPrepare.Enqueue( line.Substring( 1 ) );
                if(ConfigFile.Get().HasDebug("dialogue_file_content"))
                    Debug.Log( $"{dialogueName}: Action found: {line}" );
            }

        }
    } 

    /// <summary>
    /// Starts typing new sentence.
    /// </summary>
    protected void NextSentence() {
        if ( index < dialogueList.Count - 1 ) {
            index++;
           
            string[] sentence = dialogueList[index].Split( '>' );
            StartCoroutine( Type( sentence[0], true ) );
            StartCoroutine( Type( sentence[1], false ) );

        }
        else {
            // There are not other lines -> close the dialog.
            dialogGO.SetActive( false );
            uiController.ToggleGUI( true );
            FindObjectOfType<MainController>().PauseGameTime( false );
            foreach(Action action in actionsToDo ) {
                DoAction( action );
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
    protected IEnumerator Type(string text, bool name) {
        Text field;

        if ( name ) {
            field = nameText;
        }
        else {
            field = sentenceText;
        }
        field.text = "";
        foreach(char letter in text ) {
            field.text += letter;
            yield return new WaitForSeconds( typingSpeed );
        }
    }

    /// Actions after dialogue

    /// <summary>
    /// Prepares the action into the actionsToDo queue.
    /// </summary>
    /// <param name="actionSentence">The string with action</param>
    protected void PrepareAction(string actionSentence) {
        string[] actionString = actionSentence.Split( '>' ); // 0 - action, 1 - itemtype, 2 - count
        Action action = ( Action )Enum.Parse( typeof( Action ), actionString[0] );
        switch ( action ) {
            case Action.ToggleDoor:
                actionsToDo.Enqueue( Action.ToggleDoor );
                Debug.Log( "ToggleDoor. This is not implemented yet." );
                break;
            case Action.Award:
                actionsToDo.Enqueue( Action.Award );
                Debug.Log( "Award - " + actionString[1] + " " + actionString[2] + "x" );
                break;
            case Action.SpawnEnemies:
                actionsToDo.Enqueue( Action.SpawnEnemies );
                EnemyType type = ( EnemyType )Enum.Parse( typeof( EnemyType ), actionString[1] );
                int spawnCount = Int32.Parse(actionString[2]);
                Debug.Log( "Spawn " + type + " " + spawnCount + "x" );
                InstantiateEnemy( type, spawnCount );
                break;
            default:
                actionsToDo.Enqueue( action );
                break;
        }
    }

    /// <summary>
    /// Does given action.
    /// </summary>
    /// <param name="action"></param>
    protected void DoAction(Action action) {
        switch ( action ) {
            case Action.SpawnEnemies:
                ShowEnemies();
                break;
            case Action.GameOverWin:
                SceneManager.LoadScene( "GameOverWin" );
                break;
            case Action.GameOverLose:
                SceneManager.LoadScene( "GameOverLose" );
                break;
            default:
                Debug.LogWarning( $"Action {action.ToString()} is not implemented!" );
                break;
        }
    }

    /// <summary>
    /// Instantiates enemy on start as inactive. Use void ShowEnemies to show them.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    protected void InstantiateEnemy( EnemyType type, int count = 1 ) {
        for(int i = 0; i<count; i++ ) {
            if ( enemyPrefabs.ContainsKey( type ) ) {
                GameObject enemyGO = Instantiate( enemyPrefabs[type] );
                enemiesToSpawn.Add( enemyGO );
                enemyGO.transform.SetParent( transform );
                enemyGO.transform.position = spawnPosition;
                enemyGO.SetActive( false );
            }
            else {
                Debug.LogWarning( "InstantiateEnemy: " + type + " is not in dictionary" );
            }
        }

    }

    /// <summary>
    /// Shows instantiated enemies.
    /// </summary>
    protected void ShowEnemies() {
        foreach(GameObject enemyGO in enemiesToSpawn ) {
            if(enemyGO != null)
                enemyGO.SetActive( true );
        }
    }

    /// <summary>
    /// This is just for graphic debug.
    /// </summary>
    protected void OnDrawGizmosSelected(){
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(spawnPosition,1);
    }
}
