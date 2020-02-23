using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character {
    public Inventory inventory;
    [SerializeField]protected Camera minimapCamera;
    #region metody
    protected override void Start() {
        base.Start();
    }
    protected override void Awake() {
        Debug.Log( "Player Awake" );
        base.Awake();
        ResetCoolDown();
        inventory = Inventory.GetInstance(true);
    }

    protected override void Update() {
        if (stats.GetValue( Stats.health ) <= 0) {
            Die();
        }
        base.Update();
    }

    protected override void Move(Vector3 moveVector) {
        base.Move( moveVector );
        if ( moveVector.x < 0 ) {
            transform.localScale = new Vector3( -.5f, .5f, 0 );
        }
        else if(moveVector.x > 0 ) {
            transform.localScale = new Vector3( .5f, .5f, 0 );
        }
        Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y,-10);
        minimapCamera.transform.position = Camera.main.transform.position;
    }

    public void MovePlayer(Vector2 moveVector) {
        Move( moveVector );
    }
    public void SetPosition(Vector2 position) {
        transform.position = position;
    }

    public override void ReceiveDamage(Damage dmg) {
        base.ReceiveDamage( dmg );
    }
    #endregion
}
