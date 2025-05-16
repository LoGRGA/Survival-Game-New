using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    public bool isGimmickRoom1Done = false;
    public bool isGimmickRoom2Done = false;

    private Material ogSkybox;
    private AmbientMode ogAmbientMode;
    private Color ogAmbientColor;

    //For Main Camera to Change setting when Trigger
    private Camera playerCam;
    private CameraClearFlags ogClearFlags;
    private Color ogBackgroundColor;

    private bool darkRoomActive = false;

    void Start()
    {
        // Store the original ambient light color
        ogAmbientColor = RenderSettings.ambientLight;
        // Store the original Skybox Material
        ogSkybox = RenderSettings.skybox;
        //Store the original Ambient Mode
        ogAmbientMode = RenderSettings.ambientMode;

        //Find and save camera settings in the inspector
        playerCam = Camera.main;
        if (playerCam != null)
        {
            ogClearFlags = playerCam.clearFlags;
            ogBackgroundColor = playerCam.backgroundColor;
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerNearby = false;
    }

    private void TeleportPlayer()
    {
        int roomToTeleport = Random.Range(1, 3);
        if (roomToTeleport == 1)
        {
            isGimmickRoom1Done = true;
            TeleportPlayerTo(normalRoom1);
        }
        else
        {
            isGimmickRoom2Done = true;
            TeleportPlayerTo(normalRoom2);
            ToggleDarkRoom();
        }
    }

    private void TeleportPlayerTo(Transform room)
    {
        // Teleport the player
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.position = room.position;

        if (controller != null) controller.enabled = true;
    }
    
    void ToggleDarkRoom()
    {
        darkRoomActive = !darkRoomActive;

        if (darkRoomActive)
        {
            /*
            if (playerSpotlight != null)
                playerSpotlight.enabled = true;
            */
            // Make environment dark
            RenderSettings.skybox = null;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = Color.black;

            //Change camera to solid black background
            if (playerCam != null)
            {
                playerCam.clearFlags = CameraClearFlags.SolidColor;
                playerCam.backgroundColor = Color.black;
            }

            //Set Fog
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black; // or any color you want
            RenderSettings.fogDensity = 0.30f;

        }
        else
        {
            // Restore original settings for the lighting
            RenderSettings.skybox = ogSkybox;
            RenderSettings.ambientMode = ogAmbientMode;
            RenderSettings.ambientLight = ogAmbientColor;

            //turn off fog 
            RenderSettings.fog = false;

            //Restore orginall camera settings
            if (playerCam != null)
            {
                playerCam.clearFlags = ogClearFlags;
                playerCam.backgroundColor = ogBackgroundColor;
            }
            /*
            if (playerSpotlight != null)
            {
                playerSpotlight.enabled = false;
            } */
        }

       

    }

/*
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
        */
}
