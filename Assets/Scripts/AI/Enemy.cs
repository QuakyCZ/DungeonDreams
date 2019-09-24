using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target;

    [SerializeField]
    private float minRange;
    [SerializeField]
    private float maxRange;

    public float speed = 2f;
    public float attackSpeed = 2;
    private float attackCooldown;
    public float distance;
    public GameObject enemyGO;
    public GameObject targetGO;
    public Transform Target {
        get { return target; }
        set { target = value; }
    }

    public Slider healthBar;

    public float maxHealth;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = attackSpeed;
        Target = targetGO.transform;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
    }
    void Update() {
        attackCooldown -= Time.deltaTime;
        if(attackCooldown <= 0 && distance <= minRange) {
            Attack();
            attackCooldown = attackSpeed;
        }
        if (health <= 0) {
            Destroy( enemyGO );
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(Target.transform.position, transform.position);
        if (distance <= maxRange && distance >= minRange) {
            FollowTarget();
        }
    }



    public void FollowTarget(bool follow = true) {       
        transform.position = Vector2.MoveTowards( transform.position, target.position, speed*Time.deltaTime );      
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,maxRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, minRange );
    }

    void Attack() {
        targetGO.GetComponent<PlayerStatsController>().TakeDamage( 2 );
    }

    public void TakeDamage(float amnt) {
        Debug.Log( "Take damage" );
        health -= amnt;
        ChangeHealthBar( health );
    }

    void ChangeHealthBar(float value) {
        healthBar.value = value;
    }
}
