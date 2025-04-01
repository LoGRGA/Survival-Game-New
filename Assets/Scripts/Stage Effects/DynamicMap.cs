using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMapGenerator : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject bigTreePrefab;
    public GameObject smallTreePrefab;
    public GameObject rockPrefab1;
    public GameObject rockPrefab2;
    public GameObject rockPrefab3;

    [Header("Spawn Settings")]
    public int bigTreeCount = 20;
    public int smallTreeCount = 30;
    public int rockCount = 15;

    [Header("Height Offset")]
    public float yOffset = -3f; // To prevent objects from sinking into the ground

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        if (!HasCollider())
        {
            Debug.LogWarning("No Collider found on this GameObject. Add a Collider to define the map area.");
            return;
        }

        Debug.Log("Generating new map layout...");
        SpawnObjects(bigTreePrefab, bigTreeCount);
        SpawnObjects(smallTreePrefab, smallTreeCount);
        SpawnObjects(rockPrefab1, rockCount / 3);
        SpawnObjects(rockPrefab2, rockCount / 3);
        SpawnObjects(rockPrefab3, rockCount / 3);
    }

    bool HasCollider()
    {
        return GetComponent<Collider>() != null;
    }

    void SpawnObjects(GameObject prefab, int count)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not assigned. Skipping spawn.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnMap();
            Instantiate(prefab, spawnPosition, Quaternion.Euler(0, Random.Range(0f, 360f), 0));  // Randomize rotation
        }

        Debug.Log($"{count} instances of {prefab.name} spawned!");
    }

    Vector3 GetRandomPositionOnMap()
    {
        // Get the collider of the map to determine the bounds for object placement
        Collider mapCollider = GetComponent<Collider>();
        Bounds bounds = mapCollider.bounds;

        // Generate random X and Z coordinates within the bounds of the map
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        // Start position is above the ground (just to ensure the raycast starts from above)
        Vector3 startPosition = new Vector3(randomX, bounds.max.y + 10f, randomZ); // Start a bit higher
        RaycastHit hit;

        // Perform a raycast downward to find the ground and place objects on it
        if (Physics.Raycast(startPosition, Vector3.down, out hit, bounds.size.y * 2))
        {
            // Adjust Y position to be exactly on the ground level plus offset
            return new Vector3(randomX, hit.point.y + yOffset, randomZ);
        }
        else
        {
            // If no ground is found, default to center of the map with an upward offset
            Debug.LogWarning("No ground detected! Spawning object at map center.");
            return transform.position + Vector3.up * yOffset;
        }
    }
}
