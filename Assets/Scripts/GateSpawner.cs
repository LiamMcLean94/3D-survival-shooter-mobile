using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    public GameObject duplicationGatePrefab;
    public GameObject upgradeGatePrefab;
    public float spawnInterval = 3f; // Time between gate spawns
    public float spawnZDistance = 20f; // Distance ahead to spawn gates

    private float timeSinceLastSpawn;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnRandomGate();
            timeSinceLastSpawn = 0;
        }
    }

    private void SpawnRandomGate()
    {
        Vector3 spawnPosition = new Vector3(3f, 0, transform.position.z + spawnZDistance); // Right lane

        // Randomly choose between duplication and upgrade gate
        GameObject gatePrefab = Random.value > 0.5f ? duplicationGatePrefab : upgradeGatePrefab;
        Instantiate(gatePrefab, spawnPosition, Quaternion.identity);
    }
}
