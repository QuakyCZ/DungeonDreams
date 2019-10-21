using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    protected PlayerStatsController playerStatsController;
    protected UIController uiController;
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private List<Collider2D> hits = new List<Collider2D>();
    public Sprite openSprite;
    public Sprite closedSprite;
    protected bool doUpdate = true;

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        playerStatsController = FindObjectOfType<PlayerStatsController>();
        uiController = FindObjectOfType<UIController>();
    }

    protected virtual void Update() {
        if ( doUpdate ) {
            boxCollider.OverlapCollider( filter, hits );
            for ( int i = 0; i < hits.Count; i++ ) {
                if ( hits[i] != null && hits[i].name == "Player" ) {
                    OnCollide( hits[i] );
                }
                hits[i] = null;
            }
        }
    }

    protected virtual void OnCollide(Collider2D coll) {
        GetComponent<SpriteRenderer>().sprite = openSprite;
        uiController.Log( "Press F to collect the loot." );
    }

}
