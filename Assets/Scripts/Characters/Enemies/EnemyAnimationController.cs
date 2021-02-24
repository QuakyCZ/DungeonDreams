using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour{
    [Header("Enabled Animations")] 
    [SerializeField] private bool movementAnimation = false;
    [SerializeField] private bool attackAnimation = false;
    [SerializeField] private bool takingHitAnimation = false;
    [SerializeField] private bool dyingAnimation = false;
    [SerializeField] private bool deadAnimation = false;

    private EnemyController _enemyController = null;
    private Animator _animator = null;

    private void Start() {
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null) {
            Debug.LogError("This GameObject also needs EnemyController class! Destroying myself.");
            Destroy(gameObject);
        }
        _animator = GetComponent<Animator>();
        if (movementAnimation) {
            _enemyController.OnWalkingBegin += OnWalkingBegin;
            _enemyController.OnWalkingEnd += OnWalkingEnd;
        }

        if (attackAnimation) {
            _enemyController.OnAttackBegin += OnAttackBegin;
            _enemyController.OnAttackEnd += OnAttackEnd;
        }

        if (takingHitAnimation) {
            _enemyController.OnTakingHitBegin += OnBeginTakingHit;
            _enemyController.OnTakingHitEnd += OnStopTakingHit;
        }

        if (dyingAnimation) {
            _enemyController.OnDyingBegin += OnBeginDying;
        }

        if (deadAnimation) {
            _enemyController.OnDie += OnDie;
        }
    }

    private void SetBools(bool isWalking = false, bool isAttacking = false, bool isTakingHit = false,
        bool isDying = false, bool isDead = false) {
        if(movementAnimation)
            _animator.SetBool("IsWalking", isWalking);
        if(attackAnimation)
            _animator.SetBool("IsAttacking", isAttacking);
        if(takingHitAnimation)
            _animator.SetBool("IsTakingHit", isTakingHit);
        if(dyingAnimation)
            _animator.SetBool("IsDying", isDying);
        if(deadAnimation)
            _animator.SetBool("IsDead", isDead);
    }

    private void OnWalkingBegin() {
        SetBools(true);
    }

    private void OnWalkingEnd() {
        SetBools();
    }

    private void OnAttackBegin() {
        SetBools(false, true);
    }

    private void OnAttackEnd() {
        SetBools();
    }

    private void OnBeginTakingHit() {
        SetBools(false, false, true);
    }

    private void OnStopTakingHit() {
        SetBools();
    }

    private void OnBeginDying() {
        SetBools(false, false, false, true);
    }

    private void OnDie() {
        SetBools(false, false, false, false, true);
    }
}