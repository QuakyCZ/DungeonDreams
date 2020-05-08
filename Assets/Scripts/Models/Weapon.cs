using System;
using System.Collections.Generic;
using Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models {
    public class Weapon : MonoBehaviour {
        [SerializeField] private int minDamage;
        [SerializeField] private int maxDamage;
        [SerializeField] private float chargeTime = 2f;
        private float chargeCooldown;
        private Animator _animator;
        [ReadOnly][SerializeField] private bool _charged = true;

        public List<Collider2D> Collisions { get; protected set; }

        private void Start() {
            _animator = GetComponent<Animator>();
            chargeCooldown = chargeTime;
        }

        private void Update() {
            if (!_charged) {
                chargeCooldown -= Time.deltaTime;
                if (chargeCooldown <= 0) {
                    _charged = true;
                    chargeCooldown = chargeTime;
                }
                return;
            }
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown( KeyCode.Space )) {
                if(_charged) {
                    _charged = false;
                    Attack();
                }
            }
        }
        private void Attack() {
            _animator.SetBool( "IsAttacking", true );
        }
        
        private void EnableCollisions() {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Enemy") {
                other.SendMessage("ReceiveDamage", new Damage{damageAmount = Random.Range(minDamage,maxDamage)});
            }
        }
        
        private void AttackAnimationEnd() {
            _animator.SetBool( "IsAttacking", false );
            _charged = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }
}
