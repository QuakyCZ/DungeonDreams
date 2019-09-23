using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject targetGO;
    public Transform Target {
        get { return target; }
        set { target = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = attackSpeed;
        Target = targetGO.transform;
    }
    void Update() {
        attackCooldown -= Time.deltaTime;
        if(attackCooldown <= 0 && distance <= minRange) {
            Attack();
            attackCooldown = attackSpeed;
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
}
