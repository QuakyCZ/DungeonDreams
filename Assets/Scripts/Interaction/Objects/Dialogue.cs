using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum Action { None, Award, ToggleDoor, SpawnEnemies}
public enum DialogueCharacter{Dabel, Drak}
[System.Serializable]public class DialogueCharacterImages : SerializableDictionaryBase<DialogueCharacter,Sprite>{}
public class Dialogue : Collectable {

    [Header("Dialogue settings")]
    [SerializeField] protected GameObject dialogGO;
    [SerializeField] protected float typingSpeed = 0.02f;
    [Tooltip("Has to be same as file name.")]
    [SerializeField] protected string dialogueName;
    [SerializeField] protected Image secondCharacterImg;
    [SerializeField] protected DialogueCharacterImages dialogueCharacterImages;
    [SerializeField] protected Vector2 spawnPosition;

    protected int index = 0;
    protected Text[] textFields;
    protected Text nameText;
    protected Text sentenceText;
    protected Queue<string> actionsToPrepare;
    protected Queue<Action> actionsToDo;
    protected List<string> dialogueList;
    [SerializeField] EnemyPrefabs enemyPrefabs;
    protected List<GameObject> enemiesToSpawn;
    protected UnityEngine.Object file;
    protected DialogueCharacter secondCharacter;

    protected override void Start() {
        base.Start();
        //UnityEngine.Object[] res = Resources.LoadAll( "Dialogues" );
        //Debug.Log( res.Length );
        file = Resources.Load( "Dialogues/"+dialogueName );
        if ( file == null ) {
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
        enemiesToSpawn = new List<GameObject>();
        actionsToPrepare = new Queue<string>();
        actionsToDo = new Queue<Action>();
        textFields = dialogGO.GetComponentsInChildren<Text>(); // 0 - name, 1 - text
        nameText = textFields[0];
        sentenceText = textFields[1];
        dialogueList = new List<string>();
    }

   

    protected override void Update() {
        base.Update();

        if ( nameText.text + '>' + sentenceText.text == dialogueList[index] && Input.GetKeyDown( KeyCode.Space ) ) {
            typingSpeed = 0.02f;
            NextSentence();

        }
        else if ( nameText.text + '>' + sentenceText.text != dialogueList[index] && Input.GetKeyDown( KeyCode.Space ) ) {
            typingSpeed = 0;
        }
    }

    protected override void OnCollect() {
        base.OnCollect();
        FindObjectOfType<MainController>().PauseGameTime();

        uiController.ToggleGUI( false );
        dialogGO.SetActive( true );

        string[] sentence = dialogueList[0].Split( '>' );
        secondCharacterImg.sprite = dialogueCharacterImages[secondCharacter];
        StartCoroutine( Type( sentence[0], true ) );
        StartCoroutine( Type( sentence[1], false ) );
        
        
    }

    protected void ReadFile() {
        if ( file == null ) {
            Debug.LogError( "File doesn't exist." );
            return;
        }

        Debug.Log( file.ToString() );
        string[] lines = file.ToString().Split( '\r','\n' );
        for  ( int i = 0; i<lines.Length-1; i++) {
            string line = lines[i];
            if(line.StartsWith("/",StringComparison.Ordinal)){
                secondCharacter = (DialogueCharacter)Enum.Parse(typeof(DialogueCharacter),line.Substring(1));
            }
            else if ( line != "" && !line.Contains( "$" ))
                dialogueList.Add( line );
            else if ( line.StartsWith( "$", StringComparison.Ordinal ) ) {
                actionsToPrepare.Enqueue( line.Substring( 1 ) );
            }
        }
    }



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

    ///////////////////////////// 
    /// 
    /// Actions after dialogue
    /// 
    //////////////////////////////

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
        }

    }

    protected void DoAction(Action action) {
        switch ( action ) {
            case Action.SpawnEnemies:
                ShowEnemies();
                break;
            default:
                Debug.LogWarning( "This action was not implemented." );
                break;
        }
    }

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

    protected void ShowEnemies() {
        foreach(GameObject enemyGO in enemiesToSpawn ) {
            if(enemyGO != null)
                enemyGO.SetActive( true );
        }
    }

    protected void OnDrawGizmosSelected(){
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(spawnPosition,1);
    }
}
