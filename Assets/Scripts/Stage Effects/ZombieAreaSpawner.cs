using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAreaSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints; // Assign 8 spawn points in the inspector
    public float spawnInterval = 10f;
    public int zombiesPerSpawn = 2;

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            for (int i = 0; i < zombiesPerSpawn; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}