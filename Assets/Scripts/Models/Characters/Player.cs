using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    public Weapon weapon;
    public Inventory inventory;
    #region metody
    protected override void Start() {
        base.Start();
    }
    protected override void Awake() {
        Debug.Log( "Player Awake" );
        base.Awake();
        ResetCoolDown();
        weapon = FindObjectOfType<Weapon>();
        if ( weapon != null )
            weapon.damage = damage;
        inventory = Inventory.GetInstance(true);
        Debug.Log( "Weapon damage: " + weapon.damage );
    }

    protected override void Update() {
        base.Update();
    }

    public void Attack() {
        //Debug.Log( "Attack." );
        if ( charged && stacked == false ) {
            if ( weapon.collisions.Count > 0 ) {

                foreach ( Collider2D collision in weapon.collisions ) {
                    if ( collision.tag == "Enemy" ) {
                        Enemy enemy = collision.GetComponent<Enemy>();
                        float damage = weapon.damage * abilities.GetAbilityValue( Ability.strength );
                        Debug.Log( collision.tag );
                        Debug.Log( "Dealing " + damage + " damage." );
                        enemy.TakeDamage( Mathf.FloorToInt(damage));
                    }
                }

            }
            charged = false;
        }
    }



    protected override void Move(Vector3 moveVector) {
        base.Move( moveVector );
    }

    public void MovePlayer(Vector2 moveVector) {
        Move( moveVector );
    }
    public void SetPosition(Vector2 position) {
        transform.position = position;
    }
    #endregion
}
