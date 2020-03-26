using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Models.Characters
{
    
    public class Dragon : Enemy
    {
        private enum AttackType {
            fireAttack = 0
        }

        protected override void FixedUpdate() {
            if (playerDistance > minRange) {
                Move((targetGO.transform.position-transform.position).normalized);
            }
        }

        private Dictionary<AttackType, MethodInfo> attacks;
        private AttackType _attackType;
        protected override void Awake() {
            base.Awake();
            attacks = new Dictionary<AttackType, MethodInfo>();
            attacks.Add(AttackType.fireAttack,this.GetType().GetMethod("DoFireAttack"));
        }

        protected override void Attack()
        {
            charged = false;
            ResetCoolDown();
            var a = Random.Range(0, Enum.GetValues(typeof(AttackType)).Length);
            AttackType attackType = (AttackType) Enum.Parse(typeof(AttackType),Enum.GetName(typeof(AttackType), a));
            attacks[attackType].Invoke(this, null);

        }

        public void DoFireAttack()
        {
            Debug.Log( "Enemy attacks" );
            animator.SetBool( "isAttacking", true );
        }
        
    }
}
