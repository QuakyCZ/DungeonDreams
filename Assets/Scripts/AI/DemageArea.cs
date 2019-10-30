using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemageArea : MonoBehaviour
{
    private PlayerStatsController playerStatsController;
    float second = 2;    
    void Awake() {
        playerStatsController = FindObjectOfType<PlayerStatsController>();
    }
    /// <summary>
    /// While player collides with enemy
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision) {
        //Debug.Log( "On Trigger Stay" );
        second -= Time.deltaTime;
        if(collision.tag == "Enemy" && second <= 0) {
            Debug.Log( "Damage!" );
            playerStatsController.TakeDamage( 2 );
            second = 2;
        }
    }
}
