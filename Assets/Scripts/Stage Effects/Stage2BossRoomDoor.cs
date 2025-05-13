using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2BossRoomDoor : MonoBehaviour
{
    private bool isStay = false;
    public GameObject player;
    public Transform stage3BossRoom;

    public GameObject stage2;
    public GameObject stage3;

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
            StartCoroutine(TeleportPLayerToStage3BossRoom());
            stage2.SetActive(false);
            stage3.SetActive(true);
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

    IEnumerator TeleportPLayerToStage3BossRoom(){
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = stage3BossRoom.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }
}
