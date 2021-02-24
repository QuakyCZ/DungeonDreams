using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class EnemiesToSpawn : SerializableDictionaryBase<EnemyType, int> { }
[System.Serializable]
public class EnemyPrefabs : SerializableDictionaryBase<EnemyType, GameObject> { }