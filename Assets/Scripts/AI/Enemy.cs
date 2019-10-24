﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

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

    public float stackTime;
    float stackCoolDown;
    bool stacked = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = attackSpeed;
        Target = targetGO.transform;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        ChangeHealthBar( health );
        animator = GetComponent<Animator>();
    }
    void Update() {
        CheckStack();

        if (health <= 0) {
            Destroy( enemyGO );
        }
        
    }

    void CheckStack() {
        if (stacked == false && distance <= minRange) {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0) {
                animator.SetBool( "isAttacking", true );
                attackCooldown = attackSpeed;
            }
            else {
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
    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(Target.transform.position, transform.position);
        if (distance <= maxRange && distance >= minRange) {
            FollowTarget();
        }
    }



    public void FollowTarget(bool follow = true) {
        Vector3 targetPosition = targetGO.transform.position;
        transform.position = Vector2.MoveTowards(transform.position,targetPosition, speed*Time.deltaTime);
        
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,maxRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, minRange );
    }

    void Attack() {
               
        FindObjectOfType<PlayerStatsController>().TakeDamage( 2 );
        
    }

    public void TakeDamage(float amnt) {
        Debug.Log( "Take damage" );
        health -= amnt;
        ChangeHealthBar( health );
        Stack();
    }

    void ChangeHealthBar(float value) {
        healthBar.value = value;
    }

    void Stack() {
        Debug.Log( "Stacked" );
        stackCoolDown = stackTime;
        stacked = true;
    }

    public void AttackEnd() {
        animator.SetBool( "isAttacking", false );
        Attack();
    }
}
