using System;
using System.Collections.Generic;
using Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models {
    public class Weapon : MonoBehaviour {
        [SerializeField]protected int minDamage;
        [SerializeField]protected int maxDamage;
        private Animator _animator;
        private bool _charged = true;

        public List<Collider2D> Collisions { get; protected set; }

        private void Start() {
            _animator = GetComponent<Animator>();
        }

        private void Update() {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown( KeyCode.Space )) {
                if(_charged) {
                    _charged = false;
                    Attack();
                }
            }
        }
        private void Attack() {
            Debug.Log( "Attack" );
            _animator.SetBool( "IsAttacking", true );
        }
        
        public void AttackEnd() {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Enemy") {
                other.SendMessage("ReceiveDamage", new Damage{damageAmount = Random.Range(minDamage,maxDamage)});
            }
        }
        
        public void Charged() {
            _animator.SetBool( "IsAttacking", false );
            _charged = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }
}
