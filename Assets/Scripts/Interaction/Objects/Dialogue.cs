using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue : Collectable
{
    [SerializeField] protected GameObject dialogGO;
    [SerializeField] protected List<string> dialogueList;
    [SerializeField] protected float typingSpeed = 0.02f;
    protected int index = 0;
    protected Text[] textFields;
    protected Text nameText;
    protected Text sentenceText;

    protected override void Start() {
        base.Start();
        textFields = dialogGO.GetComponentsInChildren<Text>(); // 0 - name, 1 - text
        nameText = textFields[0];
        sentenceText = textFields[1];
    }
    protected override void Update() {
        base.Update();

        if ( nameText.text + '>' + sentenceText.text == dialogueList[index] && Input.GetKeyDown( KeyCode.Space ) ) {
            NextSentence();
        }
    }
    protected override void OnCollect() {
        base.OnCollect();
        uiController.ToggleGUI( false );
        dialogGO.SetActive( true );
        string[] sentence = dialogueList[0].Split( '>' );
        StartCoroutine( Type(sentence[0], true) );
        StartCoroutine( Type( sentence[1], false ) );
    }

    protected void NextSentence() {
        if ( index < dialogueList.Count - 1 ) {
            index++;
            string[] sentence = dialogueList[index].Split( '>' );

            StartCoroutine( Type( sentence[0], true ) );
            StartCoroutine( Type( sentence[1], false ) );
        }
        else {
            dialogGO.SetActive( false );
            uiController.ToggleGUI( true );
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
    
}
