using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour{
    private EnemyController _enemyController = null;
    private Animator _animator = null;

    private void Start() {
        _enemyController = GetComponent<EnemyController>();
        _animator = GetComponent<Animator>();
        _enemyController.OnWalkingBegin += OnWalkingBegin;
        _enemyController.OnWalkingEnd += OnWalkingEnd;
        _enemyController.OnAttackBegin += OnAttackBegin;
        _enemyController.OnAttackEnd += OnAttackEnd;
        _enemyController.OnTakingHitBegin += OnBeginTakingHit;
        _enemyController.OnTakingHitEnd += OnStopTakingHit;
        _enemyController.OnDyingBegin += OnBeginDying;
        _enemyController.OnDie += OnDie;
    }

    private void SetBools(bool isWalking = false, bool isAttacking = false, bool isTakingHit = false,
        bool isDying = false, bool isDead = false) {
        _animator.SetBool("IsWalking", isWalking);
        _animator.SetBool("IsAttacking", isAttacking);
        _animator.SetBool("IsTakingHit", isTakingHit);
        _animator.SetBool("IsDying", isDying);
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
        SetBools(false,false,true);
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
