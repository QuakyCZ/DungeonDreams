using Models.Files;
using UnityEngine;

namespace Interaction {
    public class Collectable : Collidable {
        [Header("Sprites")] [SerializeField] protected Sprite spriteUncollided;
        [SerializeField] protected Sprite spriteCollided;
        [SerializeField] protected Sprite spriteCollected;
        private bool _collected;

        protected bool Collected {
            get => _collected;
            set {
                _collected = value;
                if (value == true) {
                    doUpdate = false;
                }
            }
        }

        protected SpriteRenderer objectSprite;

        protected override void OnCollide(Collider2D coll) {
            if (coll.name == "Player") {
                objectSprite = GetComponent<SpriteRenderer>();
                objectSprite.sprite = spriteCollided;
                uiController.Log(Language.GetString(GameDictionaryType.log, "interact"), new string[1] {"E"});

                if (coll.name == "Player" && Input.GetKey(KeyCode.E)) {
                    OnCollect();
                }
            }
        }

        protected virtual void OnCollect() {
            objectSprite.sprite = spriteCollected;
            Collected = true;
        }
    }
}