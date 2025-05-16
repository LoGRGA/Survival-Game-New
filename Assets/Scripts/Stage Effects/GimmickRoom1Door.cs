using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GimmickRoom1Door : MonoBehaviour
{
    private bool isStay = false;
    public GameObject player;
    public Transform gimmickRoom2Trans;
    public Transform bossRoomTrans;
    public GameObject stage2RoomTeleporter;

    private bool isGimmickRoom2Done;
    private AudioSource audioSource;

    private Material ogSkybox;
    private AmbientMode ogAmbientMode;
    private Color ogAmbientColor;

    //For Main Camera to Change setting when Trigger
    private Camera playerCam;
    private CameraClearFlags ogClearFlags;
    private Color ogBackgroundColor;

    private bool darkRoomActive = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        isGimmickRoom2Done = stage2RoomTeleporter.GetComponent<Stage2RoomTeleporter>().isGimmickRoom2Done;

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

    // Update is called once per frame
    void Update()
    {
        //teleport to room2
        if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && !isGimmickRoom2Done)
        {
            StartCoroutine(TeleportPlayerToRoom2());
        }

        //teleport to boos room
        else if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && isGimmickRoom2Done)
        {
            StartCoroutine(TeleportPlayerToBossRoom());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStay = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStay = false;
        }
    }

    IEnumerator TeleportPlayerToRoom2()
    {
        //stage2RoomTeleporter.GetComponent<Stage2RoomTeleporter>().isGimmickRoom2Done = true;
        ToggleDarkRoom();
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = gimmickRoom2Trans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator TeleportPlayerToBossRoom()
    {
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = bossRoomTrans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
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
}
