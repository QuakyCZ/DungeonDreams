using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    #region Parameters
    [SerializeField]
    protected float minRange;
    protected Transform target;
    [SerializeField]
    protected Animator animator;

    [SerializeField] protected float immuneTime;
    private float lastImmune;

    #region Abilities
    [Header("Abilities")]
    [SerializeField]
    private float armor;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackSpeed;
    
    [SerializeField]
    private float strength;

    [SerializeField]protected bool charged;

    private float attackCooldown;

    [SerializeField]
    protected float stackTime;
    protected float stackCoolDown;
    protected bool stacked = false;
    protected bool hasImmune = false;
    #endregion

    #region Character Stats
    [Header("Character Stats")]
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int mana;
    [SerializeField]
    private int maxMana;

    #endregion

    #region Objects
    public Abilities abilities;
    public CharacterStats stats;
    private SpriteRenderer spriteRenderer;
    #endregion



    #endregion
    protected virtual void Start() {
        charged = true;
    }
    protected virtual void Awake() {
        InstantiateParameters();
    }

    protected virtual void InstantiateParameters() {
        abilities = new Abilities( armor, attackSpeed, minRange, strength, speed );
        stats = new CharacterStats(health,maxHealth,mana,maxMana);
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        ResetCoolDown();
    }

    protected virtual void Update() {
        DoCooldowns();
    }

    protected void DoCooldowns() {
        if ( stacked == true ) {
            stackCoolDown -= Time.deltaTime;
            if ( stackCoolDown <= 0 ) {
                Debug.Log( "Unstacked" );
                stacked = false;
                ResetStackCooldown();
            }
        }
        if (hasImmune) {
            spriteRenderer.color = Color.red;
            if(Time.time - lastImmune > immuneTime) {
                hasImmune = false;
                lastImmune = Time.time;
                spriteRenderer.color = Color.white;
            }
        }
    }
    protected virtual void FixedUpdate() {}

    protected virtual void Die() {
        Destroy( gameObject );
    }
    
    protected virtual void Move(Vector3 moveVector) {
        Vector3 position = transform.position;
        position += moveVector;
        transform.position = position;
    }

    /// <summary>
    /// When player damaged enemy.
    /// </summary>
    protected void Stack() {
        Debug.Log( "Stacked" );
        stacked = true;
        ResetStackCooldown();
    }

    protected void ResetCoolDown() {
        //Debug.Log( "ResetAttackCooldown" );
        attackCooldown = 1.0f / abilities.GetAbilityValue( Ability.attackSpeed );
    }
    protected void ResetStackCooldown() {
        //Debug.Log( "Reset stack cooldown" );
        stackCoolDown = stackTime;
    }

    public virtual void ReceiveDamage(Damage dmg) {
        if (animator.GetBool("isAttacking") == true)
        {
            animator.SetBool("isAttacking",false);
            ResetCoolDown();
        }
        if (hasImmune == false) {
            hasImmune = true;
            Debug.Log( "ReceiveDamage" );
            stats.ChangeActualStats( Stats.health, -dmg.damageAmount );
            FindObjectOfType<UIController>().RefreshVisibleValue( Stats.health );

            //Vector2 pushDirection = (transform.position-dmg.origin).normalized*dmg.pushForce;
            //GetComponent<Rigidbody2D>().MovePosition( pushDirection );
            if (stats.GetValue( Stats.health ) <= 0) {
                Die();
            }
        }
    }

}
