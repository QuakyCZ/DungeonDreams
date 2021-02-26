using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyController : MonoBehaviour{
    [Header("Basic Settings")] [SerializeField]
    private bool destroyOnDeath = false;
    [SerializeField]
    private int health = 10;
    [ReadOnly] [SerializeField] private int currentHealth;
    public int MaxHealth => health;
    public int CurrentHealth => currentHealth;

    [Header("Movement")] [SerializeField] private float speed = 2f;
    [SerializeField] private float minRange = 0;
    [SerializeField] private int maxPathLength = 10;

    [Header("Cooldowns")] [SerializeField] private float attackChargeTime = 2f;
    private float attackChargeCooldown;
    [ReadOnly] [SerializeField] private bool isCharged = false;

    [Header("Animations (For debug)")] [ReadOnly] [SerializeField]
    private bool isWalking = false;

    [ReadOnly] [SerializeField] private bool isAttacking = false;
    [ReadOnly] [SerializeField] private bool isTakingHit = false;
    [ReadOnly] [SerializeField] private bool isDying = false;
    [ReadOnly] [SerializeField] private bool isDead = false;


    private MainController _mainController => MainController.Instance;
    private WorldGraph _worldGraph;
    private Player player;

    private List<Vector2> path = null;
    private int currentPathIndex = 0;

    private ClonedTile _tileUnderMe =>
        _worldGraph.GetTileAt(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

    public System.Action OnWalkingBegin;
    public System.Action OnWalkingEnd;

    public System.Action OnAttackBegin;
    public System.Action OnAttackEnd;

    public System.Action OnTakingHitBegin;
    public System.Action OnTakingHitEnd;

    public System.Action OnDyingBegin;
    public System.Action OnDyingEnd;
    public System.Action OnDie;

    public System.Action OnHealthChanged;

    private void Awake() {
        currentHealth = health;
        attackChargeCooldown = attackChargeTime;
    }

    void Start() {
        _worldGraph = _mainController.worldGraph;
        player = _mainController.player;
    }

    // Update is called once per frame
    void Update() {
        
        if(!_mainController.doUpdate)
            return;
        
        if (isDead) return;
        if (isAttacking) return;
        if (isTakingHit) return;

        if (isCharged == false) {
            attackChargeCooldown -= Time.deltaTime;
            if (attackChargeCooldown <= 0) {
                isCharged = true;
                attackChargeCooldown = attackChargeTime;
            }
        }

        if (player.transform.position.x < transform.position.x) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void FixedUpdate() {
        if(!_mainController.doUpdate)
            return;
        
        if (isDead) return;

        // I can't move when I do something else.
        if (isTakingHit) return;
        if (isAttacking) return;
       
        MoveOnPath();
    }

    private void MoveOnPath() {
        // Am I next to the player?
        if (Vector2.Distance(transform.position, player.transform.position) > minRange) {
            // Do I have path?
            if (path == null) {
                // No -> find it.
                path = _worldGraph.FindVectorPath(transform.position, player.transform.position);
                
                if (path!=null && path.Count > maxPathLength) path = null;
                return;
            }

            Vector3 targetPosition = path[currentPathIndex];

            if (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
                if (isWalking == false) StartMovement();
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                Move(moveDir);
            }
            else {
                currentPathIndex++;

                if (currentPathIndex >= path.Count ||
                    CheckPlayerPosition() == false) {
                    ResetPath();
                }
            }
        }
        else {
            if (isWalking) StopMovement();
            if (isCharged && !isAttacking) StartAttack();
        }
    }

    private void StartMovement() {
        isWalking = true;
        OnWalkingBegin?.Invoke();
    }

    private void ResetPath() {
        path = null;
        currentPathIndex = 0;
    }

    private void StopMovement() {
        isWalking = false;
        OnWalkingEnd?.Invoke();
        ResetPath();
    }

    private void Move(Vector3 moveDir) {
        transform.position += moveDir * (speed * Time.deltaTime);
    }

    /// <summary>
    /// Checks if the player is still at the end of the founded path.
    /// </summary>
    /// <returns>True if player is still there, otherwise false.</returns>
    private bool CheckPlayerPosition() {
        if (_worldGraph.GetTileAt(Mathf.FloorToInt(path.Last().x), Mathf.FloorToInt(path.Last().y)) !=
            player.TileUnderMe) {
            return false;
        }

        return true;
    }

    private void StartAttack() {
        isCharged = false;
        isAttacking = true;
        if (OnAttackBegin == null) {
            StopAttack();
        }
        else {
            OnAttackBegin?.Invoke();
        }
    }

    /// <summary>
    /// Called from animation.
    /// </summary>
    private void StopAttack() {
        OnAttackEnd?.Invoke();
        isAttacking = false;
    }

    private void StartTakingHit() {
        isTakingHit = true;
        if (OnTakingHitBegin == null) {
            StopTakingHit();
        }
        else {
            OnTakingHitBegin?.Invoke();
        }
    }

    /// <summary>
    /// Called from animation.
    /// </summary>
    private void StopTakingHit() {
        OnTakingHitEnd?.Invoke();
        isTakingHit = false;
    }

    private void StartDying() {
        isDying = true;
        if (OnDyingBegin == null) {
            EndDying();
        }

        OnDyingBegin?.Invoke();

        GetComponent<PolygonCollider2D>().enabled = false;
    }

    private void EndDying() {
        OnDie?.Invoke();
        isDying = false;
        isDead = true;
        if(destroyOnDeath)
            Destroy(gameObject);
    }

    public void ReceiveDamage(Damage damage) {
        StopMovement();
        StopAttack();
        StartTakingHit();

        currentHealth -= damage.damageAmount;
        if (currentHealth <= 0) {
            StartDying();
        }

        OnHealthChanged?.Invoke();
    }


    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minRange);
    }
}