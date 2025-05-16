using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GimmickRoom2Door : MonoBehaviour
{
    private bool isStay = false;
    public GameObject player;
    public Transform gimmickRoom1Trans;
    public Transform bossRoomTrans;
    public GameObject stage2RoomTeleporter;

    private bool isGimmickRoom1Done;
    private AudioSource audioSource;

    private Material ogSkybox;
    private AmbientMode ogAmbientMode;
    private Color ogAmbientColor;

    //For Main Camera to Change setting when Trigger
    private Camera playerCam;
    private CameraClearFlags ogClearFlags;
    private Color ogBackgroundColor;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

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
        isGimmickRoom1Done = stage2RoomTeleporter.GetComponent<Stage2RoomTeleporter>().isGimmickRoom1Done;

        //teleport to room1
        if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && !isGimmickRoom1Done)
        {
            StartCoroutine(TeleportPlayerToRoom1());
        }

        //teleport to boos room
        else if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && isGimmickRoom1Done)
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

    IEnumerator TeleportPlayerToRoom1()
    {
        //ResetDarkRoom();
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = gimmickRoom1Trans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator TeleportPlayerToBossRoom()
    {
        //ResetDarkRoom();
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = bossRoomTrans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }

    public void ResetDarkRoom()
    {
        //darkRoomActive = false;

        // Restore original lighting
        RenderSettings.skybox = ogSkybox;
        RenderSettings.ambientMode = ogAmbientMode;
        RenderSettings.ambientLight = ogAmbientColor;

        // Restore camera settings
        if (playerCam != null)
        {
            playerCam.clearFlags = ogClearFlags;
            playerCam.backgroundColor = ogBackgroundColor;
        }

        // Turn off fog
        RenderSettings.fog = false;

        Debug.Log("hello");
    }
}
