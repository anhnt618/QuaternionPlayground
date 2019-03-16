using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] float minSpawnDelay = 0.5f;
    [SerializeField] float maxSpawnDelay = 1.5f;
    [SerializeField] Transform[] spawnPoints;
    Transform currentSpawnPoint;
    int index;
    bool canSpawn = false;
    float secondsBetweenSpawn;
    GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Start()
    {
        index = Random.Range(0, spawnPoints.Length);
        currentSpawnPoint = spawnPoints[index];
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            secondsBetweenSpawn = Random.Range(minSpawnDelay, maxSpawnDelay);
            index = Random.Range(0, spawnPoints.Length);
            currentSpawnPoint = spawnPoints[index];
            yield return new WaitForSeconds(secondsBetweenSpawn);
            if (gameController.NumberOfEnemiesToSpawn>0)
            {
                gameController.NumberOfEnemiesToSpawn--;
                GameObject enemy = Instantiate(EnemyPrefab, currentSpawnPoint.position, Quaternion.identity);
                enemy.transform.parent = gameObject.transform;
            }
        }
    }
}
