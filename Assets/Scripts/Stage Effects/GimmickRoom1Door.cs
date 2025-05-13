using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRoom1Door : MonoBehaviour
{
    private bool isStay = false;
    public GameObject player;
    public Transform gimmickRoom2Trans;
    public Transform bossRoomTrans;
    public GameObject stage2RoomTeleporter;

    private bool isGimmickRoom2Done;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        isGimmickRoom2Done = stage2RoomTeleporter.GetComponent<Stage2RoomTeleporter>().isGimmickRoom2Done;
    }

    // Update is called once per frame
    void Update()
    {
        //teleport to room2
        if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && !isGimmickRoom2Done){
            StartCoroutine(TeleportPlayerToRoom2());
        }

        //teleport to boos room
        else if (player != null && Input.GetKeyDown(KeyCode.F) && isStay && isGimmickRoom2Done){
            StartCoroutine(TeleportPlayerToBossRoom());
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

    IEnumerator TeleportPlayerToRoom2(){
        //stage2RoomTeleporter.GetComponent<Stage2RoomTeleporter>().isGimmickRoom2Done = true;
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = gimmickRoom2Trans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator TeleportPlayerToBossRoom(){
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = bossRoomTrans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }
}
