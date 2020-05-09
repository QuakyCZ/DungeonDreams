using System.Linq;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Containers{
    [CreateAssetMenu(fileName = "NewEnemies", menuName = "CustomContainers/Enemies")]
    public class EnemiesContainer : ScriptableObject{
        [SerializeField] private EnemyPrefabs enemyPrefabs = null;

        public GameObject GetRandomLoot() {
            return enemyPrefabs.ElementAt(Random.Range(0,enemyPrefabs.Count-1)).Value;
        }
    }
}