using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models.Files;
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
    private bool _opened = false;
    private ClonedTile tile;

    private static readonly int OpenDoorAnim = Animator.StringToHash("openDoorAnim");
    private static readonly int Opened = Animator.StringToHash("opened");

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if (ConfigFile.Get().HasOption("open_doors")) {
            locked = false;
        }

        tile = MainController.Instance.roomController.worldGraph
            .GetTileAt(
                (int) transform.position.x,
                (int) transform.position.y
                );
    }


    public void IsOpened(int opened = 0) {
        if (opened > 0) {
            Debug.Log( "Door are opened." );
            animator.SetInteger( OpenDoorAnim, 0 );
            animator.SetBool(Opened, true);
            _opened = true;
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            forbiddenArea.SetActive( false );
            tile.isWalkable = true;
        }
        else {
            Debug.Log( "Door are closed." );            
            GetComponent<SpriteRenderer>().sprite = closedSprite;
            animator.SetInteger( OpenDoorAnim, 0 );
            animator.SetBool( Opened, false );
            _opened = false;
            tile.isWalkable = false;
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
        if(_opened == false) {
            Debug.Log( "Opening the door." );
            animator.SetInteger( OpenDoorAnim, 1 );
        }
        else {
            Debug.Log( "Closing the door." );
            forbiddenArea.SetActive( true );
            animator.SetInteger( OpenDoorAnim, -1 );
        }
    }

    public bool IsLocked() {
        return locked;
    }
}
