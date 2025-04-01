using UnityEngine;
using System.Collections;

public class FixedCubeSpawner : MonoBehaviour
{
    public GameObject itemboxPrefab; // Assign the "itembox" prefab in the Inspector
    public GameObject prisonerPrefab; // Assign the prisoner prefab in the Inspector
    public GameObject player; // Assign the player object in the Inspector
    public int cubeCount = 10;

    private Vector3 topLeft = new Vector3(-1203.7f, 41.4f, -246.8f);
    private Vector3 topRight = new Vector3(-1079.2f, 41.4f, -247.3f);
    private Vector3 bottomLeft = new Vector3(-1074f, 41.4f, -377.9f);
    private Vector3 bottomRight = new Vector3(-1189.2f, 41.4f, -380.6f);

    void Start()
    {
        SpawnItemBoxes();
    }

    void SpawnItemBoxes()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            float randomX = Random.Range(bottomRight.x, topRight.x);
            float randomZ = Random.Range(topLeft.z, bottomLeft.z);
            Vector3 spawnPosition = new Vector3(randomX, topLeft.y, randomZ);

            GameObject itembox = Instantiate(itemboxPrefab, spawnPosition, Quaternion.identity);

            CubeEffect cubeEffect = itembox.AddComponent<CubeEffect>();
            cubeEffect.spawner = this;
            cubeEffect.AssignEffect(); // Assign effect and color
        }
    }

    public class CubeEffect : MonoBehaviour
    {
        public FixedCubeSpawner spawner;
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
                    Vector3 teleportPosition = new Vector3(-1153.2f, 45.2f, -192f); // Bottom Right
                    TeleportPlayer(spawner.player, teleportPosition);
                    effectMessage = "Player Teleported to Bottom Right!";
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
            Destroy(gameObject); // Destroy itembox after clicking
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
