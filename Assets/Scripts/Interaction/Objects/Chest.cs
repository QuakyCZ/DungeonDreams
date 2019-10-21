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
        int amnt = Random.Range( 1, 5 ) * playerStatsController.GetStatsValue(Stats.level);
        playerStatsController.ChangeValue( Stats.gold, amnt );
        Debug.Log( "Gained " + amnt + " gold. Now you have " + playerStatsController.GetStatsValue(Stats.gold) + " gold" );
    }


}
