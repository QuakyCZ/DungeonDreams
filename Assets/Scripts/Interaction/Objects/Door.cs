using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Collectable
{
    [Header("Door Security")]
    [SerializeField] protected bool locked = false;
    [SerializeField] protected int keyParts;
    [Header("GFX")]
    [SerializeField] protected Sprite openedSprite;
    [SerializeField] protected Sprite closedSprite;
    [SerializeField] protected GameObject forbiddenArea;
    protected Animator animator;
    bool oppened = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }


    public void IsOpened(int oppened = 0) {
        if (oppened > 0) {
            Debug.Log( "Door are oppened." );
            animator.SetInteger( "openDoorAnim", 0 );
            animator.SetBool( "oppened", true );
            this.oppened = true;
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            forbiddenArea.SetActive( false );
        }
        else {
            Debug.Log( "Door are closed." );            
            GetComponent<SpriteRenderer>().sprite = closedSprite;
            animator.SetInteger( "openDoorAnim", 0 );
            animator.SetBool( "oppened", false );
            this.oppened = false;           
        }
    }

    protected override void OnCollect() {
        if (locked) {
            if (player.inventory.GetValue( InventoryDefault.key ) < keyParts) {
                uiController.Log( $"Dveře jsou zamčeny. Nejdříve najdi {keyParts} částí klíče." );
                return;
            }
            else {
                locked = false;
                OnCollect();
            }
        }
        if(oppened == false) {
            Debug.Log( "Oppening the door." );
            animator.SetInteger( "openDoorAnim", 1 );
        }
        else {
            Debug.Log( "Closing the door." );
            forbiddenArea.SetActive( true );
            animator.SetInteger( "openDoorAnim", -1 );
        }
    }

    public bool IsLocked() {
        return locked;
    }
}
