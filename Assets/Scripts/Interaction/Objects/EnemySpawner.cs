using System.Collections.Generic;
using Containers;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemySpawner : Collidable{
    [SerializeField] private EnemiesContainer enemyPrefabs = null;
    [SerializeField] private int enemiesToSpawn = 3;
    protected List<GameObject> enemyGOList;
    protected bool triggered;
    [SerializeField] protected Vector3 spawnRange;


    protected override void Start() {
        base.Start();
        enemyGOList = new List<GameObject>();
        InstantiateEnemies();
    }

    protected override void OnCollide(Collider2D coll) {
        if (triggered) {
            return;
        }

        if (coll.name != "Player")
            return;
        ActivateEnemies();
        triggered = true;
    }

    private void InstantiateEnemies() {
        for (int i = 0; i < enemiesToSpawn; i++) {
            Vector3 newPosition = new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                transform.position.y + Random.Range(-spawnRange.y, spawnRange.y), 0);
            GameObject newEnemy = Instantiate(enemyPrefabs.GetRandomEnemy(), newPosition, Quaternion.identity);
            enemyGOList.Add(newEnemy);
            newEnemy.SetActive(false);
            newEnemy.transform.SetParent(transform);
        }
    }

    private void ActivateEnemies() {
        foreach (GameObject enemy in enemyGOList) {
            enemy.SetActive(true);
        }
    }

    protected void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRange.x * 2, spawnRange.y * 2, 0));
    }
}