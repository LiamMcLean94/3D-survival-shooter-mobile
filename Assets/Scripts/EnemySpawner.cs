using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f; // Time between spawns
    public int initialEnemies = 1;   // Initial number of enemies
    public float spawnIncreaseRate = 0.1f; // Rate at which enemies increase

    private float timeSinceLastSpawn;
    private int enemiesToSpawn;

    private void Start()
    {
        enemiesToSpawn = initialEnemies;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemies();
            timeSinceLastSpawn = 0;
            enemiesToSpawn += Mathf.CeilToInt(spawnIncreaseRate); // Increase the amount over time
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(-3f, 0, transform.position.z); // Left lane
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
