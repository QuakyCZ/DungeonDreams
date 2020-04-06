using System.Collections.Generic;
using Controllers;
using Models.Characters;
using UnityEngine;

namespace Interaction {
    public class Collidable : MonoBehaviour
    {
        protected Player player;
        protected UIController uiController;
        [HideInInspector]
        public ContactFilter2D filter;
        private BoxCollider2D _boxCollider;
        private readonly List<Collider2D> _hits = new List<Collider2D>();

        [SerializeField]protected bool doUpdate = true;

        protected virtual void Start() {
            _boxCollider = GetComponent<BoxCollider2D>();
            player = FindObjectOfType<Player>();
            uiController = FindObjectOfType<UIController>();
        }

        protected virtual void Update() {
            if ( doUpdate ) {
                _boxCollider.OverlapCollider( filter, _hits );
                for ( int i = 0; i < _hits.Count; i++ ) {
                    if (_hits[i] != null) {
                        OnCollide( _hits[i] );
                    }
                    _hits[i] = null;
                }
            }
        }

        protected virtual void OnCollide(Collider2D coll) {

        }
        
    }
}
