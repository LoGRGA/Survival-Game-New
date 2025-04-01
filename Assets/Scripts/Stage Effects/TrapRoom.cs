using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign the zombie prefab in the Inspector
    public GameObject player; // Reference to the player
    public float interactionDistance = 3f; // Distance within which the player can interact
    public float spawnDistance = 2f; // Distance in front of the player to spawn the zombie

    void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.F))
        {
            CheckInteraction();
        }
    }

    void CheckInteraction()
    {
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        RaycastHit hit;

        // Check if the player is looking at the object and within distance
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject) // Ensure it's this object
            {
                SpawnZombie();
            }
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            Vector3 spawnPosition = player.transform.position + player.transform.forward * spawnDistance;
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Zombie Spawned in front of the player!");
        }
        else
        {
            Debug.LogWarning("Zombie Prefab is not assigned!");
        }
    }
}