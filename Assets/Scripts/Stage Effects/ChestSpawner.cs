using UnityEngine;
using System.Collections;

public class ChestSpawnerWithEffects : MonoBehaviour
{
    public GameObject itemboxPrefab;    // Assign the "itembox" prefab in the Inspector
    public GameObject prisonerPrefab;   // Assign the prisoner prefab in the Inspector
    public GameObject player;           // Assign the player object in the Inspector
    public int chestCount = 10;         // Number of chests to spawn

    void Start()
    {
        SpawnItemBoxes();
    }

    void SpawnItemBoxes()
    {
        if (itemboxPrefab == null)
        {
            Debug.LogWarning("Itembox prefab not assigned!");
            return;
        }

        for (int i = 0; i < chestCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnObject();
            GameObject itembox = Instantiate(itemboxPrefab, spawnPosition, Quaternion.identity);

            CubeEffect cubeEffect = itembox.AddComponent<CubeEffect>();
            cubeEffect.spawner = this;
            cubeEffect.AssignEffect();
        }
    }

    Vector3 GetRandomPositionOnObject()
    {
        // Get the object's collider bounds
        Collider objectCollider = GetComponent<Collider>();
        if (objectCollider == null)
        {
            Debug.LogWarning("No collider found on the object! Please add a collider.");
            return transform.position;
        }

        // Get the min and max bounds
        Bounds bounds = objectCollider.bounds;

        // Generate a random position within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        // Raycast downward to find the Y position
        Vector3 startPosition = new Vector3(randomX, bounds.max.y + 1f, randomZ);
        RaycastHit hit;

        if (Physics.Raycast(startPosition, Vector3.down, out hit, bounds.size.y * 2))
        {
            return new Vector3(randomX, hit.point.y, randomZ);
        }
        else
        {
            Debug.LogWarning("No ground detected! Spawning chest at object's center.");
            return new Vector3(randomX, transform.position.y, randomZ);
        }
    }

    public class CubeEffect : MonoBehaviour
    {
        public ChestSpawnerWithEffects spawner;
        private int effectType;

        public void AssignEffect()
        {
            int randomEffect = Random.Range(1, 101);

            if (randomEffect <= 30) // 30% chance - Teleport player
            {
                effectType = 1;
                
            }
            else if (randomEffect <= 60) // 30% chance - Increase Speed
            {
                effectType = 2;
                
            }
            else if (randomEffect <= 90) // 30% chance - Spawn a prisoner
            {
                effectType = 3;
                
            }
            else // 10% chance - Nothing happens
            {
                effectType = 4;
                
            }
        }

        private void OnMouseDown()
        {
            if (spawner.player == null)
            {
                Debug.LogWarning("Player is not assigned in the Inspector!");
                return;
            }

            string effectMessage = "Nothing happened.";

            switch (effectType)
            {
                case 1: // Teleport Player
                    Vector3 teleportPosition = new Vector3(-1153.2f, 45.2f, -192f); // Example position
                    TeleportPlayer(spawner.player, teleportPosition);
                    effectMessage = "Player Teleported!";
                    break;

                case 2: // Increase Speed
                    FPSInput playerMovement = spawner.player.GetComponent<FPSInput>();
                    if (playerMovement != null)
                    {
                        playerMovement.speed += 5;
                        StartCoroutine(ResetSpeed(playerMovement, 5f));
                        effectMessage = "Player Speed Increased! (+5 for 5 seconds)";
                    }
                    break;

                case 3: // Spawn a Prisoner
                    if (spawner.prisonerPrefab != null)
                    {
                        Instantiate(spawner.prisonerPrefab, transform.position, Quaternion.identity);
                        effectMessage = "Prisoner Spawned!";
                    }
                    break;

                default:
                    effectMessage = "Nothing happened.";
                    break;
            }

            Debug.Log(effectMessage);
            Destroy(gameObject); // Destroy the chestbox after clicking
        }

        private void TeleportPlayer(GameObject player, Vector3 newPosition)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // Disable before changing position
                player.transform.position = newPosition;
                controller.enabled = true; // Re-enable after teleporting
            }
            else
            {
                player.transform.position = newPosition;
            }
        }

        private IEnumerator ResetSpeed(FPSInput player, float duration)
        {
            yield return new WaitForSeconds(duration);
            player.speed -= 5;
            Debug.Log("Player Speed Reset.");
        }
    }
}
