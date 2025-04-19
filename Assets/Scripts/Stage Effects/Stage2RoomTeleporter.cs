using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2RoomTeleporter : MonoBehaviour
{
    public Transform normalRoom1;
    public Transform normalRoom2;
    public Transform stage2BossRoom;
    public Transform stage3BossRoom;
    public Transform goalRoom;
    public GameObject player;

    public DarkRoomTrigger darkRoomTriggerScript; // Reference to the dark room controller

    private List<int> visitedRooms = new List<int>();
    private int currentStage = 0; // 0: normal rooms, 1: stage 2 boss, 2: stage 3 boss, 3: goal
    private bool isPlayerNearby = false;
    private int lastRoomIndex = -1;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Press F to teleport.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void TeleportPlayer()
    {
        Vector3 targetPosition = Vector3.zero;

        // --- Leaving Room 2? Reset the dark room ---
        if (lastRoomIndex == 2 && darkRoomTriggerScript != null)
        {
            darkRoomTriggerScript.ResetDarkRoom();
        }

        if (currentStage == 0)
        {
            int roomToTeleport;

            // Choose a room not yet visited
            do
            {
                roomToTeleport = Random.Range(1, 3); // 1 or 2
            } while (visitedRooms.Contains(roomToTeleport));

            visitedRooms.Add(roomToTeleport);

            if (roomToTeleport == 1) targetPosition = normalRoom1.position;
            else if (roomToTeleport == 2) targetPosition = normalRoom2.position;

            lastRoomIndex = roomToTeleport;

            Debug.Log("Teleported to Normal Room " + roomToTeleport);

            if (visitedRooms.Count == 2)
            {
                currentStage = 1; // Next teleport will be stage 2 boss
            }

            // Entering Room 2? Activate dark mode
            if (roomToTeleport == 2 && darkRoomTriggerScript != null)
            {
                darkRoomTriggerScript.ActivateDarkRoomManually();
            }
        }
        else if (currentStage == 1)
        {
            targetPosition = stage2BossRoom.position;
            currentStage = 2;
            lastRoomIndex = 3;
            Debug.Log("Teleported to Stage 2 Boss Room");
        }
        else if (currentStage == 2)
        {
            targetPosition = stage3BossRoom.position;
            currentStage = 3;
            lastRoomIndex = 4;
            Debug.Log("Teleported to Stage 3 Boss Room");
        }
        else if (currentStage == 3)
        {
            targetPosition = goalRoom.position;
            currentStage = 4;
            lastRoomIndex = 5;
            Debug.Log("Teleported to Goal Room");
        }
        else
        {
            Debug.Log("No further teleport destinations.");
            return;
        }

        // Teleport the player
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.position = targetPosition;

        if (controller != null) controller.enabled = true;
    }
}
