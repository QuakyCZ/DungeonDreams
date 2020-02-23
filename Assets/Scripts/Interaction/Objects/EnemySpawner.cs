using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public enum EnemyType {
    Dragon,
    Hound
}
[System.Serializable]
public class EnemiesToSpawn : SerializableDictionaryBase<EnemyType, int> { }
[System.Serializable]
public class EnemyPrefabs : SerializableDictionaryBase<EnemyType, GameObject> { }

public class EnemySpawner : Collidable
{

    [SerializeField]
    protected EnemiesToSpawn enemiesToSpawn;
    [SerializeField]
    protected EnemyPrefabs enemyPrefabs;

    protected List<GameObject> enemyGOList;
    protected bool triggered;
    [SerializeField]
    protected Vector3 spawnRange;


    protected override void Start() {
        base.Start();
        enemyGOList = new List<GameObject>();
        InstantiateEnemies();

    }

    protected override void OnCollide( Collider2D coll ) {
        if ( triggered ) {
            return;
        }
        if (coll.name != "Player")
            return;
        ActivateEnemies();
        triggered = true;
    }

    protected void InstantiateEnemies() {
        foreach ( EnemyType type in enemiesToSpawn.Keys ) {
            int n = enemiesToSpawn[type];
            for ( int i = 0; i < n; i++ ) {
                SpawnEnemy( type );
            }
        }
    }

    protected void ActivateEnemies() {
        foreach(GameObject enemy in enemyGOList ) {
            enemy.SetActive( true );
        }
    }

    /// <summary>
    /// Instantiates the enemy at a random position in the given range from the spawner.
    /// Game object is NOT active!
    /// </summary>
    /// <param name="enemyType">Enemy type.</param>
    protected void SpawnEnemy(EnemyType enemyType ) {
        if ( enemyPrefabs.ContainsKey( enemyType ) ) {
            GameObject enemyGO = Instantiate( enemyPrefabs[enemyType] );
            enemyGOList.Add( enemyGO );
            enemyGO.name = "enemy_" + enemyType.ToString() + "_" + (enemyGOList.Count - 1).ToString();
            enemyGO.transform.SetParent( this.transform );
            //enemyGO.transform.position = new Vector2( 0, 0 );
            float x = this.transform.position.x + Random.Range( -spawnRange.x, spawnRange.x );
            float y = this.transform.position.y + Random.Range( -spawnRange.y, spawnRange.y );
            //Debug.Log( "Spawning " + enemyType.ToString() + " at " + x + " " + y + " coordinates." );
            enemyGO.transform.position = new Vector2( x, y);
            enemyGO.gameObject.SetActive( false );
        }
    }

    protected void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube( transform.position,new Vector3(spawnRange.x*2,spawnRange.y*2,0) );
    }
}
