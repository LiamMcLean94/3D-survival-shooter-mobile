using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f; // Time between spawns
    public int initialEnemies = 1;   // Initial number of enemies
    public float spawnIncreaseRate = 0.1f; // Rate at which enemies increase
    public int maxEnemies = 5;

    private float timeSinceLastSpawn;
    private int enemiesToSpawn;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        enemiesToSpawn = initialEnemies;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            int enemiesToSpawnNow = Mathf.Min(maxEnemies - activeEnemies.Count, enemiesToSpawn);
            SpawnEnemies(enemiesToSpawnNow);
            timeSinceLastSpawn = 0;
            //enemiesToSpawn += Mathf.CeilToInt(spawnIncreaseRate); // Increase the amount over time
        }

        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    /*private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(-3f, 0, transform.position.z); // Left lane
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }*/

    private void SpawnEnemies(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(-3f, 0, transform.position.z);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
        }

        enemiesToSpawn += Mathf.CeilToInt(spawnIncreaseRate);
    }
}
