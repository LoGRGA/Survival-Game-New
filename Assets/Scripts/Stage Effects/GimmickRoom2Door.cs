using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRoom2Door : MonoBehaviour
{
    private bool isStay = false;
    public GameObject player;
    public Transform gimmickRoom1Trans;
    public Transform bossRoomTrans;
    public GameObject stage2RoomTeleporter;

    private bool isGimmickRoom1Done;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        isGimmickRoom1Done = stage2RoomTeleporter.GetComponent<Stage2RoomTeleporter>().isGimmickRoom1Done;

        //teleport to room1
        if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && !isGimmickRoom1Done){
            StartCoroutine(TeleportPLayerToRoom1());
        }

        //teleport to boos room
        else if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && isGimmickRoom1Done){
            StartCoroutine(TeleportPLayerToBossRoom());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            isStay = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            isStay = false;
        }
    }

    IEnumerator TeleportPLayerToRoom1(){
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = gimmickRoom1Trans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator TeleportPLayerToBossRoom(){
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = bossRoomTrans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }
}
