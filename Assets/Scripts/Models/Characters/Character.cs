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

    private float _attackCooldown = 3f;
    protected float attackCooldown { get { return _attackCooldown; } set { _attackCooldown = value; } }
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

    public Weapon weapon;
    #endregion

    protected float stackTime;
    protected float stackCoolDown;
    protected bool stacked = false;

    #endregion

    protected virtual void Start() {
        InstantiateParameters();
    }

    protected virtual void Update() {

    }

    protected virtual void FixedUpdate() {

    }

    protected virtual void InstantiateParameters() {        
        abilities = new Abilities(armor,attackSpeed,minRange,strength,speed);
        attackCooldown = attackCooldown/abilities.GetAbilityValue( Ability.attackSpeed );
        stats = new CharacterStats();
        weapon = FindObjectOfType<Weapon>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Move(Vector2 moveVector) {
        Vector2 position = transform.position;
        position += moveVector;
        transform.position = position;
    }
    
    protected void Stack() {
        Debug.Log( "Stacked" );
        stackCoolDown = stackTime;
        stacked = true;
    }

    protected virtual void CheckStack() {        
    }


}
