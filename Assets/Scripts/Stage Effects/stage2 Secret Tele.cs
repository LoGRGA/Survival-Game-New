using UnityEngine;

public class TeleportPlayerOnInteract : MonoBehaviour
{
    //public Vector3 teleportDestination;           // Set the target teleport location in the inspector
    public Transform cureRoomTrans;
    public float interactionDistance = 3f;        // Distance at which the player can interact
    private GameObject player;
    private bool isPlayerNearby = false;

    public GameObject stage2;
    public GameObject cureRoom;

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
            stage2.SetActive(false);
            cureRoom.SetActive(true);
            controller.enabled = false;
            player.transform.position = cureRoomTrans.position;
            controller.enabled = true;
        }
        else
        {
            stage2.SetActive(false);
            cureRoom.SetActive(true);
            player.transform.position = cureRoomTrans.position;
        }

        Debug.Log("Player teleported to: " + cureRoomTrans);
    }
}
