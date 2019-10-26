using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{

    public Sprite openLootedSprite;
    protected override void OnCollide( Collider2D coll ) {
        base.OnCollide( coll );
    }
    protected override void OnCollect() {
        if ( collected ) {
            return;
        }
        base.OnCollect();
        GetComponent<SpriteRenderer>().sprite = openLootedSprite;
        int amnt = Random.Range( 1, 5 ) * player.stats.GetValue(Stats.level);
        player.inventory.ChangeValue( InventoryDefault.gold, amnt, MathOperation.Add );
        Debug.Log( "Gained " + amnt + " gold. Now you have " + player.inventory.GetValue(InventoryDefault.gold) + " gold" );
    }


}
