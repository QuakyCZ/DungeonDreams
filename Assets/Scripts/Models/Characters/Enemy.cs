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
    
    protected float distance;
    #endregion

    protected GameObject targetGO;    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InstantiateEnemyParameters();
    }    

    protected override void Update() {
        base.Update();

        CheckStack();

        if (stats.GetValue(Stats.health) <= 0) {
            Destroy( this );
        }
        
    }

    protected override void FixedUpdate() {
        distance = Vector2.Distance( target.transform.position, transform.position );
        if (distance <= maxFollowDistance && distance >= minRange) {
            FollowTarget();
        }
    }


    void InstantiateEnemyParameters() {
        targetGO = GameObject.Find( "Player" );
        target = targetGO.transform;
        healthBar.maxValue = stats.GetValue( Stats.maxHealth );
        ChangeHealthBar( stats.GetValue( Stats.health ) );
        
    }

    protected void FollowTarget() {
        Move( Vector2.MoveTowards( transform.position, targetGO.transform.position, maxFollowDistance ) );
    }

    protected override void Move(Vector2 moveVector) {
        transform.position = moveVector;        
    }

    public void AttackEnd() {
        animator.SetBool( "isAttacking", false );
        Attack();
    }

    void Attack() {               
        FindObjectOfType<PlayerStatsController>().TakeDamage( 2 );        
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

    protected override void CheckStack() {
        if (stacked == false && distance <= minRange) {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0) {
                animator.SetBool( "isAttacking", true );
                attackCooldown = 3f / abilities.GetAbilityValue( Ability.attackSpeed );
            }
        }
        else if (stacked) {
            stackCoolDown -= Time.deltaTime;
            //Debug.Log( stackCoolDown );
            if (stackCoolDown <= 0) {
                stacked = false;
                //Debug.Log( "Unstacked" );
            }
        }
    }

    

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( transform.position, maxFollowDistance );
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, minRange );
    }

}
