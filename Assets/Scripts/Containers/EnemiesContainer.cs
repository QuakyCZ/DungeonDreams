using System.Linq;
using UnityEngine;

namespace Containers{
    [CreateAssetMenu(fileName = "NewEnemies", menuName = "CustomContainers/Enemies")]
    public class EnemiesContainer : ScriptableObject{
        [SerializeField] private EnemyPrefabs enemyPrefabs = new EnemyPrefabs();

        public GameObject GetRandomEnemy() {
            return enemyPrefabs.ElementAt(Random.Range(0, enemyPrefabs.Keys.Count)).Value;
        }

        public GameObject GetEnemy(EnemyType type) {
            if (enemyPrefabs.ContainsKey(type)) {
                return enemyPrefabs[type];
            }

            return null;
        }
    }
}