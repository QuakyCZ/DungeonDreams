using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Models.Characters
{
    public class Enemy : Character
    {
        [SerializeField] protected int minDamage;
        [SerializeField] protected int maxDamage;
        [Header("Death")]
        [SerializeField] protected bool dropRandom;
        [SerializeField] protected List<GameObject> dropItemsPrefabs;
        [SerializeField] protected int dropAmount;
        [SerializeField] protected int dropChanceInPercent;
        [Header("Enemy UI")]
        [SerializeField]
        private Slider healthBar;

        [SerializeField] protected Text healthText;

        #region range
        [Header("Range")]
    
        [SerializeField]
        protected float maxFollowDistance;
    
        protected float playerDistance = 100;
        #endregion

        protected GameObject targetGO;

        protected Vector2 localScale;

        protected void InstantiateEnemyParameters() {
            localScale = transform.localScale;
            targetGO = GameObject.Find( "Player" );
            target = targetGO.transform;
            healthBar.maxValue = stats.GetValue( Stats.maxHealth );
            ChangeHealthBar( stats.GetValue( Stats.health ) );

        }

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            InstantiateEnemyParameters();
        }    

        protected override void Update() {
            if ( playerDistance <= minRange && charged && stacked == false )
                Attack();
            if (stats.GetValue( Stats.health ) <= 0) {
                Die();
            }
            base.Update();
        }

        protected override void FixedUpdate() {
            if ( stacked == false ) {
                FindPlayer();
            }
        }
        protected override void Die() {
            DropItem();
            base.Die();

        }
        protected void DropItem() {
            if (dropItemsPrefabs != null && dropItemsPrefabs.Count>0) {
                for (int i = 0; i < dropAmount; i++) {
                    int r = Random.Range(0,(int)((1/(0.01*dropChanceInPercent))*dropItemsPrefabs.Count));
                    if (r < dropItemsPrefabs.Count) {
                        GameObject dropped = Instantiate(dropItemsPrefabs[r]);
                        dropped.transform.position = transform.position;
                    }
                }
            }

        }
        protected void FindPlayer() {
            playerDistance = Vector3.Distance( target.transform.position, transform.position );

            if ( playerDistance <= maxFollowDistance && playerDistance >= minRange ) {

                Vector2 moveVector = Vector2.MoveTowards( transform.position, targetGO.transform.position, abilities.GetAbilityValue( Ability.speed ) * Time.deltaTime );

                Move( moveVector );
            }
        }

        protected void Move(Vector2 moveVector) {
            transform.position = moveVector;

            Vector2 playerVector = target.transform.position;
            float deltaX = playerVector.x - transform.position.x;

            if ( deltaX < 0 ) {
                transform.localScale = new Vector2( -localScale.x, localScale.y );

            }
            else if ( deltaX > 0 ) {
                transform.localScale = new Vector2( localScale.x, localScale.y );
            }
        }
           
        /// <summary>
        /// This is the method for attack. If the attack should be basic, use base.Attack().
        /// </summary>
        protected virtual void Attack() {
            charged = false;
            ResetCoolDown();
            Debug.unityLogger.Log( "Enemy attacks" );
            animator.SetBool( "isAttacking", true );
        }

        public void AttackEnd() {
            animator.SetBool( "isAttacking", false );
            FindObjectOfType<Player>().ReceiveDamage( new Damage{origin = transform.position, damageAmount = Random.Range( minDamage, maxDamage + 1 ) } );
        }

        public void Charged() {
            Debug.Log( "Enemy - Charged" );
            charged = true;
        }

        void ChangeHealthBar(int value) {
            healthBar.value = value;
            healthText.text = value.ToString() + "/" + stats.GetValue( Stats.maxHealth );
        }

        public override void ReceiveDamage(Damage dmg) {
            base.ReceiveDamage( dmg);
            Debug.Log( "Take damage" );
            ChangeHealthBar( stats.GetValue( Stats.health ) );
            Stack();
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere( transform.position, maxFollowDistance );
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere( transform.position, minRange );
        }

    }
}
