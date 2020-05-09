using UnityEngine;

public class Collectable : Collidable{
    [Header("Sprites")] [SerializeField] protected Sprite spriteUncollided;
    [SerializeField] protected Sprite spriteCollided;
    [SerializeField] protected Sprite spriteCollected;
    protected SpriteRenderer objectSprite;
    protected bool interacting = false;

    [Header("Sound")] [SerializeField] protected AudioSource openSound = null;

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


    protected override void OnCollide(Collider2D coll) {
        if (coll.name == "Player") {
            objectSprite = GetComponent<SpriteRenderer>();
            objectSprite.sprite = spriteCollided;
            uiController.Log(Language.GetString(GameDictionaryType.log, "interact"), new string[1] {"E"});

            if (coll.name == "Player" && Input.GetKeyDown(KeyCode.E)) {
                OnCollect();
            }
        }
    }

    protected virtual void OnCollect() {
        if (interacting) {
            return;
        }

        interacting = true;
        PlaySound();
        objectSprite.sprite = spriteCollected;
        Collected = true;
    }

    protected virtual void PlaySound() {
        if (openSound != null)
            openSound.Play();
    }
}