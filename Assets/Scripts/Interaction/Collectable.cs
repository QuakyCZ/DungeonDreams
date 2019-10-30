using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    private bool _collected;
    protected bool collected { get { return _collected; } set { _collected = value; if ( value == true ) { doUpdate = false; } } }

    protected override void OnCollide( Collider2D coll ) {
        base.OnCollide( coll );

        GetComponent<SpriteRenderer>().sprite = openSprite;
        uiController.Log( "Press F to collect the loot." );

        if (coll.name == "Player" && Input.GetKey(KeyCode.F)) {
            OnCollect();
        }
    }

    protected virtual void OnCollect() {
        collected = true;
    }


}
