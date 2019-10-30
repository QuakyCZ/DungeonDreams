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

    protected bool charged;

    private float attackCooldown;

    [SerializeField]
    protected float stackTime;
    protected float stackCoolDown;
    protected bool stacked = false;
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

    #region Weapon
    [Header("Weapon")]
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected WeaponType weaponType;
    #endregion

    #region Objects
    public Abilities abilities;
    public CharacterStats stats;


    #endregion



    #endregion
    protected virtual void Start() {

    }
    protected virtual void Awake() {
        InstantiateParameters();
    }

    protected virtual void Update() {
        if ( stats.GetValue( Stats.health ) <= 0 ) {
            Destroy( this.gameObject );
        }
        DoCooldowns();
    }

    protected void DoCooldowns() {
        //Debug.Log( "DoCooldowns" );
        if ( charged == false ) {
            attackCooldown -= Time.deltaTime;
            if ( attackCooldown <= 0 ) {
                Debug.Log( "Charged" );
                charged = true;
                ResetCoolDown();
            }
        }
        if ( stacked == true ) {
            stackCoolDown -= Time.deltaTime;
            if ( stackCoolDown <= 0 ) {
                Debug.Log( "Unstacked" );
                stacked = false;
                ResetStackCooldown();
            }
        }
    }


    protected virtual void FixedUpdate() {

    }

    protected virtual void InstantiateParameters() {        
        abilities = new Abilities(armor,attackSpeed,minRange,strength,speed);
        stats = new CharacterStats();

        animator = GetComponent<Animator>();
        ResetCoolDown();
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


}
