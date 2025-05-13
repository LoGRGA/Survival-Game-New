using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3BossRoomDoor : MonoBehaviour
{
    private bool isStay = false;
    public GameObject player;
    public Transform cureRoomTrans;

    public GameObject stage3;
    public GameObject cureRoom;

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
        //teleport to room2
        if (player != null && Input.GetKeyDown(KeyCode.F) && isStay){
            StartCoroutine(TeleportPlayerToCureRoom());
            stage3.SetActive(false);
            cureRoom.SetActive(true);
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

    IEnumerator TeleportPlayerToCureRoom(){
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = cureRoomTrans.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }
}
