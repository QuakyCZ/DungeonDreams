using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : Collidable {
    [SerializeField]protected int minDamage;
    [SerializeField]protected int maxDamage;
    public int weaponLevel;
    public float knockback;
    private Animator animator;
    private bool attack = false;
    private bool charged = true;

    public List<Collider2D> collisions { get; protected set; }

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();

        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown( KeyCode.Space )) {
            if(charged) {
                charged = false;
                Attack();
            }
            
        }
    }

    protected void Attack() {
        Debug.Log( "Attack" );
        animator.SetBool( "IsAttacking", true );
        attack = true;
    }

    protected override void OnCollide(Collider2D coll) {        
        if(coll.tag == "Enemy" && attack) {
            attack = false;
            Debug.Log( "Coll enemy" );
            Damage dmg = new Damage{
                damageAmount = Random.Range(minDamage,maxDamage+1),
                origin = transform.position,
                pushForce = knockback
            };
            coll.SendMessage( "ReceiveDamage", dmg );
        }

    }

    public void AttackEnd() {
        animator.SetBool( "IsAttacking", false );
    }

    public void Charged() {
        charged = true;
    }
}
