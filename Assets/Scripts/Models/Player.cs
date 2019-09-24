using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    #region parametry a proměnné
    public float x;
    public float y;
    public GameObject playerGO;
    public PlayerStats stats;
    public Abilities abilities;
    public Vector3 position { get { return playerGO.transform.position; } }
    public Weapon weapon;

    #endregion


    #region konstruktory
    public Player(GameObject playerGO) {
        this.playerGO = playerGO;
        x = playerGO.transform.position.x;
        y = playerGO.transform.position.y;

        stats = new PlayerStats();
        abilities = new Abilities(1,0.3f,5,3,1);
    }

    #endregion

    #region metody
    public void Move(Vector3 moveVector) {
        playerGO.transform.Translate(moveVector);
        x = position.x;
        y = position.y;
    }

    public void Attack() {

        if(weapon.collisions.Count > 0) {
            foreach (Collider2D collision in weapon.collisions) {
                if(collision.tag == "Enemy") {
                    Enemy enemy = collision.GetComponent<Enemy>();
                    Debug.Log( collision.tag );
                    Debug.Log( abilities.GetAbilityValue( Ability.damage ) );
                    enemy.TakeDamage( abilities.GetAbilityValue( Ability.damage ) );
                }
            }
            
        }
    }
    #endregion
}
