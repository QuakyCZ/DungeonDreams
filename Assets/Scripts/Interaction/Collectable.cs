using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectable : Collidable
{
    public Sprite spriteCollected;
    public Sprite spriteUncollected;
    private bool _collected;
    protected bool collected { get { return _collected; } set { _collected = value; if ( value == true ) { doUpdate = false; } } }

    protected override void OnCollide( Collider2D coll ) {
        base.OnCollide( coll );

        GetComponent<SpriteRenderer>().sprite = spriteCollected;
        uiController.Log( "Press F to interact." );

        if (coll.name == "Player" && Input.GetKey(KeyCode.F)) {
            OnCollect();
        }
    }

    protected virtual void OnCollect() {
        collected = true;
    }


}
