using UnityEngine;

public class TeleportPlayerOnInteract : MonoBehaviour
{
    public Vector3 teleportDestination;           // Set the target teleport location in the inspector
    public float interactionDistance = 3f;        // Distance at which the player can interact
    private GameObject player;
    private bool isPlayerNearby = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        isPlayerNearby = Vector3.Distance(transform.position, player.transform.position) <= interactionDistance;

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = teleportDestination;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = teleportDestination;
        }

        Debug.Log("Player teleported to: " + teleportDestination);
    }
}
