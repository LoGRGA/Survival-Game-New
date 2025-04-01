using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab; // Assign the chest prefab with the RNGchest script
    public int chestCount = 5; // Number of chests to spawn
    public float minSpacing = 2f; // Minimum spacing between chests

    private Vector3 areaSize; // The size of the spawn area

    void Start()
    {
        // Get the size of the GameObject this script is attached to
        Collider areaCollider = GetComponent<Collider>();
        if (areaCollider == null)
        {
            Debug.LogError("ChestSpawner requires a Collider to define the spawn area.");
            return;
        }

        areaSize = areaCollider.bounds.size;

        SpawnChests();
    }

    void SpawnChests()
    {
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < chestCount; i++)
        {
            Vector3 randomPosition;
            int attempts = 0;

            // Ensure chests don't spawn too close together
            do
            {
                randomPosition = GetRandomPosition();
                attempts++;
            }
            while (IsTooClose(randomPosition, usedPositions) && attempts < 10);

            usedPositions.Add(randomPosition);
            Instantiate(chestPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;
        float randomX = Random.Range(center.x - areaSize.x / 2, center.x + areaSize.x / 2);
        float randomZ = Random.Range(center.z - areaSize.z / 2, center.z + areaSize.z / 2);

        return new Vector3(randomX, center.y, randomZ);
    }

    bool IsTooClose(Vector3 position, List<Vector3> usedPositions)
    {
        foreach (var usedPosition in usedPositions)
        {
            if (Vector3.Distance(position, usedPosition) < minSpacing)
            {
                return true;
            }
        }
        return false;
    }
}
