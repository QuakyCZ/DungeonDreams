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

    protected Vector2 localScale;

    protected void InstantiateEnemyParameters() {
        localScale = transform.localScale;
        targetGO = GameObject.Find( "Player" );
        target = targetGO.transform;
        healthBar.maxValue = stats.GetValue( Stats.maxHealth );
        ChangeHealthBar( stats.GetValue( Stats.health ) );

    }

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
            FindPlayer();
        }
    }

    protected void FindPlayer() {
        playerDistance = Vector3.Distance( target.transform.position, transform.position );

        if ( playerDistance <= maxFollowDistance && playerDistance >= minRange ) {

            Vector2 moveVector = Vector2.MoveTowards( transform.position, targetGO.transform.position, abilities.GetAbilityValue( Ability.speed ) * Time.deltaTime );

            Move( moveVector );
        }
    }

    protected void Move(Vector2 moveVector) {
        transform.position = moveVector;

        Vector2 playerVector = target.transform.position;
        float deltaX = playerVector.x - transform.position.x;

        if ( deltaX < 0 ) {
            transform.localScale = new Vector2( -localScale.x, localScale.y );

        }
        else if ( deltaX > 0 ) {
            transform.localScale = new Vector2( localScale.x, localScale.y );
        }
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
