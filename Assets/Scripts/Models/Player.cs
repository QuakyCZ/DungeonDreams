﻿using System.Collections;
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
    #endregion

    #region konstruktory
    public Player(GameObject playerGO) {
        this.playerGO = playerGO;
        x = playerGO.transform.position.x;
        y = playerGO.transform.position.y;

        stats = new PlayerStats();
        abilities = new Abilities();
    }
    #endregion

    #region metody
    public void Move(Vector3 moveVector) {
        playerGO.transform.Translate(moveVector);
        x = position.x;
        y = position.y;
    }

    public void Attack() {
        
    }
    #endregion
}
