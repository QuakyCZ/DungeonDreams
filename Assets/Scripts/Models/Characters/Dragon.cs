using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Models.Characters
{
    
    public class Dragon : Enemy
    {
        protected override void Attack()
        {
            charged = false;
            ResetCoolDown();
            DoFireAttack();
        }

        private void DoFireAttack()
        {
            Debug.Log( "Enemy attacks" );
            animator.SetBool( "isAttacking", true );
        }
        
    }
}
