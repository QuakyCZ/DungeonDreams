using System.Collections.Generic;
using System.Linq;
using Controllers;
using Models.Files;
using UnityEngine;
using UnityEngine.UI;

namespace Models.Characters {
    public class Enemy : Character {
        #region variables

        [SerializeField] protected int minDamage = 0;
        [SerializeField] protected int maxDamage = 0;
        [Header("Death")] [SerializeField] protected bool dropRandom = false;
        [SerializeField] protected List<GameObject> dropItemsPrefabs = null;
        [SerializeField] protected int dropAmount = 0;
        [SerializeField] protected int dropChanceInPercent = 100;
        [Header("Enemy UI")] [SerializeField] private Slider healthBar = null;

        [SerializeField] protected Text healthText = null;

        #region range

        [Header("Range")] [SerializeField] protected float maxFollowDistance = 0;

        protected float playerDistance => Vector2.Distance(transform.position, targetGO.transform.position);

        protected List<Vector2> path = null;
        protected int currentPathIndex = 0;
        private GameObject lineGO = null;
        private LineRenderer line = null;

        #endregion

        protected GameObject targetGO;

        protected Vector2 localScale;
        protected Vector2 healthBarLocalScale;
        protected Vector2 healthTextLocalScale;

        private float pathCountDown = 5;
        private bool findPath = true;

        #endregion

        protected void InstantiateEnemyParameters() {
            localScale = transform.localScale;
            healthBarLocalScale = healthBar.transform.localScale;
            healthTextLocalScale = healthText.transform.localScale;
                
            targetGO = GameObject.Find("Player");
            target = targetGO.transform;
            healthBar.maxValue = stats.GetValue(Stats.maxHealth);
            ChangeHealthBar(stats.GetValue(Stats.health));
        }

        protected override void Awake() {
            base.Awake();
            InstantiateEnemyParameters();
        }

        protected override void Start() {
            base.Start();
            lineGO = Instantiate(MainController.Instance.lineGO);
            line = lineGO.GetComponent<LineRenderer>();
        }

        protected override void Update() {
            base.Update();
            if (playerDistance <= minRange && charged && stacked == false)
                Attack();
            if (stats.GetValue(Stats.health) <= 0) {
                Die();
            }
        }

        protected override void FixedUpdate() {
            if (stacked == false) {
                if (Vector2.Distance(transform.position, targetGO.transform.position) > minRange) {
                    if (path == null) {
                        if (findPath) {
                            path = MainController.Instance.worldGraph.FindVectorPath(transform.position,
                                targetGO.transform.position);
                            if (path == null) {
                                findPath = false;
                            }
                            else {
                                currentPathIndex = 0;
                            }
                        }
                        else {
                            if (pathCountDown > 0) {
                                pathCountDown -= Time.deltaTime;
                            }
                            else {
                                findPath = true;
                                pathCountDown = 5;
                            }
                        }
                    }
                    else {
                        if (ConfigFile.Get().GetDebug("path_lines")) {
                            line.positionCount = path.Count;

                            for (int i = 0; i < path.Count; i++) {
                                line.SetPosition(i, path[i]);
                            }
                        }

                        Vector3 targetPosition = path[currentPathIndex];

                        if (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
                            Vector3 moveDir = (targetPosition - transform.position).normalized;
                            Move(moveDir);
                        }
                        else {
                            currentPathIndex++;

                            if (currentPathIndex >= path.Count ||
                                CheckPlayerPosition(new Vector2(path.Last().x, path.Last().y)) == false) {
                                StopMovement();
                            }
                        }
                    }
                }
                else {
                    StopMovement();
                }
            }
        }

        private bool CheckPlayerPosition(Vector3 position) {
            ClonedTile playerTile =
                MainController.Instance.worldGraph.GetTileAt((int) targetGO.transform.position.x,
                    (int) targetGO.transform.position.y);
            ClonedTile lastTile = MainController.Instance.worldGraph.GetTileAt((int) position.x, (int) position.y);
            if (playerTile != lastTile) {
                return false;
            }

            return true;
        }

        private bool CheckEmptyTile(Vector3 position) {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            ClonedTile tile =
                MainController.Instance.worldGraph.GetTileAt(
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y)
                );
            foreach (var enemy in enemies) {
                ClonedTile enemyTile = MainController.Instance.worldGraph.GetTileAt(
                    Mathf.FloorToInt(enemy.transform.position.x),
                    Mathf.FloorToInt(enemy.transform.position.y)
                );
                if (tile == enemyTile) return true;
            }

            return false;
        }

        protected void StopMovement() {
            path = null;
            currentPathIndex = 0;
        }

        protected override void Die() {
            DropItem();
            base.Die();
        }

        protected void DropItem() {
            if (dropItemsPrefabs != null && dropItemsPrefabs.Count > 0) {
                for (int i = 0; i < dropAmount; i++) {
                    int r = Random.Range(0, (int) ((1 / (0.01 * dropChanceInPercent)) * dropItemsPrefabs.Count));
                    if (r < dropItemsPrefabs.Count) {
                        GameObject dropped = Instantiate(dropItemsPrefabs[r]);
                        dropped.transform.position = transform.position;
                    }
                }
            }
        }


        protected override void Move(Vector3 moveVector) {
            transform.position += moveVector * (speed * Time.deltaTime);

            Vector2 playerVector = target.transform.position;
            float deltaX = playerVector.x - transform.position.x;
            
            if (deltaX < 0) {
                transform.localScale = new Vector2(-localScale.x, localScale.y);
                healthBar.transform.localScale = new Vector2(-healthBarLocalScale.x, healthBarLocalScale.y);
                healthText.transform.localScale = new Vector2(-healthTextLocalScale.x, healthTextLocalScale.y);
            }
            else if (deltaX > 0) {
                
                transform.localScale = new Vector2(localScale.x, localScale.y);
                healthBar.transform.localScale = new Vector2(healthBarLocalScale.x, healthBarLocalScale.y);
                healthText.transform.localScale = new Vector2(healthTextLocalScale.x, healthTextLocalScale.y);
            }
        }

        /// <summary>
        /// This is the method for attack. If the attack should be basic, use base.Attack().
        /// </summary>
        protected virtual void Attack() {
            charged = false;
            ResetCoolDown();
            Debug.unityLogger.Log("Enemy attacks");
            animator.SetBool("isAttacking", true);
        }

        public void AttackEnd() {
            animator.SetBool("isAttacking", false);
            FindObjectOfType<Player>().ReceiveDamage(new Damage
                {origin = transform.position, damageAmount = Random.Range(minDamage, maxDamage + 1)});
        }


        void ChangeHealthBar(int value) {
            healthBar.value = value;
            healthText.text = value.ToString() + "/" + stats.GetValue(Stats.maxHealth);
        }

        public override void ReceiveDamage(Damage dmg) {
            base.ReceiveDamage(dmg);
            Debug.Log("Take damage");
            ChangeHealthBar(stats.GetValue(Stats.health));
            Stack();
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxFollowDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, minRange);
        }
    }
}