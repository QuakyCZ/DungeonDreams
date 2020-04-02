using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace Models {
    public class Weapon : Collidable {
        [SerializeField]protected int minDamage;
        [SerializeField]protected int maxDamage;
        public int weaponLevel;
        public float knockback;
        private Animator _animator;
        private bool _attack = false;
        private bool _charged = true;

        public List<Collider2D> Collisions { get; protected set; }

        protected override void Start() {
            base.Start();
            _animator = GetComponent<Animator>();
        }

        protected override void Update() {
            base.Update();

            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown( KeyCode.Space )) {
                if(_charged) {
                    _charged = false;
                    Attack();
                }
            
            }
        }

        protected void Attack() {
            Debug.Log( "Attack" );
            _animator.SetBool( "IsAttacking", true );
            _attack = true;
        }

        protected override void OnCollide(Collider2D coll) {        
            if(coll.CompareTag("Enemy") && _attack) {
                _attack = false;
                Debug.Log( "Coll enemy" );
                Damage dmg = new Damage{
                    damageAmount = Random.Range(minDamage,maxDamage),
                    origin = transform.position,
                    pushForce = knockback
                };
                coll.SendMessage( "ReceiveDamage", dmg );
            }

        }

        public void AttackEnd() {
            _animator.SetBool( "IsAttacking", false );
        }

        public void Charged() {
            _charged = true;
        }
    }
}
