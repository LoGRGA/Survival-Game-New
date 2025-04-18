using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DarkRoomTrigger : MonoBehaviour
{
    //DarkRoom GameObject
    //public GameObject darkRoomTrigger;

    //DOOOOOOOOOOOOOOOOOR
    public GameObject doorPrefab;
    public Transform[] doorSpawnPoints;

    // Start is called before the first frame update
    public Light playerSpotlight;

    private bool playerInRange = false;
    private bool darkRoomActive = false;
    private bool flightlightOn = false;

    private Material ogSkybox;
    private AmbientMode ogAmbientMode;
    private Color ogAmbientColor;

    //For Main Camera to Change setting when Trigger
    private Camera playerCam;
    private CameraClearFlags ogClearFlags;
    private Color ogBackgroundColor;

    void Start()
    {

        //darkRoomTrigger.SetActive(false);

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

        if (playerSpotlight != null)
        {
            playerSpotlight.enabled = false; // keep off until triggered
        }

        //Spawn Door At Start
        SpawnRandomDoor();


    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.O))
        {
            ToggleDarkRoom();
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.P))
        {
            ToggleFlashlight();
        }

        //if (playerInRange && Input.GetKeyDown(KeyCode.L))
        //{
        //    SpawnRandomDoor();
        //}
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

    void ToggleFlashlight()
    {
        if (playerSpotlight != null)
        {
            flightlightOn = !flightlightOn;
            playerSpotlight.enabled = flightlightOn;
            Debug.Log("Flashlight toggled: " + flightlightOn);
        }
    }

    void SpawnRandomDoor()
    {
        if (doorPrefab == null || doorSpawnPoints.Length == 0)
        {
            Debug.LogWarning("Door prefab or spawn points not set!");
            return;
        }

        int randomIndex = Random.Range(0, doorSpawnPoints.Length);
        Transform spawnPoint = doorSpawnPoints[randomIndex];
        Instantiate(doorPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInRange = true;
            /*
            // Make environment dark
            RenderSettings.skybox = null;
                RenderSettings.ambientMode = AmbientMode.Flat;
                RenderSettings.ambientLight = Color.black;

                //Set Fog
                RenderSettings.fog = true;
                RenderSettings.fogColor = Color.black; // or any color you want
                RenderSettings.fogDensity = 0.30f;

                //Change camera to solid black background
                if (playerCam != null)
                {
                    playerCam.clearFlags = CameraClearFlags.SolidColor;
                    playerCam.backgroundColor = Color.black;
                }

            if (playerSpotlight != null)
            {
                //make the light appear on the player head
                playerSpotlight.enabled = true;
                Debug.Log("Spotlight enabled!");
            }
            else
            {
                Debug.LogWarning("Spotlight reference is missing!");
            }
            */
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
                playerInRange = false;
            /*
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
            if (playerSpotlight != null)
            {
                playerSpotlight.enabled = false;
            }
            */
        }
    }
}
