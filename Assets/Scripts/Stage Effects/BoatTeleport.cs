using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTeleport : MonoBehaviour
{
    public Transform teleportPoint;
    public GameObject player;
    private bool isStay = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.F) && isStay){
            StartCoroutine(TeleportPLayer(player));
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

    IEnumerator TeleportPLayer(GameObject gameObject){
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.position = teleportPoint.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().enabled = true;
    }
}
