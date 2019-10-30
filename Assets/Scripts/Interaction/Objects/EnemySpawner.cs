using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType {
    dragon,
    hound
}
public class EnemySpawner : Collidable
{
    [SerializeField]
    protected List<EnemyType> enemiesToSpawn;
    [SerializeField]
    protected List<GameObject> enemyPrefabs;
    protected bool triggered;

    protected override void Start() {
        base.Start();
    }

    protected override void OnCollide( Collider2D coll ) {
        if ( triggered ) {
            return;
        }

        foreach (EnemyType enemy in enemiesToSpawn ) {

            switch ( enemy ) {
                case EnemyType.hound:
                    Instantiate( enemyPrefabs[0] );
                    break;
                case EnemyType.dragon:
                    Instantiate( enemyPrefabs[1] );
                    break;

            }
        }

    }
}
