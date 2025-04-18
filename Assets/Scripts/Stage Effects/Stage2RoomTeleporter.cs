using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2RoomTeleporter : MonoBehaviour
{
    public Transform normalRoom1;
    public Transform normalRoom2;
    public Transform bossRoom;
    public Transform finalRoom;
    public GameObject player;

    private List<int> visitedRooms = new List<int>();
    private bool isPlayerNearby = false;
    private bool hasVisitedBossRoom = false;
    private bool hasVisitedFinalRoom = false;

    private void Update()
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
        if (visitedRooms.Count < 2)
        {
            int roomToTeleport;

            // Pick a room the player hasn't visited yet
            do
            {
                roomToTeleport = Random.Range(1, 3); // 1 or 2
            } while (visitedRooms.Contains(roomToTeleport));

            visitedRooms.Add(roomToTeleport);

            Vector3 targetPosition = (roomToTeleport == 1) ? normalRoom1.position : normalRoom2.position;
            MovePlayerTo(targetPosition);

            Debug.Log("Teleported to normal room " + roomToTeleport);
        }
        else if (!hasVisitedBossRoom)
        {
            MovePlayerTo(bossRoom.position);
            hasVisitedBossRoom = true;
            Debug.Log("Teleported to boss room.");
        }
        else if (!hasVisitedFinalRoom)
        {
            MovePlayerTo(finalRoom.position);
            hasVisitedFinalRoom = true;
            Debug.Log("Teleported to final room.");
        }
        else
        {
            Debug.Log("All rooms visited.");
        }
    }

    private void MovePlayerTo(Vector3 targetPosition)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = targetPosition;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = targetPosition;
        }
    }
}
