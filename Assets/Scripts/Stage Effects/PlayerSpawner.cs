using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject mapObject; // Assign your map object in the Inspector
    public float yOffset = 0.5f; // Offset to lift the player slightly above the ground

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (mapObject == null)
        {
            Debug.LogError("Map object is not assigned! Please assign the map object in the Inspector.");
            return;
        }

        Vector3 spawnPosition = GetRandomPositionOnMap();

        if (spawnPosition != Vector3.zero)
        {
            // Move the player to the random position
            transform.position = spawnPosition;
            Debug.Log("Player spawned at: " + spawnPosition);
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

        Vector3 startPosition = new Vector3(randomX, bounds.max.y + 10f, randomZ); // Start raycast above
        RaycastHit hit;

        // Raycast down to find the ground
        if (Physics.Raycast(startPosition, Vector3.down, out hit, bounds.size.y * 2))
        {
            return new Vector3(randomX, hit.point.y + yOffset, randomZ); // Adjust Y to match ground height
        }
        else
        {
            Debug.LogWarning("No ground detected! Spawning player at default position.");
            return transform.position + Vector3.up * yOffset;
        }
    }
}
