using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    protected Player player;
    protected UIController uiController;
    [HideInInspector]
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private List<Collider2D> hits = new List<Collider2D>();

    protected bool doUpdate = true;

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>();
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

    }

}
