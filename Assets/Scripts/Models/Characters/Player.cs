using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public Inventory inventory;   

    protected override void InstantiateParameters() {
        base.InstantiateParameters();
    }

    #region metody
    protected override void Start() {
        base.Start();
        inventory = Inventory.GetInstance();
    }
    public void Attack() {
        
        if(weapon.collisions.Count > 0) {
            foreach (Collider2D collision in weapon.collisions) {
                if (collision.tag == "Enemy") {
                    Enemy enemy = collision.GetComponent<Enemy>();
                    int damage = weapon.damage * (int)abilities.GetAbilityValue( Ability.strength );
                    Debug.Log( collision.tag );
                    Debug.Log( "Dealing " + damage + " damage." );
                    enemy.TakeDamage( (int)(weapon.damage * abilities.GetAbilityValue( Ability.strength )) );
                }
            }
            
        }
    }

    protected override void Move(Vector2 moveVector) {
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
