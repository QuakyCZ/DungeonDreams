using UnityEngine;

namespace Containers{
    
    [CreateAssetMenu(fileName = "NewLoot", menuName = "CustomContainers/Loot")]
    public class LootContainer : ScriptableObject{
        [SerializeField] private  GameObject[] lootPrefabs = null;

        public GameObject GetRandomLoot() {
            return lootPrefabs[Random.Range(0, lootPrefabs.Length - 1)];
        }
    }
}