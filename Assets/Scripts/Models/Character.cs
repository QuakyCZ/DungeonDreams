using UnityEngine;

public class Character : MonoBehaviour{
    [SerializeField] protected float minRange;
    protected Transform target;
    [SerializeField] protected Animator animator;

    [SerializeField] protected float immuneTime;
    private float lastImmune;

    private WorldGraph _worldGraph => MainController.Instance.worldGraph;

    public ClonedTile TileUnderMe => _worldGraph.GetTileAt(Mathf.FloorToInt(transform.position.x),
        Mathf.FloorToInt(transform.position.y));

    #region Abilities

    [Header("Abilities")] [SerializeField] private float armor = 0;
    [SerializeField] protected float speed = 0;
    [SerializeField] private float attackSpeed = 0;

    [SerializeField] private float strength = 0;

    [SerializeField] protected bool charged = true;

    private float attackCooldown = 0;

    [SerializeField] protected float stackTime = 0;
    protected float stackCoolDown = 0;
    protected bool stacked = false;
    protected bool hasImmune = false;

    #endregion

    #region Character Stats

    [Header("Character Stats")] [SerializeField]
    private int health = 0;

    [SerializeField] private int maxHealth = 0;
    [SerializeField] private int mana = 0;
    [SerializeField] private int maxMana = 0;

    #endregion

    #region Objects

    public Abilities abilities;
    public CharacterStats stats;
    private SpriteRenderer spriteRenderer = null;

    #endregion


    protected virtual void Start() {
        charged = true;
    }

    protected virtual void Awake() {
        InstantiateParameters();
    }

    protected virtual void InstantiateParameters() {
        abilities = new Abilities(armor, attackSpeed, minRange, strength, speed);
        stats = new CharacterStats(health, maxHealth, mana, maxMana);
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        ResetCoolDown();
    }

    protected virtual void Update() {
        DoCooldowns();
    }

    /// <summary>
    /// Does cooldowns charge, stack, immune.
    /// </summary>
    protected void DoCooldowns() {
        if (charged == false) {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0) {
                Charge();
            }
        }

        if (stacked == true) {
            stackCoolDown -= Time.deltaTime;
            if (stackCoolDown <= 0) {
                Unstack();
            }
        }

        if (hasImmune) {
            spriteRenderer.color = Color.red;
            if (Time.time - lastImmune > immuneTime) {
                RemoveImmune();
            }
        }
    }

    protected virtual void FixedUpdate() { }

    ////////////////////////////
    ///
    ///    Cooldown methods
    /// 
    /// ////////////////////////
    /// <summary>
    /// This is for the animation event.
    /// </summary>
    protected void Charge() {
        charged = true;
        ResetCoolDown();
    }

    protected void Unstack() {
        Debug.Log("Unstacked");
        stacked = false;
        ResetStackCooldown();
    }

    protected void RemoveImmune() {
        hasImmune = false;
        lastImmune = Time.time;
        spriteRenderer.color = Color.white;
    }

    protected virtual void Die() {
        Destroy(gameObject);
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
        Debug.Log("Stacked");
        stacked = true;
        ResetStackCooldown();
    }

    protected void ResetCoolDown() {
        //Debug.Log( "ResetAttackCooldown" );
        attackCooldown = 1.0f / abilities.GetAbilityValue(Ability.attackSpeed);
    }

    protected void ResetStackCooldown() {
        //Debug.Log( "Reset stack cooldown" );
        stackCoolDown = stackTime;
    }

    public virtual void ReceiveDamage(Damage dmg) {
        if (animator.GetBool("isAttacking") == true) {
            animator.SetBool("isAttacking", false);
            charged = false;
            ResetCoolDown();
        }

        if (hasImmune == false) {
            hasImmune = true;
            Debug.Log("ReceiveDamage");
            stats.ChangeActualStats(Stats.health, -dmg.damageAmount);
            FindObjectOfType<UIController>().RefreshVisibleValue(Stats.health);

            //Vector2 pushDirection = (transform.position-dmg.origin).normalized*dmg.pushForce;
            //GetComponent<Rigidbody2D>().MovePosition( pushDirection );
            if (stats.GetValue(Stats.health) <= 0) {
                Die();
            }
        }
    }
}