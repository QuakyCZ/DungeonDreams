using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectable : Collidable
{
    [Header("GFX")]
    [SerializeField] protected Sprite spriteUncollided;
    [SerializeField] protected Sprite spriteCollided;
    [SerializeField] protected Sprite spriteCollected;
    private bool _collected;
    protected bool collected { get { return _collected; } set { _collected = value; if ( value == true ) { doUpdate = false; } } }
    protected SpriteRenderer objectSprite;

    protected override void OnCollide( Collider2D coll ) {
        if (coll.name == "Player") {        
            objectSprite = GetComponent<SpriteRenderer>();
            objectSprite.sprite = spriteCollided;
            uiController.Log( "Stiskni F pro interakci." );

            if (coll.name == "Player" && Input.GetKey( KeyCode.F )) {
                OnCollect();
            }
        }
    }

    protected virtual void OnCollect() {
        objectSprite.sprite = spriteCollected;
        collected = true;
    }


}
