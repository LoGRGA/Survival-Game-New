using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;    // Assign your zombie prefab in the Inspector
    public GameObject mapObject;       // Assign your map object (with collider) in the Inspector
    public float spawnInterval = 5f;   // Time interval between spawns
    public float yOffset = 1f;         // Offset to prevent clipping into the ground

    void Start()
    {
        StartCoroutine(SpawnZombieRoutine());
    }

    IEnumerator SpawnZombieRoutine()
    {
        while (true)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab == null || mapObject == null)
        {
            Debug.LogError("Zombie prefab or map object is not assigned!");
            return;
        }

        Vector3 spawnPosition = GetRandomPositionOnMap();

        if (spawnPosition != Vector3.zero)
        {
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // Ensure Rigidbody works properly
            Rigidbody rb = zombie.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // Enable physics
            }

            Debug.Log("Zombie spawned at: " + spawnPosition);
        }
    }

    Vector3 GetRandomPositionOnMap()
    {
        Collider mapCollider = mapObject.GetComponent<Collider>();
        if (mapCollider == null)
        {
            Debug.LogWarning("No Collider found on the map object! Add a Collider to define the spawn area.");
            return Vector3.zero;
        }

        Bounds bounds = mapCollider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 startPosition = new Vector3(randomX, bounds.max.y + 10f, randomZ); // Start above the map
        RaycastHit hit;

        // Raycast down to find the ground with a larger distance
        if (Physics.Raycast(startPosition, Vector3.down, out hit, bounds.size.y * 5)) // Increased raycast distance
        {
            return new Vector3(randomX, hit.point.y + yOffset, randomZ); // Adjust Y to match ground height
        }
        else
        {
            Debug.LogWarning("No ground detected! Zombie not spawned.");
            return Vector3.zero;
        }
    }
}
