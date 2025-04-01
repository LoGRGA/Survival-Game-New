using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomtopudge : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(-1131.394f, 44.78194f, -205.4929f); // The position to teleport to
    public GameObject player; // Reference to the player object
    public GameObject zombiePrefab; // Assign the zombie prefab in the inspector
    public float interactionDistance = 3f; // Distance within which the player can interact
    public float spawnDistance = 10f; // Distance in front of the player where the zombie spawns

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
            if (hit.collider.gameObject == gameObject) // Check if it's this object
            {
                TeleportPlayer(player, teleportPosition);
                SpawnZombie();
            }
        }
    }

    private void TeleportPlayer(GameObject player, Vector3 newPosition)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false; // Disable CharacterController before teleporting
            player.transform.position = newPosition;
            controller.enabled = true; // Re-enable CharacterController after teleporting
        }
        else
        {
            player.transform.position = newPosition;
        }

        Debug.Log("Player Teleported to: " + newPosition);
    }

    private void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            Vector3 spawnPosition = player.transform.position + player.transform.forward * spawnDistance;
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Zombie spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("Zombie prefab is not assigned!");
        }
    }
}
