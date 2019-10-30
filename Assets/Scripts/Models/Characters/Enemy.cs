using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : Character
{
    [Header("Enemy UI")]
    [SerializeField]
    private Slider healthBar;

    #region range
    [Header("Range")]
    
    [SerializeField]
    protected float maxFollowDistance;
    
    protected float playerDistance;
    #endregion

    protected GameObject targetGO;    

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        InstantiateEnemyParameters();
    }    

    protected override void Update() {
        if ( playerDistance <= minRange && charged && stacked == false )
            Attack();
        base.Update();
    }

    protected override void FixedUpdate() {
        if ( stacked == false ) {
            playerDistance = Vector3.Distance( target.transform.position, transform.position );
            if ( playerDistance <= maxFollowDistance && playerDistance >= minRange ) {
                transform.position = Vector2.MoveTowards( transform.position, targetGO.transform.position, abilities.GetAbilityValue( Ability.speed ) * Time.deltaTime );
            }
        }
    }


    void InstantiateEnemyParameters() {
        targetGO = GameObject.Find( "Player" );
        target = targetGO.transform;
        healthBar.maxValue = stats.GetValue( Stats.maxHealth );
        ChangeHealthBar( stats.GetValue( Stats.health ) );

    }

    public void AttackEnd() {
        animator.SetBool( "isAttacking", false );
        FindObjectOfType<PlayerStatsController>().TakeDamage( 2 );

    }

    void Attack() {
        charged = false;
        ResetCoolDown();
        Debug.Log( "Enemy attacks" );
        animator.SetBool( "isAttacking", true );


    }

    public void TakeDamage(int amnt) {
        Debug.Log( "Take damage" );
        stats.ChangeActualStats(Stats.health, -amnt);
        ChangeHealthBar( stats.GetValue(Stats.health) );
        Stack();
    }

    void ChangeHealthBar(int value) {
        healthBar.value = value;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( transform.position, maxFollowDistance );
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, minRange );
    }

}
