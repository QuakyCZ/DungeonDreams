using Controllers;
using Models.Files;
using Models.Inventory;
using UnityEngine;

namespace Interaction.Objects {
    public class Door : Collectable
    {
        [Header("Door Security")]
        [SerializeField] protected bool locked = false;
        [SerializeField] protected int keyParts;
        
        [Header("GFX")]
        [SerializeField] protected Sprite openedSprite = null;
        [SerializeField] protected Sprite closedSprite = null;
        [SerializeField] protected GameObject forbiddenArea = null;
        [SerializeField] private GameObject lockGameObject = null;
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
            if (ConfigFile.Get().GetOption("open_doors")) {
                locked = false;
            }

            if (locked) {
                lockGameObject.SetActive(true);
            }
            //Debug.Log($"Door: {Mathf.FloorToInt(transform.position.x)} {Mathf.FloorToInt(transform.position.y)}");
       
            tile = MainController.Instance.roomController.worldGraph
                .GetTileAt(
                    Mathf.FloorToInt(transform.position.x),
                    Mathf.FloorToInt(transform.position.y)
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
                    string[] arr = {keyParts.ToString()};
                    uiController.Log(Language.GetString(GameDictionaryType.log,"notEnoughKeys"),arr);
                    return;
                }
                else {
                    locked = false;
                    lockGameObject.SetActive(false);
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
}
