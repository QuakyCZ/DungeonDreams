using Controllers;
using Models.Files;
using Models.Inventory;
using UnityEngine;

namespace Interaction.Objects {
    public class Door : Collectable {
        [SerializeField] private AudioSource lockedSound = null;
        [SerializeField] private AudioSource unlockSound = null;
        
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
                interacting = false;
            }
            else {
                Debug.Log( "Door are closed." );            
                GetComponent<SpriteRenderer>().sprite = closedSprite;
                animator.SetInteger( OpenDoorAnim, 0 );
                animator.SetBool( Opened, false );
                _opened = false;
                tile.isWalkable = false;
                interacting = false;
            }
        }

        protected override void PlaySound() {
            Debug.Log("Play sound");
            if (IsLocked()) {
                lockedSound.Play();
            }
            else {
                openSound.Play();
            }
        }

        protected override void OnCollect() {
            if (interacting) {
                return;
            }
            interacting = true;
            if (locked) {
                if (player.inventory.GetValue( InventoryDefault.key ) < keyParts) {
                    PlaySound();
                    string[] arr = {keyParts.ToString()};
                    uiController.Log(Language.GetString(GameDictionaryType.log,"notEnoughKeys"),arr);
                    interacting = false;
                    return;
                }
                else {
                    unlockSound.Play();
                    locked = false;
                    lockGameObject.SetActive(false);
                    interacting = false;
                    return;
                }
            }
            if(_opened == false) {
                PlaySound();
                Debug.Log( "Opening the door." );
                animator.SetInteger( OpenDoorAnim, 1 );
            }
            else {
                Debug.Log("Closing the door.");
                forbiddenArea.SetActive(true);
                animator.SetInteger(OpenDoorAnim, -1);
            }
        }

        public bool IsLocked() {
            return locked;
        }
    }
}
