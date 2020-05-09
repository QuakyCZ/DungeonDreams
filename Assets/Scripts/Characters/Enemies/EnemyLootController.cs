using Containers;
using UnityEngine;

namespace Characters.Enemies{
    public class EnemyLootController : MonoBehaviour{
        [SerializeField] private LootContainer loot = null;
        [SerializeField][Range(0,100)] private int dropChance = 50;
        private EnemyController _enemyController;
        
        void Start() {
            _enemyController = GetComponent<EnemyController>();
            if (_enemyController == null) {
                Debug.LogError("This GameObject also needs EnemyController class! Destroying myself.");
                Destroy(gameObject);
            }

            _enemyController.OnDie += DropLoot;
        }

        private void DropLoot() {
            Debug.Log("DropLoot");
            int number = Random.Range(0, 100);
            if(number >= 0 && number <=dropChance)
                Instantiate(loot.GetRandomLoot(),transform.position,Quaternion.identity);
        }
    }
}